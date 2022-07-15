using System.Runtime.InteropServices;

namespace OggVorbis
{
    public static class VorbisPlugin
    {
        public static void Save(
            string filePath,
            UnityEngine.AudioClip audioClip,
            float quality = 0.4f,
            int samplesToRead = 1024)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new System.ArgumentException("The file path is null or white space");
            }
            if (audioClip == null)
            {
                throw new System.ArgumentNullException(nameof(audioClip));
            }
            if (samplesToRead <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(samplesToRead));
            }
            short finalChannelsCount = (short)audioClip.channels;
            if (finalChannelsCount != 1 && finalChannelsCount != 2)
            {
                throw new System.ArgumentException($"Only one or two channels are supported, provided channels count: {finalChannelsCount}");
            }
            if (!filePath.EndsWith(".ogg"))
            {
                filePath += ".ogg";
            }

            float[] pcm = new float[audioClip.samples * audioClip.channels];
            audioClip.GetData(pcm, 0);
            int returnCode = NativeBridge.WriteAllPcmDataToFile(filePath, pcm, pcm.Length, finalChannelsCount, audioClip.frequency, quality, samplesToRead);
            NativeErrorException.ThrowExceptionIfNecessary(returnCode);
        }
        public static byte[] GetOggVorbis(
            UnityEngine.AudioClip audioClip,
            float quality = 0.4f,
            int samplesToRead = 1024)
        {
            if (audioClip == null)
            {
                throw new System.ArgumentNullException(nameof(audioClip));
            }
            if (samplesToRead <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(samplesToRead));
            }
            short finalChannelsCount = (short)audioClip.channels;
            if (finalChannelsCount != 1 && finalChannelsCount != 2)
            {
                throw new System.ArgumentException($"Only one or two channels are supported, provided channels count: {finalChannelsCount}");
            }
            int returnCode;
            System.IntPtr bytesPtr = System.IntPtr.Zero;
            byte[] bytes;
            try
            {
                float[] pcm = new float[audioClip.samples * audioClip.channels];
                audioClip.GetData(pcm, 0);
                returnCode = NativeBridge.WriteAllPcmDataToMemory(
                    out bytesPtr,
                    out int bytesLength,
                    pcm,
                    pcm.Length,
                    finalChannelsCount,
                    audioClip.frequency,
                    quality,
                    samplesToRead);
                NativeErrorException.ThrowExceptionIfNecessary(returnCode);
                bytes = new byte[bytesLength];
                Marshal.Copy(bytesPtr, bytes, 0, bytesLength);
            }
            finally
            {
                returnCode = NativeBridge.FreeMemoryArrayForWriteAllPcmData(bytesPtr);
                NativeErrorException.ThrowExceptionIfNecessary(returnCode);
            }
            return bytes;
        }

        public static UnityEngine.AudioClip Load(string filePath, int maxSamplesToRead = 1024)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new System.ArgumentException("The file path is null or white space");
            }
            if (maxSamplesToRead <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(maxSamplesToRead));
            }
            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException();
            }
            int returnCode;
            System.IntPtr pcmPtr = System.IntPtr.Zero;
            UnityEngine.AudioClip audioClip;
            try
            {
                returnCode = NativeBridge.ReadAllPcmDataFromFile(
                    filePath,
                    out pcmPtr,
                    out int pcmLength,
                    out short channels,
                    out int frequency,
                    maxSamplesToRead);
                NativeErrorException.ThrowExceptionIfNecessary(returnCode);
                float[] pcm = new float[pcmLength];
                Marshal.Copy(pcmPtr, pcm, 0, pcmLength);
                audioClip = UnityEngine.AudioClip.Create(System.IO.Path.GetFileName(filePath), pcmLength / channels, channels, frequency, false);
                audioClip.SetData(pcm, 0);
            }
            finally
            {
                returnCode = NativeBridge.FreeSamplesArrayNativeMemory(ref pcmPtr);
                NativeErrorException.ThrowExceptionIfNecessary(returnCode);
            }
            return audioClip;
        }
        public static UnityEngine.AudioClip ToAudioClip(byte[] bytes, string audioClipName, int maxSamplesToRead = 1024)
        {
            if (bytes == null)
            {
                throw new System.ArgumentNullException(nameof(bytes));
            }
            if (bytes.Length < 10)
            {
                throw new System.ArgumentException(nameof(bytes));
            }
            if (string.IsNullOrWhiteSpace(audioClipName))
            {
                throw new System.ArgumentException("Please provide an audio clip name");
            }
            if (maxSamplesToRead <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(maxSamplesToRead));
            }
            int returnCode;
            System.IntPtr pcmPtr = System.IntPtr.Zero;
            UnityEngine.AudioClip audioClip = null;
            try
            {
                returnCode = NativeBridge.ReadAllPcmDataFromMemory(
                    bytes,
                    bytes.Length,
                    out pcmPtr,
                    out int pcmLength,
                    out short channels,
                    out int frequency,
                    maxSamplesToRead);
                NativeErrorException.ThrowExceptionIfNecessary(returnCode);
                float[] pcm = new float[pcmLength];
                Marshal.Copy(pcmPtr, pcm, 0, pcmLength);
                audioClip = UnityEngine.AudioClip.Create(audioClipName, pcmLength / channels, channels, frequency, false);
                audioClip.SetData(pcm, 0);
            }
            finally
            {
                returnCode = NativeBridge.FreeSamplesArrayNativeMemory(ref pcmPtr);
                NativeErrorException.ThrowExceptionIfNecessary(returnCode);
            }
            return audioClip;
        }
    }
}