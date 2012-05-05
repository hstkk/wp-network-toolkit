﻿//2012 Sami Hostikka <dev@01.fi>
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
using Microsoft.Phone.Shell; 

namespace network_toolkit
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region Initialize
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(mainPage_Loaded);

            DeviceNetworkInformation.NetworkAvailabilityChanged +=new EventHandler<NetworkNotificationEventArgs>(networkInfo_NetworkAvailabilityChanged);
        }
        #endregion

        #region Etc
        /// <summary>
        /// Gets network information and sets it to network panoramaItem.
        /// </summary>
        private void getNetworkInfo()
        {
            carrier.Text = DeviceNetworkInformation.CellularMobileOperator.ToLower();
            networkAvailable.Text = DeviceNetworkInformation.IsNetworkAvailable.ToString().ToLower();
            cellularDataEnabled.Text = DeviceNetworkInformation.IsCellularDataEnabled.ToString().ToLower();
            wifiEnabled.Text = DeviceNetworkInformation.IsWiFiEnabled.ToString().ToLower();
        }
        #endregion

        #region Events
        /// <summary>
        /// Load data for the ViewModel Items and set settings if first run.
        /// </summary>
        private void mainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
                App.ViewModel.LoadData();
            if ((Application.Current as App).isFirstRun)
            {
                (Application.Current as App).isFirstRun = false;
                NavigationService.Navigate(new Uri("/settings.xaml", UriKind.Relative));
            }
        }

        private void contextMenu_Click(object sender, SelectionChangedEventArgs e)
        {
            //ContextMenu contextMenu = (sender as ContextMenu);
            //MessageBox.Show(contextMenu.);
        }

        /// <summary>
        /// Handle selection changed on ListBox.
        /// </summary>
        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox listbox = (sender as ListBox);
                // If selected index is -1 (no selection) do nothing
                if (listbox.SelectedIndex == -1)
                    return;

                Menu menu = listbox.SelectedItem as Menu;
                if(menu != null)
                    // Navigate to the new page
                    NavigationService.Navigate(new Uri(menu.Url, UriKind.Relative));

                // Reset selected index to -1 (no selection)
                listbox.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// When navigated to page, gets settings and network information.
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            panorama.DefaultItem = panorama.Items[(Application.Current as App).homescreen];
            getNetworkInfo();
        }

        /// <summary>
        /// Opens connection settings
        /// </summary>
        private void setting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button button = sender as Button;
                if (button != null)
                {
                    ConnectionSettingsTask connectionSettingsTask = new ConnectionSettingsTask();
                    if (button.Content.ToString().StartsWith("wifi"))
                        connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.WiFi;
                    else
                        connectionSettingsTask.ConnectionSettingsType = ConnectionSettingsType.Cellular;
                    connectionSettingsTask.Show();
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// When a network changes, updates network information.
        /// </summary>
        private void networkInfo_NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            getNetworkInfo();
        }

        /// <summary>
        /// Navigates to selected page.
        /// </summary>
        private void applicationBarMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ApplicationBarMenuItem applicationBarMenuItem = sender as ApplicationBarMenuItem;
                if (applicationBarMenuItem != null)
                {
                    string uri = "";
                    switch (applicationBarMenuItem.Text)
                    {
                        case "Help":
                            uri = "help";
                            break;
                        case "Settings":
                            //TODO
                            App.ViewModel.addToFavorites("help", "/help.xaml");
                            uri = "settings";
                            break;
                        default:
                            MessageBox.Show("Sorry, page not found");
                            break;
                    }
                    if (!uri.Equals(""))
                        NavigationService.Navigate(new Uri("/" + uri + ".xaml", UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion
    }
}