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
    public partial class Speed_test : PhoneApplicationPage
    {
        public Speed_test()
        {
            InitializeComponent();
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot.SelectedIndex == 0)
                ApplicationBar.IsVisible = true;
            else
                ApplicationBar.IsVisible = false;
        }

        private void test_Click(object sender, EventArgs e)
        {
            if (!performanceProgressBar.IsIndeterminate)
            {
                performanceProgressBar.IsIndeterminate = true;
                applicationBarIconButton.IsEnabled = false;
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloadCompleted);
                    webClient.DownloadStringAsync(new Uri((listPicker.SelectedItem as ListPickerItem).Tag.ToString()));
                }
                catch (Exception ex)
                {
                    err.Visibility = System.Windows.Visibility.Visible;
                    performanceProgressBar.IsIndeterminate = false;
                    applicationBarIconButton.IsEnabled = true;
                }
            }
        }

        private void downloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                MessageBox.Show(e.Result.ToString());
            }
            catch (Exception ex)
            {
                err.Visibility = System.Windows.Visibility.Visible;
            }
            finally
            {
                performanceProgressBar.IsIndeterminate = false;
                applicationBarIconButton.IsEnabled = true;
            }
        }
    }
}