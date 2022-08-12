using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.IO;

namespace NLayer.NAudioSupport
{
    public class ManagedMpegStream : WaveStream, IDisposable
    {
        Stream _source;
        WaveFormat _waveFormat;
        MpegFile _fileDecoder;
        bool _closeOnDispose;

        public ManagedMpegStream(string fileName)
            : this(System.IO.File.OpenRead(fileName), true)
        {

        }

        public ManagedMpegStream(Stream source)
            : this(source, false)
        {
        }

        public ManagedMpegStream(Stream source, bool closeOnDispose)
        {
            this._source = source;
            this._closeOnDispose = closeOnDispose;
            this._fileDecoder = new MpegFile(this._source);
            this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._fileDecoder.SampleRate, this._fileDecoder.Channels);
        }

        public override WaveFormat WaveFormat
        {
            get { return _waveFormat; }
        }

        public void SetEQ(float[] eq)
        {
            _fileDecoder.SetEQ(eq);
        }

        public StereoMode StereoMode
        {
            get { return _fileDecoder.StereoMode; }
            set { _fileDecoder.StereoMode = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._fileDecoder.ReadSamples(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (_source != null)
            {
                if (_closeOnDispose)
                {
                    _source.Dispose();
                }
                _source = null;
            }
            base.Dispose(disposing);
        }

        public override long Length
        {
            get { return this._fileDecoder.Length; }
        }

        public override long Position
        {
            get
            {
                return this._fileDecoder.Position;
            }
            set
            {
                this._fileDecoder.Position = value;
            }
        }
    }
}
