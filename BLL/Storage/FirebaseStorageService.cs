using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace BLL.Storage
{
    /// <summary>
    /// Implementación usando Google Cloud Storage (bucket de Firebase Storage).
    /// Requiere un Service Account JSON con permisos de Storage Admin/Writer.
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

            var obj = await _client.UploadObjectAsync(
                bucket: _options.Bucket,
                objectName: objectName,
                contentType: contentType,
                source: content,
                cancellationToken: ct
            ).ConfigureAwait(false);

            if (_options.MakePublicOnUpload)
            {
                // Hacer el objeto público (ACL legacy). Alternativa: firmar URL temporal.
                await _client.UpdateObjectAclAsync(_options.Bucket, objectName, new[]
                {
                    new Google.Apis.Storage.v1.Data.ObjectAccessControl
                    {
                        Entity = "allUsers",
                        Role = "READER"
                    }
                }, cancellationToken: ct).ConfigureAwait(false);

                // URL pública estándar
                return $"https://storage.googleapis.com/{WebUtility.UrlEncode(_options.Bucket)}/{WebUtility.UrlEncode(objectName)}";
            }

            // Retorna la ruta interna
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
            catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                // No existe; para nuestro caso lo tratamos como "eliminado"
                return false;
            }
        }
    }
}
