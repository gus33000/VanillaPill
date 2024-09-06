using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace VelocityPillApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        public static string key = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Notifications\Data";
        public const string featureStoreName = "418A073AA3BC7C75";

        public static List<VelocityHelper.WNFState> statelist = new List<VelocityHelper.WNFState>();

        public static VelocityHelper helper = new VelocityHelper();

        public static IList<string> Lines
        {
            get; set;
        }

        public static List<VelocityHelper.WNFState> templatelist = new List<VelocityHelper.WNFState>();

        public static List<AlphaKeyGroup<VelocityHelper.WNFState>> itemSource = new List<AlphaKeyGroup<VelocityHelper.WNFState>>();
        public static List<AlphaKeyGroup<VelocityHelper.WNFState>> itemSource2 = new List<AlphaKeyGroup<VelocityHelper.WNFState>>();

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (e.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = e.PreviousExecutionState == ApplicationExecutionState.Terminated;
                ExtendedSplashScreen extendedSplash = new ExtendedSplashScreen(e.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }

            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
