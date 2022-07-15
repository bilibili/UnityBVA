namespace OggVorbis
{
    public class NativeErrorException : System.Exception
    {
        public NativeErrorCode NativeErrorCode { get; }

        private NativeErrorException(NativeErrorCode nativeErrorCode)
            : base($"Error code: {nativeErrorCode}")
        {
            NativeErrorCode = nativeErrorCode;
        }

        public override string ToString()
        {
            return $"{nameof(NativeErrorException)} {Message}";
        }

        internal static void ThrowExceptionIfNecessary(int returnValue)
        {
            if (returnValue == 0)
            {
                return;
            }
            throw new NativeErrorException((NativeErrorCode)returnValue);
        }
    }
}
