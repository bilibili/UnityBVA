//	Copyright (c) 2012 Calvin Rien
//        http://the.darktable.com
//
//	This software is provided 'as-is', without any express or implied warranty. In
//	no event will the authors be held liable for any damages arising from the use
//	of this software.
//
//	Permission is granted to anyone to use this software for any purpose,
//	including commercial applications, and to alter it and redistribute it freely,
//	subject to the following restrictions:
//
//	1. The origin of this software must not be misrepresented; you must not claim
//	that you wrote the original software. If you use this software in a product,
//	an acknowledgment in the product documentation would be appreciated but is not
//	required.
//
//	2. Altered source versions must be plainly marked as such, and must not be
//	misrepresented as being the original software.
//
//	3. This notice may not be removed or altered from any source distribution.
//
//  =============================================================================
//
//  derived from Gregorio Zanon's script
//  http://forum.unity3d.com/threads/119295-Writing-AudioListener.GetOutputData-to-wav-problem?p=806734&viewfull=1#post806734

// 
// Fork by R-WebsterNoble
// Optimized! Now 20 times quicker
// Easy to get byte[] instead of saving file

// Note: GetWav() with trimming returns the full buffer with a load of zeros on the end. 
// Use the length out parameter to know where the data stops.
//

using System;
using System.IO;
using System.Text;
using UnityEngine;

public static class WaveAudio
{
    private const int HEADER_SIZE = 44;
    private const float RESCALE_FACTOR = 32767; //to convert float to Int16

    public static void Save(string filepath, AudioClip clip, bool trimBegin = false, bool trimEnd = false)
    {
        if (!filepath.ToLower().EndsWith(".wav"))
        {
            filepath += ".wav";
        }
        Directory.CreateDirectory(Path.GetDirectoryName(filepath));

        using (FileStream fileStream = new FileStream(filepath, FileMode.Create))
        using (BinaryWriter writer = new BinaryWriter(fileStream))
        {
            byte[] wav = GetWav(clip, out int length, trimBegin, trimEnd);
            writer.Write(wav, 0, length);
        }
    }

    public static byte[] GetWav(AudioClip clip, out int length, bool trimBegin = false, bool trimEnd = false)
    {
        byte[] data = ConvertAndWrite(clip, out length, out uint samples, trimBegin, trimEnd);

        WriteHeader(data, clip, length, samples);

        return data;
    }

