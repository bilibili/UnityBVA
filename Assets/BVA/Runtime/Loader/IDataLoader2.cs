using System.IO;

namespace BVA.Loader
{
	public interface IDataLoader2 : IDataLoader
	{
		Stream LoadStream(string relativeFilePath);
	}
}
