using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint.ASTP {
	class ASTPClient : AbsClientEndpt {
		private readonly string interf;
		private AbsASTPCypher cypher;

		public ASTPClient() {
			interf = "default";
			cypher = null;
		}

		public ASTPClient(string nic) {
			interf = nic;
			cypher = null;
		}

		#region AbsClientEndpt
		/// <summary>
		/// Builds and sends a message without waiting on a response.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <returns></returns>
		public override bool SendAlert(string msg) {
			System.Console.WriteLine("ASTPClient is using " + interf + " to send Alert " + msg);
			return true;
		}

		/// <summary>
		/// Builds and sends a message and then waits on the response.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <returns>The response.</returns>
		public override string SendMsg(string msg) {
			System.Console.WriteLine("ASTPClient is using " + interf + " to send Message " + msg);
			return msg;  //simulate echo
		}
		#endregion

		public AbsASTPCypher Cypher { get => cypher; set => cypher = value; }
	}
}
