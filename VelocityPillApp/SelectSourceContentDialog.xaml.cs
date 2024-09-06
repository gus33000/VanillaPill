using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace VelocityPillApp
{
    public sealed partial class SelectSourceContentDialog : ContentDialog
    {
        public SelectSourceContentDialog()
        {
            InitializeComponent();

            RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
            bool checkedR = true;
            if (reg.RegEnumKey(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, "", out IReadOnlyList<RegistryHelper.REG_ITEM> items) == RegistryHelper.REG_STATUS.SUCCESS)
            {
                foreach (RegistryHelper.REG_ITEM item in items)
                {
                    if (reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, $@"{item.Name}\Microsoft\Windows NT\CurrentVersion\Notifications\Data", App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data) == RegistryHelper.REG_STATUS.SUCCESS)
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
            ContentPanel.Children.Add(new RadioButton() { Content = "Offline", GroupName = "SelectHive", IsChecked = checkedR });
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
            foreach (Windows.UI.Xaml.UIElement btn in ContentPanel.Children)
            {
                RadioButton radioBtn = (RadioButton)btn;
                if (radioBtn.IsChecked == true)
                {
                    if (radioBtn.Content == "Offline")
                    {
                        return;
                    }

                    if (reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, $@"{(string)radioBtn.Content}\Microsoft\Windows NT\CurrentVersion\Notifications\Data", App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out _, out _) == RegistryHelper.REG_STATUS.SUCCESS)
                    {
                        App.key = $@"{(string)radioBtn.Content}\Microsoft\Windows NT\CurrentVersion\Notifications\Data";
                        return;
                    }
                    else if (reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, $@"{(string)radioBtn.Content}\Notifications\Data", App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out _, out _) == RegistryHelper.REG_STATUS.SUCCESS)
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
