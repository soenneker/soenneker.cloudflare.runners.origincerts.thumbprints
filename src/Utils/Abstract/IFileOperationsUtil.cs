using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Cloudflare.Runners.OriginCerts.Thumbprints.Utils.Abstract;

public interface IFileOperationsUtil
{
    ValueTask Process(string filePath, CancellationToken cancellationToken);
}
