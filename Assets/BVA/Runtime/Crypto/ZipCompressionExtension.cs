using System.IO;
namespace BVA.FileEncryptor
{
    public static class ZipCompressionExtension
    {
        /// <summary>
        /// In-memory GLB creation helper. Useful for platforms where no filesystem is available (e.g. WebGL).
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static void SaveBVACompressed(this GLTFSceneExporter gLTFSceneExporter,string filePath, string sceneName, string password = Confidential.DEFAULT_PASSWORD)
        {
            using (var stream = new MemoryStream())
            {
                gLTFSceneExporter.SaveGLBToStream(stream, sceneName);
                stream.Flush();
                ZipCompression.ZipStream(stream, filePath, Path.GetFileName(filePath), password);
            }
        }
    }
}