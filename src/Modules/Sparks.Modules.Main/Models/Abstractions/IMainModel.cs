using System.Windows.Media;

namespace Sparks.Modules.Main.Models.Abstractions;

internal interface IMainModel
{
    ImageSource Frame { get; set; }
    int Floor { get; set; }
    string Conclusion { get; set; }
    string SparksCount { get; set; }
}
