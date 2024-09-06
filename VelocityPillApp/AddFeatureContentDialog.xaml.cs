using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VelocityPillApp
{
    public sealed partial class AddFeatureContentDialog : ContentDialog
    {
        public bool addtrue = false;

        public AddFeatureContentDialog()
        {
            InitializeComponent();
        }

        public async Task<string> DoIt()
        {
            _ = await ShowAsync();
            return addtrue ? FeatureIDBox.Text : "";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            addtrue = true;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
