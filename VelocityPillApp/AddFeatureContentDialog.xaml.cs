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
            this.InitializeComponent();
        }

        public async Task<string> DoIt()
        {
            await ShowAsync();
            if (addtrue)
                return FeatureIDBox.Text;
            return "";
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
