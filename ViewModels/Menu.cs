using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace network_toolkit.ViewModels
{
    public class Menu
    {
        public string Title { get; private set; }
        public string Url { get; private set; }

        public Menu(string Title, string Url)
        {
            this.Title = Title;
            this.Url = Url;
        }
    }
}
