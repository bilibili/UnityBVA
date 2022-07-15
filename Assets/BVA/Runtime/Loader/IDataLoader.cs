using System.IO;
using System.Threading.Tasks;

namespace BVA.Loader
{
	public interface IDataLoader
	{
		Task<Stream> LoadStreamAsync(string relativeFilePath);
	}
}
