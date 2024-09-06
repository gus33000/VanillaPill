using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VelocityPillApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtendedSplashScreen : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private readonly SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;
        private readonly double ScaleFactor;

        private void SetupTitleBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar.GetForCurrentView().BackgroundOpacity = 0;
            }

            _ = ApplicationView.GetForCurrentView().SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
            ApplicationViewTitleBar titlebar = ApplicationView.GetForCurrentView().TitleBar;
            SolidColorBrush transparentColorBrush = new SolidColorBrush { Opacity = 0 };
            Windows.UI.Color transparentColor = transparentColorBrush.Color;
            titlebar.BackgroundColor = transparentColor;
            titlebar.ButtonBackgroundColor = transparentColor;
            titlebar.ButtonInactiveBackgroundColor = transparentColor;

            if (Application.Current.Resources["ApplicationForegroundThemeBrush"] is SolidColorBrush solidColorBrush)
            {
                titlebar.ButtonForegroundColor = solidColorBrush.Color;
                titlebar.ButtonInactiveForegroundColor = solidColorBrush.Color;
            }


            if (Application.Current.Resources["ApplicationForegroundThemeBrush"] is SolidColorBrush colorBrush)
            {
                titlebar.ForegroundColor = colorBrush.Color;
            }

            Windows.UI.Color hovercolor = (Application.Current.Resources["ApplicationForegroundThemeBrush"] as SolidColorBrush).Color;
            hovercolor.A = 32;
            titlebar.ButtonHoverBackgroundColor = hovercolor;
            titlebar.ButtonHoverForegroundColor = (Application.Current.Resources["ApplicationForegroundThemeBrush"] as SolidColorBrush).Color;
            hovercolor.A = 64;
            titlebar.ButtonPressedBackgroundColor = hovercolor;
            titlebar.ButtonPressedForegroundColor = (Application.Current.Resources["ApplicationForegroundThemeBrush"] as SolidColorBrush).Color;
        }

        public ExtendedSplashScreen(SplashScreen splashscreen, bool loadState)
        {
            RequestedTheme = ElementTheme.Dark;
            ScaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            InitializeComponent();
            SetupTitleBar();
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
            splash = splashscreen;

            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, object>(DismissedEventHandler);
                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
                // Optional: Add a progress ring to your splash screen to show users that content is loading
                PositionRing();
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();
            // Restore the saved session state if necessary
            RestoreStateAsync(loadState);
        }

        private void RestoreStateAsync(bool loadState)
        {
            if (loadState)
            {
                // TODO: write code to load state
            }
        }

        // Position the extended splash screen image in the same location as the system splash screen image.
        private void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.Left);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Top);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                extendedSplashImage.Height = splashImageRect.Height / ScaleFactor;
                extendedSplashImage.Width = splashImageRect.Width / ScaleFactor;
            }
            else
            {
                extendedSplashImage.Height = splashImageRect.Height;
                extendedSplashImage.Width = splashImageRect.Width;
            }
        }

        private void PositionRing()
        {
            splashProgressRing.SetValue(Canvas.LeftProperty, splashImageRect.X + (splashImageRect.Width * 0.5) - (splashProgressRing.Width * 0.5));
            splashProgressRing.SetValue(Canvas.TopProperty, splashImageRect.Y + splashImageRect.Height + (splashImageRect.Height * 0.1));

            //Desc.SetValue(Canvas.LeftProperty, splashImageRect.X + (splashImageRect.Width * 0.5) - (splashProgressRing.Width * 0.5));
            Desc.Width = LoadingPanel.ActualWidth;
            Desc.SetValue(Canvas.TopProperty, splashProgressRing.Height + 16 + splashImageRect.Y + splashImageRect.Height + (splashImageRect.Height * 0.1));
        }

        private void ExtendedSplash_OnResize(object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
                PositionRing();
            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        private async void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
            // Complete app setup operations here...
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (sender != null)
                {
                    // Update the coordinates of the splash screen image.
                    splashImageRect = sender.ImageLocation;
                    PositionImage();
                    PositionRing();
                }

                _ = await new SelectSourceContentDialog().ShowAsync();

                StorageFile file = null;

                try
                {
                    Windows.Storage.Pickers.FileOpenPicker openDialog = new Windows.Storage.Pickers.FileOpenPicker()
                    {
                        SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary
                    };
                    openDialog.FileTypeFilter.Add(".csv");
                    openDialog.FileTypeFilter.Add(".txt");
                    file = await openDialog.PickSingleFileAsync();
                }
                catch { }

                Desc.Text = "Loading feature informations...";

                _ = ThreadPool.RunAsync(async (_) =>
                {
                    if (file != null)
                    {
                        //var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///velbase.csv"));
                        App.Lines = await FileIO.ReadLinesAsync(file);

                        List<string> newlines = new List<string>();
                        int count = 0;
                        foreach (string line in App.Lines)
                        {
                            count++;
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                Desc.Text = "Loading feature informations... " + (int)Math.Round((double)(100 * count) / App.Lines.Count) + "%";
                            });

                            string idstr = line.Split(',').Last();
                            if (!idstr.StartsWith("0x"))
                            {
                                idstr = "0x" + int.Parse(idstr).ToString("X4");
                            }
                            for (int i = 0; i <= 10 - idstr.Length; i++)
                            {
                                idstr = "0x0" + idstr.Substring(2);
                            }

                            string nline = line.Split(',').First() + "," + idstr;

                            if (newlines.Any(x => x.ToLower().EndsWith(nline.Split(',').Last().ToLower())))
                            {
                                string newstr = nline.Split(',').First() + " / " + newlines.First(x => x.ToLower().EndsWith(nline.Split(',').Last().ToLower()));
                                newlines.Remove(newlines.First(x => x.ToLower().EndsWith(nline.Split(',').Last().ToLower())));
                                newlines.Add(newstr);
                            }
                            else
                            {
                                newlines.Add(nline);
                            }
                        }

                        App.Lines = newlines;

                        count = 0;
                        foreach (string line in App.Lines)
                        {
                            count++;
                            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                            {
                                Desc.Text = "Loading feature templates... " + (int)Math.Round((double)(100 * count) / App.Lines.Count) + "%";
                            });

                            VelocityHelper.WNFState state = new VelocityHelper.WNFState() { ID = uint.Parse(line.Split(',').Last().Substring(2), System.Globalization.NumberStyles.HexNumber), WNFStateData = new byte[] { 01, 08, 00, 00, 00, 00, 00, 00 } };
                            App.templatelist.Add(state);
                        }
                    }
                    else
                    {
                        App.Lines = new List<string>();
                    }

                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Desc.Text = "Loading current features... ";
                    });

                    App.statelist = App.helper.GetFeatureStates();

                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        Desc.Text = "Sorting features... ";
                    });

                    App.itemSource = AlphaKeyGroup<VelocityHelper.WNFState>.CreateGroups(App.statelist, CultureInfo.InvariantCulture,
                                         s => s.DisplayableID, true);

                    App.itemSource2 = AlphaKeyGroup<VelocityHelper.WNFState>.CreateGroups(App.templatelist, CultureInfo.InvariantCulture,
                                         s => s.DisplayableID, true);

                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        rootFrame.Navigate(typeof(MainPage));
                        Window.Current.Content = rootFrame;
                        Window.Current.Activate();
                    });
                });
            });
        }
    }
}
