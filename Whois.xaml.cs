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
    public partial class Whois : PhoneApplicationPage
    {
        public Whois()
        {
            InitializeComponent();
            /*TcpPing whois = new TcpPing();
            whois.connect("whois.internic.net", 43);
            whois.send("google.com\r\n");
            whois.receive();
            whois.close();*/
        }
    }
}