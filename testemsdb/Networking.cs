using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using testemsdb;

namespace HCV_TestHarness
{
	public class HCVClient
	{
        private readonly int MAX_MSG_LEN = 256;
        private readonly int SV_PORT = 10060;

        private Socket ServerConn;
		public bool IsConnected { get; private set; }

		public HCVClient(IPAddress server)
		{
			try
			{
				IPEndPoint serverEndPoint = new IPEndPoint(server, SV_PORT);
				ServerConn = new Socket(IPAddress.Parse(GetLocalIPAddress()).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				ServerConn.Connect(serverEndPoint);

				if (ServerConn.Connected)
				{
					IsConnected = true;
				}
			}
			catch (Exception)
			{
				IsConnected = false;
			}
		}

		public void Dispose()
		{
			ServerConn.Close();
		}

		public string ValidateHCN(string hcn)
		{
			SendInfo(hcn);
			return ReceiveResponse();
		}

		private void SendInfo(string info)
		{
			ServerConn.Send(Encoding.ASCII.GetBytes(info));
		}

		private string ReceiveResponse()
		{
			string res = string.Empty;

			byte[] msgBuf = new byte[MAX_MSG_LEN];

			int msgSize = ServerConn.Receive(msgBuf);

			res = Encoding.ASCII.GetString(msgBuf, 0, msgSize);

			return res;
		}

		// Aquired from https://stackoverflow.com/questions/6803073/get-local-ip-address
		private static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}
	}
}
