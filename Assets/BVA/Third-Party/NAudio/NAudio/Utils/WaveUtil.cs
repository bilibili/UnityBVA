using NLayer.NAudioSupport;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NAudio.Wave
{
    public static class WaveUtil
    {
        public struct WaveHeaderData
        {
            public string RiffHeader; // "riff"
            public Int32 FileSize; // ファイルサイズ-8
            public string WaveHeader; // "WAVE"
            public string FormatChunk; // "fmt "
            public Int32 FormatChunkSize; // fmtチャンクのバイト数
            public Int16 FormatID; // フォーマット
            public Int16 Channel; // チャンネル数
            public Int32 SampleRate; // サンプリングレート
            public Int32 BytePerSec; // データ速度
            public Int16 BlockSize; // ブロックサイズ
            public Int16 BitPerSample; // 量子化ビット数
            public string DataChunk; // "data"
            public Int32 DataChunkSize; // 波形データのバイト数
            public Int32 PlayTimeMsec;

            public override string ToString()
            {
                return $"{SampleRate}_{Channel}_{BytePerSec}_{BitPerSample}";
            }
        }

        public static bool TryReadHeader(MemoryStream ms, out WaveHeaderData waveHeader)
        {
            waveHeader = new WaveHeaderData();
            try
            {
                var br = new BinaryReader(ms);
                waveHeader.RiffHeader = System.Text.Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));
                waveHeader.FileSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
                waveHeader.WaveHeader = System.Text.Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));

                var bytesPerSec = 0;
                var readFmtChunk = false;
                var readDataChunk = false;
                while (!readFmtChunk || !readDataChunk)
                {
                    var chunk = System.Text.Encoding.GetEncoding(20127).GetString(br.ReadBytes(4));
                    if (chunk.ToLower().CompareTo("fmt ") == 0)
                    {
                        waveHeader.FormatChunk = chunk;
                        waveHeader.FormatChunkSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        waveHeader.FormatID = BitConverter.ToInt16(br.ReadBytes(2), 0);
                        waveHeader.Channel = BitConverter.ToInt16(br.ReadBytes(2), 0);
                        waveHeader.SampleRate = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        waveHeader.BytePerSec = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        waveHeader.BlockSize = BitConverter.ToInt16(br.ReadBytes(2), 0);
                        waveHeader.BitPerSample = BitConverter.ToInt16(br.ReadBytes(2), 0);
                        bytesPerSec = waveHeader.SampleRate * waveHeader.BlockSize;
                        if (waveHeader.FormatChunkSize > 16) br.ReadBytes(waveHeader.FormatChunkSize - 16);
                        readFmtChunk = true;
                    }
                    else if (chunk.ToLower().CompareTo("data") == 0)
                    {
                        waveHeader.DataChunk = chunk;
                        waveHeader.DataChunkSize = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        waveHeader.PlayTimeMsec =
                            (int)((double)waveHeader.DataChunkSize / (double)bytesPerSec * 1000);
                        readDataChunk = true;
                    }
                    else
                    {
                        var size = BitConverter.ToInt32(br.ReadBytes(4), 0);
                        if (0 < size) br.ReadBytes(size);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return false;
            }
            return true;
        }
        private static void WriteHeader(AudioClip clip, MemoryStream stream)
        {
            var hz = clip.frequency;
            var channels = clip.channels;
            var samples = clip.samples;

            stream.Seek(0, SeekOrigin.Begin);

            var riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            stream.Write(riff, 0, 4);

            var chunkSize = BitConverter.GetBytes(stream.Length - 8);
            stream.Write(chunkSize, 0, 4);

            var wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            stream.Write(wave, 0, 4);

            var fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            stream.Write(fmt, 0, 4);

            var subChunk1 = BitConverter.GetBytes(16);
            stream.Write(subChunk1, 0, 4);

            UInt16 one = 1;

            var audioFormat = BitConverter.GetBytes(one);
            stream.Write(audioFormat, 0, 2);

            var numChannels = BitConverter.GetBytes(channels);
            stream.Write(numChannels, 0, 2);

            var sampleRate = BitConverter.GetBytes(hz);
            stream.Write(sampleRate, 0, 4);

            var byteRate =
                BitConverter.GetBytes(hz * channels *
                                      2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
            stream.Write(byteRate, 0, 4);

            var blockAlign = (ushort)(channels * 2);
            stream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            UInt16 bps = 16;
            var bitsPerSample = BitConverter.GetBytes(bps);
            stream.Write(bitsPerSample, 0, 2);

            var datastring = System.Text.Encoding.UTF8.GetBytes("data");
            stream.Write(datastring, 0, 4);

            var subChunk2 = BitConverter.GetBytes(samples * channels * 2);
            stream.Write(subChunk2, 0, 4);
        }

        public static byte[] GetWaveBinary(AudioClip clip)
        {
            using (var stream = new MemoryStream())
            {
                stream.Seek(44, SeekOrigin.Begin);

                var samples = new float[clip.samples * clip.channels];
                clip.GetData(samples, 0);

                var intData = new Int16[samples.Length];

                var bytesData = new byte[samples.Length * 2];

                var rescaleFactor = 32767;
                for (var i = 0; i < samples.Length; i++)
                {
                    intData[i] = (short)(samples[i] * rescaleFactor);
                    var byteArr = new byte[2];
                    byteArr = BitConverter.GetBytes(intData[i]);
                    byteArr.CopyTo(bytesData, i * 2);
                }


                Buffer.BlockCopy(intData, 0, bytesData, 0, bytesData.Length);
                stream.Write(bytesData, 0, bytesData.Length);

                WriteHeader(clip, stream);

                return stream.GetBuffer();
            }
        }
        public static Task<(WaveHeaderData, float[])> ToWaveData(byte[] mp3Data)
        {
            return Task.Run(() =>
            {
                using (var ms = new MemoryStream(mp3Data))
                using (var reader = new Mp3FileReader(ms, wf => new Mp3FrameDecompressor(wf)))
                {
                    var outStream = new MemoryStream();
                    WaveFileWriter.WriteWavFileToStream(outStream, new WaveFloatTo16Provider(reader));
                    outStream.Position = 0;
                    if (!TryReadHeader(outStream, out var waveHeader))
                    {
                        Debug.LogWarning("Cannot read wave header.");
                    }
                    var wavData = CreateRangedRawData(outStream.GetBuffer(),
                    waveHeader.FormatChunkSize + 28,
                    waveHeader.DataChunkSize / waveHeader.BlockSize,
                    waveHeader.Channel, waveHeader.BitPerSample);
                    return (waveHeader, wavData);
                }
            });
        }
        public static async Task<AudioClip> ToAudioClip(byte[] mp3Data, string name)
        {
            var (waveHeader, wavData) = await ToWaveData(mp3Data);
            var clip = AudioClip.Create(name, waveHeader.DataChunkSize / waveHeader.BlockSize, waveHeader.Channel, waveHeader.SampleRate, false);
            clip.SetData(wavData, 0);
            return clip;
        }
        private static readonly float RANGE_VALUE_BIT_8 = 1.0f / Mathf.Pow(2, 7); // 1 / 128
        private static readonly float RANGE_VALUE_BIT_16 = 1.0f / Mathf.Pow(2, 15); // 1 / 32768
        private static readonly int BIT_8 = 8;
        private static readonly int BIT_16 = 16;
        private static readonly int BIT_24 = 24;

        private static float[] CreateRangedRawData(byte[] rawData, int wavBufIdx, int samples, int channels,
           int bitPerSample)
        {
            var rangedRawData = new float[samples * channels];

            var stepByte = bitPerSample / BIT_8;
            var nowIdx = wavBufIdx;

            for (var i = 0; i < samples * channels; ++i)
            {
                rangedRawData[i] = ConvertByteToFloatData(rawData, nowIdx, bitPerSample);
                nowIdx += stepByte;
            }

            return rangedRawData;
        }
        // Force save as 16-bit .wav
        const int BlockSize_16Bit = 2;
        private static float ConvertByteToFloatData(byte[] rawData, int idx, int bitPerSample)
        {
            var floatData = 0.0f;
            try
            {
                if (bitPerSample == BIT_8)
                {
                    floatData = ((int)rawData[idx] - 0x80) * RANGE_VALUE_BIT_8;
                }
                else if (bitPerSample == BIT_16)
                {
                    floatData = BitConverter.ToInt16(rawData, idx) * RANGE_VALUE_BIT_16;
                }
                else if (bitPerSample == BIT_24)
                {
                    // skip low bit
                    floatData = BitConverter.ToInt16(rawData, idx + 1) * RANGE_VALUE_BIT_16;
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex);
                throw;
            }
            return floatData;
        }
        private static int WriteBytesToMemoryStream(ref MemoryStream stream, byte[] bytes, string tag = "")
        {
            int count = bytes.Length;
            stream.Write(bytes, 0, count);
            //Debug.LogFormat ("WAV:{0} wrote {1} bytes.", tag, count);
            return count;
        }

        private static int WriteFileHeader(ref MemoryStream stream, int fileSize)
        {
            int count = 0;
            int total = 12;

            // riff chunk id
            byte[] riff = Encoding.ASCII.GetBytes("RIFF");
            count += WriteBytesToMemoryStream(ref stream, riff, "ID");

            // riff chunk size
            int chunkSize = fileSize - 8; // total size - 8 for the other two fields in the header
            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(chunkSize), "CHUNK_SIZE");

            byte[] wave = Encoding.ASCII.GetBytes("WAVE");
            count += WriteBytesToMemoryStream(ref stream, wave, "FORMAT");

            // Validate header
            Debug.AssertFormat(count == total, "Unexpected wav descriptor byte count: {0} == {1}", count, total);

            return count;
        }
        private static int WriteFileFormat(ref MemoryStream stream, int channels, int sampleRate, UInt16 bitDepth)
        {
            int BytesPerSample(UInt16 bitDepth)
            {
                return bitDepth / 8;
            }
            int count = 0;
            int total = 24;

            byte[] id = Encoding.ASCII.GetBytes("fmt ");
            count += WriteBytesToMemoryStream(ref stream, id, "FMT_ID");

            int subchunk1Size = 16; // 24 - 8
            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk1Size), "SUBCHUNK_SIZE");

            UInt16 audioFormat = 1;
            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(audioFormat), "AUDIO_FORMAT");

            UInt16 numChannels = Convert.ToUInt16(channels);
            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(numChannels), "CHANNELS");

            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(sampleRate), "SAMPLE_RATE");

            int byteRate = sampleRate * channels * BytesPerSample(bitDepth);
            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(byteRate), "BYTE_RATE");

            UInt16 blockAlign = Convert.ToUInt16(channels * BytesPerSample(bitDepth));
            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(blockAlign), "BLOCK_ALIGN");

            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(bitDepth), "BITS_PER_SAMPLE");

            // Validate format
            Debug.AssertFormat(count == total, "Unexpected wav fmt byte count: {0} == {1}", count, total);

            return count;
        }
        private static byte[] ConvertAudioClipDataToInt16ByteArray(float[] data)
        {
            MemoryStream dataStream = new MemoryStream();

            int x = sizeof(Int16);

            Int16 maxValue = Int16.MaxValue;

            int i = 0;
            while (i < data.Length)
            {
                dataStream.Write(BitConverter.GetBytes(Convert.ToInt16(data[i] * maxValue)), 0, x);
                ++i;
            }
            byte[] bytes = dataStream.ToArray();

            // Validate converted bytes
            Debug.AssertFormat(data.Length * x == bytes.Length, "Unexpected float[] to Int16 to byte[] size: {0} == {1}", data.Length * x, bytes.Length);

            dataStream.Dispose();

            return bytes;
        }

        private static int WriteFileData(ref MemoryStream stream, AudioClip audioClip, UInt16 bitDepth)
        {
            int count = 0;
            int total = 8;

            // Copy float[] data from AudioClip
            float[] data = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(data, 0);

            byte[] bytes = ConvertAudioClipDataToInt16ByteArray(data);

            byte[] id = Encoding.ASCII.GetBytes("data");
            count += WriteBytesToMemoryStream(ref stream, id, "DATA_ID");

            int subchunk2Size = Convert.ToInt32(audioClip.samples * audioClip.channels * BlockSize_16Bit); // BlockSize (bitDepth)
            count += WriteBytesToMemoryStream(ref stream, BitConverter.GetBytes(subchunk2Size), "SAMPLES");

            // Validate header
            Debug.AssertFormat(count == total, "Unexpected wav data id byte count: {0} == {1}", count, total);

            // Write bytes to stream
            count += WriteBytesToMemoryStream(ref stream, bytes, "DATA");

            // Validate audio data
            Debug.AssertFormat(bytes.Length == subchunk2Size, "Unexpected AudioClip to wav subchunk2 size: {0} == {1}", bytes.Length, subchunk2Size);

            return count;
        }

        private static int BlockSize(UInt16 bitDepth)
        {
            switch (bitDepth)
            {
                case 32:
                    return sizeof(Int32); // 32-bit -> 4 bytes (Int32)
                case 16:
                    return sizeof(Int16); // 16-bit -> 2 bytes (Int16)
                case 8:
                    return sizeof(sbyte); // 8-bit -> 1 byte (sbyte)
                default:
                    throw new Exception(bitDepth + " bit depth is not supported.");
            }
        }

        public static byte[] FromAudioClip(AudioClip audioClip, string filepath = null, bool saveAsFile = false)
        {
            MemoryStream stream = new MemoryStream();

            const int headerSize = 44;

            // get bit depth
            UInt16 bitDepth = 16; //BitDepth (audioClip);

            // total file size = 44 bytes for header format and audioClip.samples * factor due to float to Int16 / sbyte conversion
            int fileSize = audioClip.samples * audioClip.channels * BlockSize_16Bit + headerSize; // BlockSize (bitDepth)

            // chunk descriptor (riff)
            WriteFileHeader(ref stream, fileSize);
            // file header (fmt)
            WriteFileFormat(ref stream, audioClip.channels, audioClip.frequency, bitDepth);
            // data chunks (data)
            WriteFileData(ref stream, audioClip, bitDepth);

            byte[] bytes = stream.ToArray();

            // Validate total bytes
            Debug.AssertFormat(bytes.Length == fileSize, "Unexpected AudioClip to wav format byte count: {0} == {1}", bytes.Length, fileSize);

            // Save file to persistant storage location
            if (saveAsFile)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                File.WriteAllBytes(filepath, bytes);
                //Debug.Log ("Auto-saved .wav file: " + filepath);
            }

            stream.Dispose();

            return bytes;
        }

        public static void SaveToMP3File(AudioClip audioClip, string path)
        {
            byte[] pcmData = FromAudioClip(audioClip);

            using var reader = new WaveFileReader(new MemoryStream(pcmData));
            try
            {
                MediaFoundationEncoder.EncodeToMp3(reader, path, 48000);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }
    }
}