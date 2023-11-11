using Sparks.Modules.Main.Models.Abstractions;
using System;
using System.Windows.Media;

namespace Sparks.Modules.Main.Models;

class MainModel : BindableBase, IMainModel
{
    private ImageSource _frame;
    public ImageSource Frame
    {
        get => _frame;
        set => SetProperty(ref _frame, value);
    }

    private string _sparksCount = "Количество искр: ";
    public string SparksCount
    {
        get => _sparksCount;
        set => SetProperty(ref _sparksCount, $"Количество искр: {value}");
    }

    private string _conclusion = "Выпуск не продут";
    public string Conclusion
    {
        get => _conclusion;
        set => SetProperty(ref _conclusion, value);
    }
}
