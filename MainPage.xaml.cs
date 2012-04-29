using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using network_toolkit.ViewModels;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Net.NetworkInformation; 

namespace network_toolkit
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            DeviceNetworkInformation.NetworkAvailabilityChanged +=new EventHandler<NetworkNotificationEventArgs>(networkInfo_NetworkAvailabilityChanged);
        }

        private void getNetworkInfo()
        {
            carrier.Text = DeviceNetworkInformation.CellularMobileOperator.ToLower();
            networkAvailable.Text = DeviceNetworkInformation.IsNetworkAvailable.ToString();
            cellularDataEnabled.Text = DeviceNetworkInformation.IsCellularDataEnabled.ToString();
            wifiEnabled.Text = DeviceNetworkInformation.IsWiFiEnabled.ToString();
        }

        #region Events
        /// <summary>
        /// Load data for the ViewModel Items.
        /// </summary>
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void contextMenu_Click(object sender, SelectionChangedEventArgs e)
        {
            //ContextMenu contextMenu = (sender as ContextMenu);
            //MessageBox.Show(contextMenu.);
        }

        /// <summary>
        /// Handle selection changed on ListBox
        /// </summary>
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = (sender as ListBox);
            // If selected index is -1 (no selection) do nothing
            if (listbox.SelectedIndex == -1)
                return;

            Menu menu = listbox.SelectedItem as Menu;
            MessageBox.Show(menu.Url);
            // Navigate to the new page
            //NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));

            // Reset selected index to -1 (no selection)
            listbox.SelectedIndex = -1;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            getNetworkInfo();
        }

        private void wifi_Click(object sender, RoutedEventArgs e)
        {
            ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.WiFi;
            connectionSettingsTask.Show();
        }

        private void cellular_Click(object sender, RoutedEventArgs e)
        {
            ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
            connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Cellular;
            connectionSettingsTask.Show();
        }

        private void networkInfo_NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            getNetworkInfo();
        }
        #endregion
    }
}