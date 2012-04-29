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

namespace network_toolkit
{
    public partial class settings : PhoneApplicationPage
    {
        public settings()
        {
            InitializeComponent();
        }

        private void toggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            toggleSwitch.Content = "Disallow";
        }

        private void toggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            toggleSwitch.Content = "Allow";
        }

        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            int index;
            if (radioButton != null && int.TryParse(radioButton.Tag.ToString(), out index))
                index++;
        }
    }
}