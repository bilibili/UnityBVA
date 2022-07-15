using System;

namespace GLTF
{
	public class GLTFHeaderInvalidException : Exception
	{
		public GLTFHeaderInvalidException() : base() { }
		public GLTFHeaderInvalidException(string message) : base(message) { }
		public GLTFHeaderInvalidException(string message, Exception inner) : base(message, inner) { }
		protected GLTFHeaderInvalidException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
		{ }
	}
	
	public class GLTFParseException : Exception
	{
		public GLTFParseException() : base() { }
		public GLTFParseException(string message) : base(message) { }
		public GLTFParseException(string message, Exception inner) : base(message, inner) { }
		protected GLTFParseException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
		{ }
	}

	public class GLTFLoadException : Exception
	{
		public GLTFLoadException() : base() { }
		public GLTFLoadException(string message) : base(message) { }
		public GLTFLoadException(string message, Exception inner) : base(message, inner) { }
		protected GLTFLoadException(System.Runtime.Serialization.SerializationInfo info,
			System.Runtime.Serialization.StreamingContext context)
		{ }
	}
}
