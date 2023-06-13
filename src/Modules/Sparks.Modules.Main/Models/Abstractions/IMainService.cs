using System.Threading;
using System.Threading.Tasks;

namespace Sparks.Modules.Main.Models.Abstractions;

internal interface IMainService
{
    /// <summary>
    ///     Считать видео
    /// </summary>
    /// <param name="mainModel"></param>
    Task ReadFile(IMainModel mainModel, CancellationToken cancellationToken);
}
