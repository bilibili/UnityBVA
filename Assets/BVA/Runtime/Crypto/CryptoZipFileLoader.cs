using System.IO;
using System;
using System.Threading.Tasks;
using BVA.FileEncryptor;
using BVA.Extensions;

namespace BVA.Loader
{
    /// <summary>
    /// 加密压缩文件加载方法，如果是zip文件会先读取到MemoryStream，否则直接加载
    /// </summary>
    public class CryptoZipFileLoader : IDataLoader, IDataLoader2
    {
        private readonly string _rootDirectoryPath;
        private readonly string _password;
        public CryptoZipFileLoader(string zipFile, string password = Confidential.DEFAULT_PASSWORD)
        {
            _rootDirectoryPath = zipFile;
            _password = password;
        }

        public Task<Stream> LoadStreamAsync(string relativeFilePath)
        {
            return Task.Run(() => LoadStream(relativeFilePath));
        }

        public Stream LoadStream(string relativeFilePath)
        {
            if (relativeFilePath == null)
            {
                throw new ArgumentNullException("relativeFilePath is null");
            }

            relativeFilePath = relativeFilePath.ConvertSpaceString();
            string pathToLoad = Path.Combine(_rootDirectoryPath, relativeFilePath);
            if (!File.Exists(pathToLoad))
            {
                throw new FileNotFoundException($"File {pathToLoad} not found", relativeFilePath);
            }
            FileStream fs = File.OpenRead(pathToLoad);
            if (ZipCompression.IsZipFile(fs))
                return ZipCompression.UnZipMemoryStream(File.OpenRead(pathToLoad), _password);
            else
                return fs;
        }
    }
}
