using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace WebView2DownloadFile
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            _ = wv2.EnsureCoreWebView2Async().AsTask().ContinueWith(async (task) =>
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    wv2.CoreWebView2.DownloadStarting += OnDownloadStarting;
                    wv2.CoreWebView2.Navigate("http://demo.borland.com/testsite/download_testpage.php");
                });
            });
        }

        private void OnDownloadStarting(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2DownloadStartingEventArgs args)
        {
            Trace.WriteLine("DownloadStarting");
            var downloadOp = args.DownloadOperation;
            args.DownloadOperation.StateChanged += (sender2, args2) =>
            {
                var state = downloadOp.State;
                switch (state)
                {
                    case Microsoft.Web.WebView2.Core.CoreWebView2DownloadState.InProgress:
                        Trace.WriteLine("Download StateChanged: InProgress");
                        break;
                    case Microsoft.Web.WebView2.Core.CoreWebView2DownloadState.Completed:
                        Trace.WriteLine("Download StateChanged: Completed");
                        break;
                    case Microsoft.Web.WebView2.Core.CoreWebView2DownloadState.Interrupted:
                        Trace.WriteLine("Download StateChanged: Interrupted, reason: " + downloadOp.InterruptReason);
                        break;
                }
            };
        }
    }
}
