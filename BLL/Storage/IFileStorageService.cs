using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;


namespace BLL.Storage
{
    /// <summary>
    /// Servicio de almacenamiento de archivos en la nube.
    /// Subida y eliminación en Firebase Storage.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Sube un archivo al bucket. Devuelve la URL pública (si el objeto es de lectura pública)
        /// o la ruta gs:// si no se configura como público.
        /// </summary>
        /// <param name="objectName">Ruta/nombre del objeto en el bucket, ej: "docs/actas/acta-123.pdf"</param>
        /// <param name="content">Stream del archivo</param>
        /// <param name="contentType">MIME type (ej: "application/pdf")</param>
        Task<string> UploadAsync(string objectName, Stream content, string contentType, CancellationToken ct = default);

        /// <summary>
        /// Elimina un objeto del bucket por su nombre.
        /// </summary>
        Task<bool> DeleteAsync(string objectName, CancellationToken ct = default);
    }
}