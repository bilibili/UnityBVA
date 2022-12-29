using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using UnityEngine;

namespace BVA.FileEncryptor
{
    public static class ZipCompression
    {
        static readonly byte[] ZipHeader = new byte[] { 0x50, 0x4B, 0x03, 0x04 };
        public static void ZipFile(string fileToZip, string zipedFile, string password)
        {
            if (!File.Exists(fileToZip))
            {
                throw new FileNotFoundException(fileToZip + " is not exist!");
            }

            FileStream ZipFile = File.OpenRead(fileToZip);
            string entry = Path.GetFileName(fileToZip);
            ZipStream(ZipFile, zipedFile, entry, password);
        }
        public static void ZipStream(FileStream stream, string zipedFile, string entryName, string password, int compressionLevel = 6)
        {
            FileStream zipFile = null;
            ZipOutputStream zipStream = null;
            try
            {
                zipFile = File.Create(zipedFile);
                zipStream = new ZipOutputStream(zipFile);
                zipStream.Password = password;
                ZipEntry zipEntry = new ZipEntry(entryName);
                zipStream.PutNextEntry(zipEntry);
                zipStream.SetLevel(compressionLevel);
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Close();
                zipStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (zipFile != null)
                {
                    zipFile.Close();
                }
            }
        }
        public static void ZipStream(MemoryStream stream, string zipedFile, string entryName, string password, int compressionLevel = 6)
        {
            FileStream zipFile = null;
            ZipOutputStream zipStream = null;
            try
            {
                zipFile = File.Create(zipedFile);
                zipStream = new ZipOutputStream(zipFile);
                zipStream.Password = password;
                ZipEntry zipEntry = new ZipEntry(entryName);
                zipStream.PutNextEntry(zipEntry);
                zipStream.SetLevel(compressionLevel);
                zipStream.Write(stream.GetBuffer(), 0, (int)stream.Length);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (zipFile != null)
                {
                    zipFile.Close();
                }
            }
        }

        public static void UnZipFile(string zipedFile, string strDirectory, string password, bool overWrite = true)
        {
            UnZipStream(File.OpenRead(zipedFile), strDirectory, password, overWrite);
        }
        public static bool IsZipFile(Stream stream)
        {
            byte[] buffer = new byte[4];
            stream.Read(buffer, 0, 4);
            return buffer[0] == ZipHeader[0] && buffer[1] == ZipHeader[1] && buffer[2] == ZipHeader[2] && buffer[3] == ZipHeader[3];
        }
        public static void UnZipStream(Stream stream, string strDirectory, string password, bool overWrite = true, int blockSize = 4096)
        {

            if (strDirectory == "")
                strDirectory = Directory.GetCurrentDirectory();
            if (!strDirectory.EndsWith("\\"))
                strDirectory += "\\";

            using (ZipInputStream zipInputStream = new ZipInputStream(stream))
            {
                zipInputStream.Password = password;
                ZipEntry theEntry;

                while ((theEntry = zipInputStream.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    Directory.CreateDirectory(strDirectory + directoryName);

                    if (fileName != "")
                    {
                        if ((File.Exists(strDirectory + directoryName + fileName) && overWrite) || (!File.Exists(strDirectory + directoryName + fileName)))
                        {
                            using (FileStream streamWriter = File.Create(strDirectory + directoryName + fileName))
                            {
                                byte[] data = new byte[blockSize];
                                while (true)
                                {
                                    blockSize = zipInputStream.Read(data, 0, data.Length);

                                    if (blockSize > 0)
                                        streamWriter.Write(data, 0, blockSize);
                                    else
                                        break;
                                }
                                streamWriter.Close();
                            }
                        }
                    }
                }
                stream.Close();
                zipInputStream.Close();
            }
        }

        public static MemoryStream UnZipMemoryStream(Stream stream, string password, int blockSize = 4096)
        {
            MemoryStream streamWriter = new MemoryStream();
            using (ZipInputStream zipInputStream = new ZipInputStream(stream))
            {
                zipInputStream.Password = password;
                ZipEntry theEntry;

                while ((theEntry = zipInputStream.GetNextEntry()) != null)
                {
                    string directoryName = "";
                    string pathToZip = "";
                    pathToZip = theEntry.Name;

                    if (pathToZip != "")
                        directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                    string fileName = Path.GetFileName(pathToZip);

                    if (fileName != "")
                    {
                        byte[] data = new byte[blockSize];
                        while (true)
                        {
                            blockSize = zipInputStream.Read(data, 0, data.Length);

                            if (blockSize > 0)
                                streamWriter.Write(data, 0, blockSize);
                            else
                                break;
                        }
                    }
                }
                stream.Close();
                zipInputStream.Close();
            }
            return streamWriter;
        }
    }
}