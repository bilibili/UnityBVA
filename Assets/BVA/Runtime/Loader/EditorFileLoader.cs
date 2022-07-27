using System.IO;
using System;
using System.Threading.Tasks;
using BVA.Extensions;

namespace BVA.Loader
{
	public class EditorFileLoader : IDataLoader, IDataLoader2
	{
		private readonly string _rootDirectoryPath;

		public EditorFileLoader(string rootDirectoryPath)
		{
			_rootDirectoryPath = rootDirectoryPath;
		}

		public Task<Stream> LoadStreamAsync(string relativeFilePath)
		{
			return Task.FromResult(LoadStream(relativeFilePath));
		}

		public Stream LoadStream(string relativeFilePath)
		{
			if (relativeFilePath == null)
			{
				throw new ArgumentNullException("relativeFilePath");
			}

			relativeFilePath = relativeFilePath.ConvertSpaceString();
			string pathToLoad = Path.Combine(_rootDirectoryPath, relativeFilePath);
			if (!File.Exists(pathToLoad))
			{
				throw new FileNotFoundException($"Buffer file {pathToLoad} not found", relativeFilePath);
			}

			return File.OpenRead(pathToLoad);
		}
	}
}
