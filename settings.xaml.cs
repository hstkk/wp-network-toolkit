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

            if ((Application.Current as App).homescreen == 0)
                favorites.IsChecked = true;
            else
                tools.IsChecked = true;
        }
        #endregion

        #region Events
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