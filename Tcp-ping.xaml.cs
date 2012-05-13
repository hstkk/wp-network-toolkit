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
using System.Text;

namespace network_toolkit
{
    public partial class Tcp_ping : PhoneApplicationPage
    {
        public Tcp_ping()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            if (PhoneApplicationService.Current.State.ContainsKey("tcpHost"))
                PhoneApplicationService.Current.State.Remove("tcpHost");
            PhoneApplicationService.Current.State.Add("tcpHost", host.Text);
            if (PhoneApplicationService.Current.State.ContainsKey("tcpPort"))
                PhoneApplicationService.Current.State.Remove("tcpPort");
            PhoneApplicationService.Current.State.Add("tcpPort", port.Text);
            if (PhoneApplicationService.Current.State.ContainsKey("tcpResult"))
                PhoneApplicationService.Current.State.Remove("tcpResult");
            PhoneApplicationService.Current.State.Add("tcpResult", result.Text);
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string tmp;
            if (PhoneApplicationService.Current.State.ContainsKey("tcpHost"))
            {
                tmp = PhoneApplicationService.Current.State["tcpHost"] as string;
                if (!tmp.Equals(""))
                    host.Text = tmp;
            }
            if (PhoneApplicationService.Current.State.ContainsKey("tcpPort"))
            {
                tmp = PhoneApplicationService.Current.State["tcpPort"] as string;
                if (!tmp.Equals(""))
                    port.Text = tmp;
            }
            if (PhoneApplicationService.Current.State.ContainsKey("tcpResult"))
            {
                tmp = PhoneApplicationService.Current.State["tcpResult"] as string;
                if (!tmp.Equals(""))
                    result.Text = tmp;
            }
        }

        private void enablePing(bool isPort = false)
        {
            if (validatePort(isPort) && validateHost())
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            else
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
        }

        private bool validatePort(bool isPort = false){
            int i = 0;
            bool valid = true;
            if (port.Text.Equals(""))
                valid = false;
            else if (int.TryParse(port.Text, out i) && i < 0 && i > 65535)
                valid = false;
            else
                foreach (string tmp in port.Text.Split(' '))
                    if (!(int.TryParse(tmp, out i) && i > 0 && i <= 65535))
                        valid = false;
            if (isPort && !valid)
                MessageBox.Show("Port number is not valid, use ports 1 to 65535.");
            return valid;
        }

        private bool validateHost()
        {
            // http://stackoverflow.com/a/106223
            if (Regex.IsMatch(host.Text, @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$") || Regex.IsMatch(host.Text, @"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$"))
                return true;
             return false;
        }

        private void ping_Click(object sender, EventArgs e)
        {
            if (validatePort() && validateHost())
            {
                resultText.Visibility = System.Windows.Visibility.Visible;
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
                (ApplicationBar.Buttons[1] as ApplicationBarIconButton).IsEnabled = true;
                performanceProgressBar.IsIndeterminate = true;
                StringBuilder stringBuilder = new StringBuilder();

                int i;
                foreach (string tmp in port.Text.Split(' '))
                {
                    int.TryParse(tmp, out i);
                    using (TcpPing tcpPing = new TcpPing())
                    {
                        stringBuilder.Append(i + ": " + tcpPing.connect(host.Text, i) + "\n");
                    }
                }
                performanceProgressBar.IsIndeterminate = false;
                result.Text = stringBuilder.ToString();
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = true;
            }
            else
                (ApplicationBar.Buttons[0] as ApplicationBarIconButton).IsEnabled = false;
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

        private void port_TextChanged(object sender, TextChangedEventArgs e)
        {
            enablePing(true);
        }

        private void host_TextChanged(object sender, TextChangedEventArgs e)
        {
            enablePing();
        }

        private void orientationChanged(object sender, OrientationChangedEventArgs e)
        {
            if ((e.Orientation & PageOrientation.Portrait) == (PageOrientation.Portrait))
            {
                Grid.SetColumn(hostStack, 0);
                Grid.SetRow(hostStack, 0);
                Grid.SetColumn(portStack, 0);
                Grid.SetRow(portStack, 1);
                scrollViewer.Height = 336;
                grid.ColumnDefinitions[1].Width = GridLength.Auto;
                grid.ColumnDefinitions[2].Width = GridLength.Auto;
                grid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            }
            else
            {
                grid.ColumnDefinitions[0].Width = new GridLength(0);
                grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
                Grid.SetColumn(hostStack, 1);
                Grid.SetRow(hostStack, 0);
                Grid.SetColumn(portStack, 2);
                Grid.SetRow(portStack, 0);
                scrollViewer.Height = 200;
            }
        }
    }
}