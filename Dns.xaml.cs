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
using Microsoft.Phone.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Phone.Shell;

namespace network_toolkit
{
    public partial class Dns : PhoneApplicationPage
    {
        public Dns()
        {
            InitializeComponent();
        }

        private bool validateHost()
        {
            // http://stackoverflow.com/a/106223
            if (Regex.IsMatch(host.Text, @"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$"))
                return true;
            return false;
        }


        private void resolve_Click(object sender, EventArgs e)
        {
            try
            {
                result.Text = "";
                performanceProgressBar.IsIndeterminate = true;
                resultText.Visibility = System.Windows.Visibility.Visible;

                DnsEndPoint dnsEndPoint = new DnsEndPoint(host.Text, 0);
                DeviceNetworkInformation.ResolveHostNameAsync(dnsEndPoint, nameResolutionCallback, null);
            }
            catch (Exception ex)
            {
            }
        }

        private void nameResolutionCallback(NameResolutionResult nameResolutionResult)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                if (nameResolutionResult.NetworkErrorCode == NetworkError.Success)
                {
                    foreach (IPEndPoint ipEndPoint in nameResolutionResult.IPEndPoints)
                        stringBuilder.Append(ipEndPoint.Address + "\n");
                }
                else
                    stringBuilder.Append(nameResolutionResult.NetworkErrorCode);
                // Otherwise invalid cross-thread access exception.
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    performanceProgressBar.IsIndeterminate = false;
                    result.Text = stringBuilder.ToString();
                    (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = true;
                });
            }
            catch (Exception e)
            {
            }
        }

        private void email_Click(object sender, EventArgs e)
        {
            #if RELEASE
                EmailComposeTask emailComposeTask = new EmailComposeTask();
                emailComposeTask.Subject = "DNS lookup - " + host.Text;
                emailComposeTask.Body = result.Text;
                emailComposeTask.Show();
            # else
                MessageBox.Show("On Windows Phone Emulator, an exception occurs when using the email compose task. Test the email compose task on a physical device.");
            #endif
        }

        private void host_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(validateHost())
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            else
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
        }
    }
}