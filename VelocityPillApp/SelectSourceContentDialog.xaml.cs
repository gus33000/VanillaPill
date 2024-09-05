using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace VelocityPillApp
{
    public sealed partial class SelectSourceContentDialog : ContentDialog
    {
        public SelectSourceContentDialog()
        {
            this.InitializeComponent();

            RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
            IReadOnlyList<RegistryHelper.REG_ITEM> items;
            bool checkedR = true;
            if (reg.RegEnumKey(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, "", out items) == RegistryHelper.REG_STATUS.SUCCESS)
            {
                foreach (RegistryHelper.REG_ITEM item in items)
                {
                    RegistryHelper.REG_VALUE_TYPE datatype;
                    string data;
                    if (reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, $@"{item.Name}\Microsoft\Windows NT\CurrentVersion\Notifications\Data", App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out datatype, out data) == RegistryHelper.REG_STATUS.SUCCESS)
                    {
                        ContentPanel.Children.Add(new RadioButton() { Content = item.Name, GroupName = "SelectHive", IsChecked = checkedR });
                        checkedR = false;
                    }
                    else if (reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, $@"{item.Name}\Notifications\Data", App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out datatype, out data) == RegistryHelper.REG_STATUS.SUCCESS)
                    {
                        ContentPanel.Children.Add(new RadioButton() { Content = item.Name, GroupName = "SelectHive", IsChecked = checkedR });
                        checkedR = false;
                    }
                }
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
            foreach (var btn in ContentPanel.Children)
            {
                var radioBtn = (RadioButton)btn;
                if (radioBtn.IsChecked == true)
                {
                    RegistryHelper.REG_VALUE_TYPE datatype;
                    string data;
                    if (reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, $@"{(string)radioBtn.Content}\Microsoft\Windows NT\CurrentVersion\Notifications\Data", App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out datatype, out data) == RegistryHelper.REG_STATUS.SUCCESS)
                    {
                        App.key = $@"{(string)radioBtn.Content}\Microsoft\Windows NT\CurrentVersion\Notifications\Data";
                        return;
                    }
                    else if (reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, $@"{(string)radioBtn.Content}\Notifications\Data", App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out datatype, out data) == RegistryHelper.REG_STATUS.SUCCESS)
                    {
                        App.key = $@"{(string)radioBtn.Content}\Notifications\Data";
                        return;
                    }
                }
            }
            args.Cancel = true;
        }
    }
}
