using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VelocityPillApp
{
    public sealed partial class CommandContentDialog : ContentDialog
    {
        public CommandContentDialog(string cmd)
        {
            InitializeComponent();
            CommandTextBox.IsReadOnly = false;
            CommandTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, cmd);
            CommandTextBox.IsReadOnly = true;
        }
    }
}
