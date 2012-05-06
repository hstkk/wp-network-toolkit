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
using System.Net.Sockets;
using System.Threading;
using System.Text;
using Microsoft.Phone.Net.NetworkInformation;

namespace network_toolkit
{
    public class TcpPing : IDisposable
    {
        Socket socket = null;
        static ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        const int TIMEOUT = 5000;
        const int MAX_BUFFER_SIZE = 2048;

        /// <summary>
        /// Opens TCP socket based on host and port.
        /// </summary>
        /// <param name="host">Remote host</param>
        /// <param name="port">TCP port</param>
        /// <returns></returns>
        public string connect(string host, int port)
        {
            string result = "Can't connect to remote host!";
            try
            {
                DnsEndPoint dnsEndPoint = new DnsEndPoint(host, port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
                socketAsyncEventArgs.RemoteEndPoint = dnsEndPoint;

                socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object sender, SocketAsyncEventArgs e)
                {
                    result = e.SocketError.ToString();
                    manualResetEvent.Set();
                });

                manualResetEvent.Reset();
                socket.ConnectAsync(socketAsyncEventArgs);
                manualResetEvent.WaitOne(TIMEOUT);
            }
            catch (Exception e)
            {
            }
            return result;
        }

        /// <summary>
        /// Closes socket.
        /// </summary>
        public void Dispose()
        {
            if (socket != null)
                socket.Close();
        }
    }
}
