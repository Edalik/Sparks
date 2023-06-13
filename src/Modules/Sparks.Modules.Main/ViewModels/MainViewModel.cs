using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Sparks.Modules.Main.Models;
using Sparks.Modules.Main.Models.Abstractions;
using Sparks.Modules.Main.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sparks.Modules.Main.ViewModels;

class MainViewModel
{
    private readonly IMainService _mainService;
    public IMainModel Model { get; }
    public ICommand ChooseFileCommand { get; }


    [Reactive] public CancellationTokenSource LastTokenSource { get; set; }
    private Task lastReadingTask;

    public MainViewModel()
    {
        _mainService = new MainService();
        Model = new MainModel();

        ChooseFileCommand = ReactiveCommand.Create
        (
            async () =>
            {
                LastTokenSource?.Cancel();
                LastTokenSource = new();

                if (lastReadingTask is not null)
                {
                    await lastReadingTask;
                }

                lastReadingTask = ReadFile(LastTokenSource.Token);
                await lastReadingTask;
            }
        );
    }

    private Task ReadFile(CancellationToken cancellationToken) => _mainService.ReadFile(Model, cancellationToken);
}