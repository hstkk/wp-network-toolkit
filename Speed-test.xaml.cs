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
using System.Text.RegularExpressions;

namespace network_toolkit
{
    public partial class Speed_test : PhoneApplicationPage
    {
        bool textfieldGotFocus = false;
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

        private void download_Click(object sender, EventArgs e)
        {
            if (!performanceProgressBar.IsIndeterminate)
            {
                performanceProgressBar.IsIndeterminate = true;
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
                testFile.IsEnabled = false;
                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(downloadCompleted);
                    webClient.DownloadStringAsync(new Uri(testFile.Text, UriKind.Absolute));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    err.Visibility = System.Windows.Visibility.Visible;
                    performanceProgressBar.IsIndeterminate = false;
                    (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
                    testFile.IsEnabled = true;
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
                MessageBox.Show(ex.ToString());
                err.Visibility = System.Windows.Visibility.Visible;
            }
            finally
            {
                performanceProgressBar.IsIndeterminate = false;
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
                testFile.IsEnabled = true;
            }
        }

        private void testFile_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textfieldGotFocus)
            {
                MessageBox.Show("kk");
                if (!Regex.IsMatch(testFile.Text, @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"))
                {
                    (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
                    MessageBox.Show("URL is not valid");
                }
                else if (!performanceProgressBar.IsIndeterminate)
                    (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            }
        }

        private void testFile_GotFocus(object sender, RoutedEventArgs e)
        {
            textfieldGotFocus = true;
        }
    }
}