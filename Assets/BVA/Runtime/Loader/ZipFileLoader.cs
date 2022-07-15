using System.IO;
using System;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Linq;
using UnityEngine;

namespace BVA.Loader
{
    public class ZipFileLoader : IDataLoader, IDataLoader2
    {
        private readonly ZipArchive zipArchive;
        private readonly string zipArchivePath;
        public ZipFileLoader(string zipFile)
        {
            zipArchive = ZipFile.Open(zipFile, ZipArchiveMode.Update);
            zipArchivePath = zipFile;
        }

        public Task<Stream> LoadStreamAsync(string relativeFilePath)
        {
            return Task.Run(() => LoadStream(relativeFilePath));
        }

        public Stream LoadStream(string relativeFilePath)
        {
            if (relativeFilePath == null)
            {
                throw new ArgumentNullException("relativeFilePath");
            }

            ZipArchiveEntry zipArchiveEntry = zipArchive.GetEntry(relativeFilePath);

            if (zipArchiveEntry == null)
            {
                zipArchiveEntry = SearchGLTFFile();
            }
            if (zipArchiveEntry == null)
            {
                throw new FileNotFoundException($"Buffer file {relativeFilePath} not found  in {zipArchivePath}");
            }

            Stream result = zipArchiveEntry.Open();
            return result;
        }

        private ZipArchiveEntry SearchGLTFFile()
        {
            var result = zipArchive.Entries.Where(x => Path.GetExtension(x.Name) == ".gltf").ToArray();
            if (result.Length == 0)
            {
                throw new FileNotFoundException($" file {zipArchivePath} is not contain any gltf file");
            }
            if (result.Length > 1)
            {
                Debug.LogWarning($" file {zipArchivePath} contain more than one gltf file");
            }
            return result[0];
        }
    }
}
