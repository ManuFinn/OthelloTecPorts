// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using OthelloWinUiPort.Models;
using OthelloWinUiPort.ViewModels;
using OthelloWinUiPort.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace OthelloWinUiPort
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();

            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<Ficha>()
                .AddSingleton<OthelloViewModel>()
                .BuildServiceProvider());
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var viewModel = new OthelloViewModel();
            var window = new OthelloView();
            window.Activate();
        }
    }
}
