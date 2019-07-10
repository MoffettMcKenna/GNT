using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint.STP {
	class STPClient : AbsClientEndpt {
		private readonly string interf;

		public STPClient() {
			interf = "default";
		}

		public STPClient(string nic) {
			interf = nic;
		}

		#region AbsClientEndpt
		/// <summary>
		/// Builds and sends a message without waiting on a response.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <returns></returns>
		public override bool SendAlert(string msg) {
			System.Console.WriteLine("STPClient is using " + interf + " to send Alert " + msg);
			return true;
		}

		/// <summary>
		/// Builds and sends a message and then waits on the response.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <returns>The response.</returns>
		public override string SendMsg(string msg) {
			System.Console.WriteLine("STPClient is using " + interf + " to send Message " + msg);
			return msg;  //simulate echo
		}
		#endregion
	}
}
