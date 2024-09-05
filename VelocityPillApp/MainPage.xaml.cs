using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VelocityPillApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly DataTemplate dtSmall;
        private readonly DataTemplate dtEnlarged;

        public MainPage()
        {
            this.InitializeComponent();

            //if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
                //AddFeatureButton.AllowFocusOnInteraction = true;

            dtSmall = (DataTemplate)Resources["dtSmall"];
            dtEnlarged = (DataTemplate)Resources["dtEnlarged"];

            Window.Current.SetTitleBar(ChromeTop);
            ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Colors.Transparent;
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            CoreApplication.GetCurrentView().TitleBar.IsVisibleChanged += TitleBar_IsVisibleChanged;

            //CurrentFeaturesList.ItemsSource = App.helper.GetFeatureStates();

            ((CollectionViewSource)Resources["FeatureGroups"]).Source = App.itemSource;

            ((CollectionViewSource)Resources["TemplateGroups"]).Source = App.itemSource2;
        }

        private void TitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (CoreApplication.GetCurrentView().TitleBar.IsVisible)
            {
                Thickness mg = MainGrid.Margin;
                mg.Top = 32;
                MainGrid.Margin = mg;
            }
            else
            {
                Thickness mg = MainGrid.Margin;
                mg.Top = 0;
                MainGrid.Margin = mg;
            }
        }

        private void SelectButton_Checked(object sender, RoutedEventArgs e)
        {
            DeleteSelectedFeatures.Visibility = Visibility.Visible;
            ToggleSelectedFeatures.Visibility = Visibility.Visible;
        }

        private void SelectButton_Unchecked(object sender, RoutedEventArgs e)
        {
            DeleteSelectedFeatures.Visibility = Visibility.Collapsed;
            ToggleSelectedFeatures.Visibility = Visibility.Collapsed;
        }

        private bool JustClicked = false;

        private async void CurrentFeaturesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            JustClicked = true;
            ListView lv = (sender as ListView);

            if (lv.SelectedItem == null)
            {
                lv.SelectedItem = e.ClickedItem;
            }

            else
            {
                if (lv.SelectedItem.Equals(e.ClickedItem))
                {
                    lv.SelectedItem = null;
                }

                else
                {
                    lv.SelectedItem = e.ClickedItem;
                }
            }
            await Task.Delay(400);
            JustClicked = false;
        }

        private void CurrentFeaturesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0)
            {
                foreach (object item in e.RemovedItems)
                {
                    try
                    {
                        ((ListViewItem)(sender as ListView).ContainerFromItem(item)).ContentTemplate = dtSmall;
                    }
                    catch
                    {

                    }
                }

                foreach (object item in e.AddedItems)
                {
                    try
                    {
                        ListView listview = sender as ListView;
                        object firstaddeditem = e.AddedItems[0];
                        ListViewItem container = (ListViewItem)listview.ContainerFromItem(firstaddeditem);
                        if (container != null)
                        {
                            container.ContentTemplate = dtEnlarged;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private async void removeIDButton_Click(object sender, RoutedEventArgs e)
        {
            VelocityHelper.WNFState state = (sender as Button).DataContext as VelocityHelper.WNFState;

            App.helper.RemoveFeatureState(state);

            await Task.Delay(400);
            //CurrentFeaturesList.ItemsSource = App.helper.GetFeatureStates();

            List<AlphaKeyGroup<VelocityHelper.WNFState>> itemSource = AlphaKeyGroup<VelocityHelper.WNFState>.CreateGroups(App.helper.GetFeatureStates(), CultureInfo.InvariantCulture,
                                 s => s.DisplayableID, true);
            ((CollectionViewSource)Resources["FeatureGroups"]).Source = itemSource;
        }

        private async void FeatureStateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (JustClicked) return;
                VelocityHelper.WNFState state = (sender as ComboBox).DataContext as VelocityHelper.WNFState;

                App.helper.SetFeatureState(state);

                await Task.Delay(400);
                //CurrentFeaturesList.ItemsSource = App.helper.GetFeatureStates();

                List<AlphaKeyGroup<VelocityHelper.WNFState>> itemSource = AlphaKeyGroup<VelocityHelper.WNFState>.CreateGroups(App.helper.GetFeatureStates(), CultureInfo.InvariantCulture,
                                     s => s.DisplayableID, true);
                ((CollectionViewSource)Resources["FeatureGroups"]).Source = itemSource;

                JustClicked = true;
                CurrentFeaturesList.ScrollIntoView(CurrentFeaturesList.Items.First(x => (x as VelocityHelper.WNFState).ID == state.ID), ScrollIntoViewAlignment.Leading);
                CurrentFeaturesList.SelectedItem = CurrentFeaturesList.Items.First(x => (x as VelocityHelper.WNFState).ID == state.ID);
                await Task.Delay(400);
            }
            catch
            {

            }
            JustClicked = false;
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(400);
            //CurrentFeaturesList.ItemsSource = App.helper.GetFeatureStates();

            List<AlphaKeyGroup<VelocityHelper.WNFState>> itemSource = AlphaKeyGroup<VelocityHelper.WNFState>.CreateGroups(App.helper.GetFeatureStates(), CultureInfo.InvariantCulture,
                                 s => s.DisplayableID, true);
            ((CollectionViewSource)Resources["FeatureGroups"]).Source = itemSource;
        }

        private async void EditStateButton_Click(object sender, RoutedEventArgs e)
        {
            VelocityHelper.WNFState state = (sender as Button).DataContext as VelocityHelper.WNFState;

            App.helper.SetFeatureState(state);

            await Task.Delay(400);
            //CurrentFeaturesList.ItemsSource = App.helper.GetFeatureStates();

            List<AlphaKeyGroup<VelocityHelper.WNFState>> itemSource = AlphaKeyGroup<VelocityHelper.WNFState>.CreateGroups(App.helper.GetFeatureStates(), CultureInfo.InvariantCulture,
                                 s => s.DisplayableID, true);
            ((CollectionViewSource)Resources["FeatureGroups"]).Source = itemSource;

            JustClicked = true;
            CurrentFeaturesList.ScrollIntoView(CurrentFeaturesList.Items.First(x => (x as VelocityHelper.WNFState).ID == state.ID), ScrollIntoViewAlignment.Leading);
            CurrentFeaturesList.SelectedItem = CurrentFeaturesList.Items.First(x => (x as VelocityHelper.WNFState).ID == state.ID);
            await Task.Delay(400);
            JustClicked = false;
        }

        private void RefreshDumpButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //byte[] result;
                //var ret = new VelocityPillRT.VelocityPill().GetFeatureState(App.featureStoreName, out result);

                RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
                reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data);

                string binstr = data.Substring(8);


                string s = Regex.Replace(binstr, ".{2}", "$0-").TrimEnd('-');
                String[] tempAry = s.Split('-');
                byte[] result = new byte[tempAry.Length];
                for (int i = 0; i < tempAry.Length; i++)
                {
                    result[i] = Convert.ToByte(tempAry[i], 16);
                }

                DumpTextBox.Text = BitConverter.ToString(result).Replace("-", "");
            }
            catch
            {

            }
        }

        private static uint SwapEndianness(uint value)
        {
            uint b1 = (value >> 0) & 0xff;
            uint b2 = (value >> 8) & 0xff;
            uint b3 = (value >> 16) & 0xff;
            uint b4 = (value >> 24) & 0xff;

            return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
        }

        private void WriteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string s = Regex.Replace(DumpTextBox.Text, ".{2}", "$0-").TrimEnd('-');
                String[] tempAry = s.Split('-');
                byte[] buffer = new byte[tempAry.Length];
                for (int i = 0; i < tempAry.Length; i++)
                {
                    buffer[i] = Convert.ToByte(tempAry[i], 16);
                }

                byte[] result = buffer;

                int retc = new VelocityPillRT.VelocityPill().SetFeatureState(App.featureStoreName, result);
                if (retc != 0)
                {
                    RegistryHelper.CRegistryHelper reg = new RegistryHelper.CRegistryHelper();
                    reg.RegQueryValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, out RegistryHelper.REG_VALUE_TYPE datatype, out string data);
                    string countstr = data.Substring(0, 8);
                    uint count = SwapEndianness(uint.Parse(countstr, System.Globalization.NumberStyles.HexNumber));
                    count++;
                    string datatowrite = BitConverter.ToString(BitConverter.GetBytes(count)).Replace("-", "") + BitConverter.ToString(result).Replace("-", "");
                    reg.RegSetValue(RegistryHelper.REG_HIVES.HKEY_LOCAL_MACHINE, App.key, App.featureStoreName, RegistryHelper.REG_VALUE_TYPE.REG_BINARY, datatowrite);
                }

                int ret = new VelocityPillRT.VelocityPill().GetFeatureState(App.featureStoreName, out result);
                DumpTextBox.Text = BitConverter.ToString(result).Replace("-", "");
            }
            catch
            {

            }
        }
        
        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.helper.ClearFeatureStates();
                await Task.Delay(400);
                //CurrentFeaturesList.ItemsSource = App.helper.GetFeatureStates();

                List<AlphaKeyGroup<VelocityHelper.WNFState>> itemSource = AlphaKeyGroup<VelocityHelper.WNFState>.CreateGroups(App.helper.GetFeatureStates(), CultureInfo.InvariantCulture,
                                     s => s.DisplayableID, true);
                ((CollectionViewSource)Resources["FeatureGroups"]).Source = itemSource;
            }
            catch
            {

            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            //App.helper.ClearFeatureStates();
            List<VelocityHelper.WNFState> lst = new List<VelocityHelper.WNFState>();

            foreach (object item in TemplatesList.SelectedItems)
            {
                lst.Add(item as VelocityHelper.WNFState);
            }

            App.helper.AddMultipleFeatureStates(lst, true);
        }

        private void ResetTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            TemplatesList.SelectedItems.Clear();
        }

        private void TemplatesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TemplatesList.SelectedItems.Count > 255)
            {
                TemplatesList.SelectedItems.RemoveAt(255);
            }
        }

        private void ApplyAddButton_Click(object sender, RoutedEventArgs e)
        {
            List<VelocityHelper.WNFState> lst = new List<VelocityHelper.WNFState>();

            foreach (object item in TemplatesList.SelectedItems)
            {
                lst.Add(item as VelocityHelper.WNFState);
            }

            App.helper.AddMultipleFeatureStates(lst, false);
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ret = await new AddFeatureContentDialog().DoIt();
                if (ret.Trim() != "" && ret.Count() == 10)
                {
                    uint id = uint.Parse(ret.Substring(2), System.Globalization.NumberStyles.HexNumber);

                    VelocityHelper.WNFState state = new VelocityHelper.WNFState() { ID = id, WNFStateData = new byte[] { 01, 08, 00, 00, 00, 00, 00, 00 } };

                    App.helper.AddFeatureState(state);

                    await Task.Delay(400);
                    //CurrentFeaturesList.ItemsSource = App.helper.GetFeatureStates();

                    List<AlphaKeyGroup<VelocityHelper.WNFState>> itemSource = AlphaKeyGroup<VelocityHelper.WNFState>.CreateGroups(App.helper.GetFeatureStates(), CultureInfo.InvariantCulture,
                                         s => s.DisplayableID, true);
                    ((CollectionViewSource)Resources["FeatureGroups"]).Source = itemSource;

                    JustClicked = true;
                    CurrentFeaturesList.ScrollIntoView(CurrentFeaturesList.Items.First(x => (x as VelocityHelper.WNFState).ID == state.ID), ScrollIntoViewAlignment.Leading);
                    CurrentFeaturesList.SelectedItem = CurrentFeaturesList.Items.First(x => (x as VelocityHelper.WNFState).ID == state.ID);
                    await Task.Delay(400);
                    JustClicked = false;
                }
            }
            catch
            {

            }
        }
    }
}
