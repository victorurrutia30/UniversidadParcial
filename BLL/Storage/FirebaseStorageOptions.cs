using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Storage
{
    /// <summary>
    /// Opciones para conectar a Firebase Storage.
    /// </summary>
    public sealed class FirebaseStorageOptions
    {
        public string Bucket { get; init; } = string.Empty;
        public string CredentialsFile { get; init; } = string.Empty;
        /// <summary>
        /// Si quieres que los objetos sean públicos tras subirlos.
        /// </summary>
        public bool MakePublicOnUpload { get; init; } = true;
    }
}

