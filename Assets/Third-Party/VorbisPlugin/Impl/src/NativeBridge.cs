using System.Runtime.InteropServices;

namespace OggVorbis
{
    internal static class NativeBridge
    {
#if UNITY_IOS && !UNITY_EDITOR
        private const string PLUGIN_NAME = "__Internal";
#else
        private const string PLUGIN_NAME = "VorbisPlugin";
#endif

        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "write_all_pcm_data_to_file")]
        internal static extern int WriteAllPcmDataToFile(
            string filePath,
            float[] samples,
            int samplesLength,
            short channels,
            int frequency,
            float base_quality,
            int samplesToRead);
        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "write_all_pcm_data_to_memory")]
        internal static extern int WriteAllPcmDataToMemory(
            out System.IntPtr bytesPtr,
            out int bytesLength,
            float[] samples,
            int samplesLength,
            short channels,
            int frequency,
            float base_quality,
            int samplesToRead);
        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "free_memory_array_for_write_all_pcm_data")]
        internal static extern int FreeMemoryArrayForWriteAllPcmData(
            System.IntPtr bytesNativeArray);

        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "read_all_pcm_data_from_file")]
        internal static extern int ReadAllPcmDataFromFile(
            string filePath,
            out System.IntPtr samples,
            out int samplesLength,
            out short channels,
            out int frequency,
            int maxSamplesToRead);
        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "read_all_pcm_data_from_memory")]
        internal static extern int ReadAllPcmDataFromMemory(
            byte[] memoryArray,
            int memoryArrayLength,
            out System.IntPtr samples,
            out int samplesLength,
            out short channels,
            out int frequency,
            int maxSamplesToRead);
        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "free_samples_array_native_memory")]
        internal static extern int FreeSamplesArrayNativeMemory(
            ref System.IntPtr samples);

        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "open_read_file_stream")]
        internal static extern System.IntPtr OpenReadFileStream(
            string filePath,
            out short channels,
            out int frequency);
        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "read_from_file_stream")]
        internal static extern int ReadFromFileStream(
            System.IntPtr state,
            float[] samplesToFill,
            int maxSamplesToRead);
        [DllImport(PLUGIN_NAME,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "close_file_stream")]
        internal static extern int CloseFileStream(
            System.IntPtr state);
    }
}