    private static byte[] ConvertAndWrite(AudioClip clip, out int length, out uint samplesAfterTrimming, bool trimBegin, bool trimEnd)
    {
        float[] samples = new float[clip.samples * clip.channels];

        clip.GetData(samples, 0);

        int sampleCount = samples.Length;

        int start = 0;
        int end = sampleCount - 1;

        if (trimBegin)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                if ((short)(samples[i] * RESCALE_FACTOR) == 0)
                    continue;

                start = i;
                break;
            }
        }
        if (trimEnd)
        {
            for (int i = sampleCount - 1; i >= 0; i--)
            {
                if ((short)(samples[i] * RESCALE_FACTOR) == 0)
                    continue;

                end = i;
                break;
            }
        }

        byte[] buffer = new byte[(sampleCount * 2) + HEADER_SIZE];

        int p = HEADER_SIZE;
        for (int i = start; i <= end; i++)
        {
            short value = (short)(samples[i] * RESCALE_FACTOR);
            buffer[p++] = (byte)(value >> 0);
            buffer[p++] = (byte)(value >> 8);
        }

        length = p;
        samplesAfterTrimming = (uint)(end - start + 1);
        return buffer;
    }

    private static void AddDataToBuffer(byte[] buffer, ref uint offset, byte[] addBytes)
    {
        for (int i = 0; i < addBytes.Length; ++i)
        {
            buffer[offset++] = addBytes[i];
        }
    }

    private static void WriteHeader(byte[] stream, AudioClip clip, int length, uint samples)
    {
        uint hz = (uint)clip.frequency;
        ushort channels = (ushort)clip.channels;

        uint offset = 0u;

        byte[] riff = Encoding.UTF8.GetBytes("RIFF");
        AddDataToBuffer(stream, ref offset, riff);

        byte[] chunkSize = BitConverter.GetBytes(length - 8);
        AddDataToBuffer(stream, ref offset, chunkSize);

        byte[] wave = Encoding.UTF8.GetBytes("WAVE");
        AddDataToBuffer(stream, ref offset, wave);

        byte[] fmt = Encoding.UTF8.GetBytes("fmt ");
        AddDataToBuffer(stream, ref offset, fmt);

        byte[] subChunk1 = BitConverter.GetBytes(16u);
        AddDataToBuffer(stream, ref offset, subChunk1);

        const ushort one = 1;

        byte[] audioFormat = BitConverter.GetBytes(one);
        AddDataToBuffer(stream, ref offset, audioFormat);

        byte[] numChannels = BitConverter.GetBytes(channels);
        AddDataToBuffer(stream, ref offset, numChannels);

        byte[] sampleRate = BitConverter.GetBytes(hz);
        AddDataToBuffer(stream, ref offset, sampleRate);

        byte[] byteRate = BitConverter.GetBytes(hz * channels * 2); // sampleRate * bytesPerSample*number of channels, here 44100*2*2
        AddDataToBuffer(stream, ref offset, byteRate);

        ushort blockAlign = (ushort)(channels * 2);
        AddDataToBuffer(stream, ref offset, BitConverter.GetBytes(blockAlign));

        ushort bps = 16;
        byte[] bitsPerSample = BitConverter.GetBytes(bps);
        AddDataToBuffer(stream, ref offset, bitsPerSample);

        byte[] dataString = Encoding.UTF8.GetBytes("data");
        AddDataToBuffer(stream, ref offset, dataString);

        byte[] subChunk2 = BitConverter.GetBytes(samples * 2);
        AddDataToBuffer(stream, ref offset, subChunk2);
    }

    public static AudioClip ToAudioClip(byte[] fileBytes, string name = "wav")
    {
        //string riff = Encoding.ASCII.GetString (fileBytes, 0, 4);
        //string wave = Encoding.ASCII.GetString (fileBytes, 8, 4);
        int subchunk1 = BitConverter.ToInt32(fileBytes, 16);
        ushort audioFormat = BitConverter.ToUInt16(fileBytes, 20);

        // NB: Only uncompressed PCM wav files are supported.
        string formatCode = FormatCode(audioFormat);
        Debug.AssertFormat(audioFormat == 1 || audioFormat == 65534, "Detected format code '{0}' {1}, but only PCM and WaveFormatExtensable uncompressed formats are currently supported.", audioFormat, formatCode);

        ushort channels = BitConverter.ToUInt16(fileBytes, 22);
        int sampleRate = BitConverter.ToInt32(fileBytes, 24);
        //int byteRate = BitConverter.ToInt32 (fileBytes, 28);
        //UInt16 blockAlign = BitConverter.ToUInt16 (fileBytes, 32);
        ushort bitDepth = BitConverter.ToUInt16(fileBytes, 34);

        int headerOffset = 16 + 4 + subchunk1 + 4;
        int subchunk2 = BitConverter.ToInt32(fileBytes, headerOffset);
        //Debug.LogFormat ("riff={0} wave={1} subchunk1={2} format={3} channels={4} sampleRate={5} byteRate={6} blockAlign={7} bitDepth={8} headerOffset={9} subchunk2={10} filesize={11}", riff, wave, subchunk1, formatCode, channels, sampleRate, byteRate, blockAlign, bitDepth, headerOffset, subchunk2, fileBytes.Length);

        float[] data;
        switch (bitDepth)
        {
            case 8:
                data = Convert8BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
                break;
            case 16:
                data = Convert16BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
                break;
            case 24:
                data = Convert24BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
                break;
            case 32:
                data = Convert32BitByteArrayToAudioClipData(fileBytes, headerOffset, subchunk2);
                break;
            default:
                throw new Exception(bitDepth + " bit depth is not supported.");
        }

        AudioClip audioClip = AudioClip.Create(name, data.Length, (int)channels, sampleRate, false);
        audioClip.SetData(data, 0);
        return audioClip;
    }

    private static float[] Convert8BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
    {
        int wavSize = BitConverter.ToInt32(source, headerOffset);
        headerOffset += sizeof(int);
        Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "Failed to get valid 8-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

        float[] data = new float[wavSize];

        sbyte maxValue = sbyte.MaxValue;

        int i = 0;
        while (i < wavSize)
        {
            data[i] = (float)source[i] / maxValue;
            ++i;
        }

        return data;
    }

    private static float[] Convert16BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
    {
        int wavSize = BitConverter.ToInt32(source, headerOffset);
        headerOffset += sizeof(int);
        Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "Failed to get valid 16-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

        int x = sizeof(short); // block size = 2
        int convertedSize = wavSize / x;

        float[] data = new float[convertedSize];

        short maxValue = short.MaxValue;

        int offset = 0;
        int i = 0;
        while (i < convertedSize)
        {
            offset = i * x + headerOffset;
            data[i] = (float)BitConverter.ToInt16(source, offset) / maxValue;
            ++i;
        }

        Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

        return data;
    }

    private static float[] Convert24BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
    {
        int wavSize = BitConverter.ToInt32(source, headerOffset);
        headerOffset += sizeof(int);
        Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "Failed to get valid 24-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

        int x = 3; // block size = 3
        int convertedSize = wavSize / x;

        int maxValue = int.MaxValue;

        float[] data = new float[convertedSize];

        byte[] block = new byte[sizeof(int)]; // using a 4 byte block for copying 3 bytes, then copy bytes with 1 offset

        int offset = 0;
        int i = 0;
        while (i < convertedSize)
        {
            offset = i * x + headerOffset;
            Buffer.BlockCopy(source, offset, block, 1, x);
            data[i] = (float)BitConverter.ToInt32(block, 0) / maxValue;
            ++i;
        }

        Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

        return data;
    }

    private static float[] Convert32BitByteArrayToAudioClipData(byte[] source, int headerOffset, int dataSize)
    {
        int wavSize = BitConverter.ToInt32(source, headerOffset);
        headerOffset += sizeof(int);
        Debug.AssertFormat(wavSize > 0 && wavSize == dataSize, "Failed to get valid 32-bit wav size: {0} from data bytes: {1} at offset: {2}", wavSize, dataSize, headerOffset);

        int x = sizeof(float); //  block size = 4
        int convertedSize = wavSize / x;

        int maxValue = int.MaxValue;

        float[] data = new float[convertedSize];

        int offset = 0;
        int i = 0;
        while (i < convertedSize)
        {
            offset = i * x + headerOffset;
            data[i] = (float)BitConverter.ToInt32(source, offset) / maxValue;
            ++i;
        }
        Debug.AssertFormat(data.Length == convertedSize, "AudioClip .wav data is wrong size: {0} == {1}", data.Length, convertedSize);

        return data;
    }
    private static string FormatCode(ushort code)
    {
        switch (code)
        {
            case 1:
                return "PCM";
            case 2:
                return "ADPCM";
            case 3:
                return "IEEE";
            case 7:
                return "μ-law";
            case 65534:
                return "WaveFormatExtensable";
            default:
                Debug.LogWarning("Unknown wav code format:" + code);
                return "";
        }
    }
}
