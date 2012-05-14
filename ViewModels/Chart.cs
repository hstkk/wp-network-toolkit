//2012 Sami Hostikka <dev@01.fi>
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
    public class Chart
    {
        #region Initialize
        public string title { get; set; }
        public double download { get; set; }
        
            public Chart(string title)
        {
            this.title = title;
        }
        #endregion
    }
}
