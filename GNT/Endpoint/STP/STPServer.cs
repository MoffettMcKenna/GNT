using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNT.Endpoint;

namespace GNT.Endpoint.STP {
	class STPServer : AbsServerEndpt {
		private readonly string interf;

		public STPServer() {
			interf = "default";
		}

		public STPServer(string nic) {
			interf = nic;
		}

		#region AbsServerEndpt
		/// <summary>
		/// Start listening for an incoming connection.  Implementing classes will need to determine how to manage connections.
		/// </summary>
		public override void Listen() {
			System.Console.WriteLine("STPServer is listening for incoming connections on interface " + interf + ".");
		}

		/// <summary>
		/// Halts the server, cleaning up any connections.
		/// </summary>
		public override void Stop() {
			System.Console.WriteLine("STPServer is stopping.");
		}
		#endregion
	}
}
