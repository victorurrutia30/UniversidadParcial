using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;           // <- Necesario para PredefinedObjectAcl
using Google.Cloud.Storage.V1;

namespace BLL.Storage
{
    /// <summary>
    /// Implementación usando Google Cloud Storage (bucket de Firebase Storage).
    /// Requiere un Service Account JSON con permisos de Storage (p.ej. Storage Object Admin).
    /// </summary>
    public class FirebaseStorageService : IFileStorageService
    {
        private readonly StorageClient _client;
        private readonly FirebaseStorageOptions _options;

        public FirebaseStorageService(FirebaseStorageOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(_options.Bucket))
                throw new ArgumentException("FirebaseStorageOptions.Bucket no puede ser vacío.");
            if (string.IsNullOrWhiteSpace(_options.CredentialsFile))
                throw new ArgumentException("FirebaseStorageOptions.CredentialsFile no puede ser vacío.");

            var credential = GoogleCredential.FromFile(_options.CredentialsFile);
            _client = StorageClient.Create(credential);
        }

        public async Task<string> UploadAsync(string objectName, Stream content, string contentType, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(objectName))
                throw new ArgumentException("objectName es requerido.", nameof(objectName));
            if (content is null)
                throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrWhiteSpace(contentType))
                contentType = "application/octet-stream";

            // Si queremos que sea público al subir, usamos PredefinedAcl = PublicRead
            var uploadOptions = new UploadObjectOptions();
            if (_options.MakePublicOnUpload)
            {
                uploadOptions.PredefinedAcl = PredefinedObjectAcl.PublicRead;
            }

            var obj = await _client.UploadObjectAsync(
                bucket: _options.Bucket,
                objectName: objectName,
                contentType: contentType,
                source: content,
                options: uploadOptions,
                cancellationToken: ct
            ).ConfigureAwait(false);

            // Si es público, devolvemos la URL HTTP pública.
            if (_options.MakePublicOnUpload)
            {
                return $"https://storage.googleapis.com/{WebUtility.UrlEncode(_options.Bucket)}/{WebUtility.UrlEncode(objectName)}";
            }

            // Si no es público, devolvemos la ruta gs://
            return $"gs://{_options.Bucket}/{objectName}";
        }

        public async Task<bool> DeleteAsync(string objectName, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(objectName))
                return false;

            try
            {
                await _client.DeleteObjectAsync(_options.Bucket, objectName, cancellationToken: ct).ConfigureAwait(false);
                return true;
            }
            catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // No existe; lo tratamos como "no eliminado"
                return false;
            }
        }
    }
}
