using System;
using UnityEngine.Networking;

namespace BVA 
{
	[Serializable()]
	public class WebRequestException : Exception 
	{
		public WebRequestException() : base() { }
		public WebRequestException(string message) : base(message) { }
		public WebRequestException(string message, Exception inner) : base(message, inner) { }
		public WebRequestException(UnityWebRequest www) : base(string.Format("Error: {0} when requesting: {1}", www.responseCode, www.url)) { }
	}

	[Serializable()]
	public class ShaderNotFoundException : Exception
	{
		public ShaderNotFoundException() : base() { }
		public ShaderNotFoundException(string message) : base(message) { }
		public ShaderNotFoundException(string message, Exception inner) : base(message, inner) { }
	}
	[Serializable()]
	public class TextureNotExportableException : Exception
	{
		public TextureNotExportableException() : base() { }
		public TextureNotExportableException(string message) : base(message) { }
		public TextureNotExportableException(string message, Exception inner) : base(message, inner) { }
	}
	/// <summary>
	/// Load exceptions occur during runtime errors through use of the GLTFSceneImporter
	/// </summary>
	public class LoadException : Exception
	{
		public LoadException() : base() { }
		public LoadException(string message) : base(message) { }
	}
	/// <summary>
	/// Something only can be exported inside Unity Editor
	/// </summary>
	public class EditorExportOnlyException : Exception
	{
		public EditorExportOnlyException() : base() { }
		public EditorExportOnlyException(string message) : base(message) { }
	}
}
