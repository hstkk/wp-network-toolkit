//2012 Sami Hostikka <dev@01.fi>
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
using Microsoft.Phone.Shell;

namespace network_toolkit
{
    public partial class Settings : PhoneApplicationPage
    {
        #region Initialize
        public Settings()
        {
            InitializeComponent();

            //toggleSwitch.IsChecked = (Application.Current as App).isLocationAllowed;
            if ((Application.Current as App).homescreen == 0)
                favorites.IsChecked = true;
            else
                tools.IsChecked = true;
        }
        #endregion

        #region Events
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            /*if (PhoneApplicationService.Current.State.ContainsKey("location"))
                PhoneApplicationService.Current.State.Remove("location");
            PhoneApplicationService.Current.State.Add("location", toggleSwitch.IsChecked);*/
            if (PhoneApplicationService.Current.State.ContainsKey("homescreen"))
                PhoneApplicationService.Current.State.Remove("homescreen");
            PhoneApplicationService.Current.State.Add("homescreen", favorites.IsChecked);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            bool tmp;
            /*if (PhoneApplicationService.Current.State.ContainsKey("location") && bool.TryParse(PhoneApplicationService.Current.State["location"] as string, out tmp))
                toggleSwitch.IsChecked = tmp;*/
            if (PhoneApplicationService.Current.State.ContainsKey("homescreen") && bool.TryParse(PhoneApplicationService.Current.State["homescreen"] as string, out tmp))
            {
                if (tmp)
                    favorites.IsChecked = true;
                else
                    tools.IsChecked = true;
            }
        }
        
        /// <summary>
        /// When toggleswitch is unchecked updates toggleswitch content and saves state to settings.
        /// </summary>
        /*private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            toggleSwitch.Content = "Disallow";
            (Application.Current as App).isLocationAllowed = false;
        }*/

        /// <summary>
        /// When toggleswitch is checked updates toggleswitch content and saves state to settings.
        /// </summary>
        /*private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            toggleSwitch.Content = "Allow";
            (Application.Current as App).isLocationAllowed = true;
        }*/


        /// <summary>
        /// Saves selected homescreen to settings.
        /// </summary>
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton radioButton = sender as RadioButton;
                int index;
                if (radioButton != null && int.TryParse(radioButton.Tag.ToString(), out index))
                    (Application.Current as App).homescreen = index;
            }
            catch (Exception ex)
            {
            }
        }
    #endregion
    }
}