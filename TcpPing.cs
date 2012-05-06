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

namespace network_toolkit
{
    public class TcpPing
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
            string result = "";
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
            MessageBox.Show(result);
            return result;
        }

        /// <summary>
        /// Send message to remote host using earlier socket connection.
        /// </summary>
        /// <param name="message">Message to remote host</param>
        /// <returns></returns>
        public string send(string message)
        {
            string response = "timeout";

            if (socket != null)
            {
                try
                {
                    SocketAsyncEventArgs socketAsyncEventArgs = new SocketAsyncEventArgs();
                    socketAsyncEventArgs.RemoteEndPoint = socket.RemoteEndPoint;

                    socketAsyncEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object sender, SocketAsyncEventArgs e)
                    {
                        response = e.SocketError.ToString();
                        manualResetEvent.Set();
                    });

                    byte[] data = Encoding.UTF8.GetBytes(message);
                    socketAsyncEventArgs.SetBuffer(data, 0, data.Length);

                    manualResetEvent.Reset();
                    socket.SendAsync(socketAsyncEventArgs);
                    manualResetEvent.WaitOne(TIMEOUT);
                }
                catch (Exception e)
                {
                }
            }
            else
                response = "no socket";
            MessageBox.Show(response);
            return response;
        }

        public void close()
        {
            if (socket != null)
                socket.Close();
        }
    }
}
