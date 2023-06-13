using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Sparks.Modules.Main.Models;
using Sparks.Modules.Main.Models.Abstractions;
using Sparks.Modules.Main.ViewModels;
using Sparks.Modules.Main.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparks.Modules.Main;

public class MainModule : IModule
{
    public void OnInitialized(IContainerProvider containerProvider)
    {
        var regionManager = containerProvider.Resolve<IRegionManager>();

        regionManager.RegisterViewWithRegion("MainRegion", typeof(MainView));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterSingleton<IMainModel, MainModel>();
        ViewModelLocationProvider.Register<MainView, MainViewModel>();
    }
}
