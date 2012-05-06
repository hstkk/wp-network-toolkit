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
using Microsoft.Phone.Tasks;
using System.Text.RegularExpressions;

namespace network_toolkit
{
    public partial class Tcp_ping : PhoneApplicationPage
    {
        bool hostGotFocus = false, portGotFocus = false, hostValid = false, portValid = false;
        public Tcp_ping()
        {
            InitializeComponent();
        }

        private void enablePing(){
            if (hostValid && portValid)
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            else
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
        }

        private void host_GotFocus(object sender, RoutedEventArgs e)
        {
            hostGotFocus = true;
        }

        private void host_LostFocus(object sender, RoutedEventArgs e)
        {
            if (hostGotFocus)
            {
                // http://stackoverflow.com/a/106223
                if (Regex.IsMatch(host.Text, @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$") || Regex.IsMatch(host.Text, @"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$"))
                    hostValid = true;
                else
                    hostValid = false;
                enablePing();
            }
        }

        private void port_GotFocus(object sender, RoutedEventArgs e)
        {
            portGotFocus = true;
        }

        private void port_LostFocus(object sender, RoutedEventArgs e)
        {
            if (portGotFocus)
            {
                int i = 0;
                if (int.TryParse(port.Text, out i) && i > 0 && i <= 65535)
                    portValid = true;
                else
                {
                    portValid = false;
                    MessageBox.Show("Port number is not valid, use ports 1 to 65535");
                }
                enablePing();
            }
        }

        private void ping_Click(object sender, EventArgs e)
        {
            resultText.Visibility = System.Windows.Visibility.Visible;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
            (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = true;
            performanceProgressBar.IsIndeterminate = false;
            using (TcpPing tcpPing = new TcpPing())
            {
                int i = 0;
                int.TryParse(port.Text, out i);
                result.Text = tcpPing.connect(host.Text, i);
            }
            performanceProgressBar.IsIndeterminate = true;
            (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
        }

        private void email_Click(object sender, EventArgs e)
        {
            #if RELEASE
                EmailComposeTask emailComposeTask = new EmailComposeTask();
                emailComposeTask.Subject = "TCP ping - " + host.Text + ":" + port.Text;
                emailComposeTask.Body = result.Text;
                emailComposeTask.Show();
            # else
                MessageBox.Show("On Windows Phone Emulator, an exception occurs when using the email compose task. Test the email compose task on a physical device.");
            #endif
        }
    }
}