using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint.ASTP {
	class ASTPClient : AbsClientEndpt {
		private AbsASTPCypher cypher;

		public ASTPClient() {
			cypher = null;
		}

		#region AbsClientEndpt
		/// <summary>
		/// Builds and sends a message without waiting on a response.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <returns></returns>
		public override bool SendAlert(string msg) {
			System.Console.WriteLine("\t\tASTPClient is Sending Alert " + msg);
			return true;
		}

		/// <summary>
		/// Builds and sends a message and then waits on the response.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <returns>The response or an empty string if there was none.</returns>
		public override string SendMsg(string msg) {
			//TODO with the changes in conversation make this actually send and receive
			ASTPConversation convo = new ASTPConversation(cypher, 0);
			System.Console.WriteLine("\t\tASTPClient.SendMsg: Sending Message " + msg);
			convo.AddMessage(msg);
			convo.ProcessMsg(null, null);  //simulate the processing thread
			string test = string.Empty;
			convo.GetResponse(out test);  //making this call sets test
			return test;
		}
		#endregion

		public AbsASTPCypher Cypher { get => cypher; set => cypher = value; }
	}
}
