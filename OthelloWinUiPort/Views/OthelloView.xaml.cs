// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OthelloWinUiPort.Models;
using OthelloWinUiPort.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OthelloWinUiPort.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OthelloView : Window
    {

        public OthelloViewModel? ViewModel { get; }

        public OthelloView()
        {
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this); // m_window in App.cs
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            var size = new Windows.Graphics.SizeInt32();
            size.Width = 500;
            size.Height = 650;
            
            appWindow.Resize(size);

            this.InitializeComponent();

            ViewModel = Ioc.Default.GetService<OthelloViewModel>();
        }

        private void lstBoard_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var myItemsControl = (ItemsControl)sender;
            var point = e.GetCurrentPoint(myItemsControl).Position;

            foreach (var item in myItemsControl.Items)
            {
                var container = (FrameworkElement)myItemsControl.ContainerFromItem(item);
                var rect = container.TransformToVisual(myItemsControl).TransformBounds(new Rect(0, 0, container.ActualWidth, container.ActualHeight));
                if (rect.Contains(point))
                {
                    var myObject = item as Ficha;

                    ViewModel.Prueba(myObject);

                    break;
                }
            }


        }


    }
}
