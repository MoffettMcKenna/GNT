using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GNT.Endpoint.ASTP {
	class ASTPConversation {
		private AbsASTPCypher cypher;
		private int port;  //TODO create a socket and start listening on it instead of saving it
		private List<string> messages;
		private List<string> responses;

		public ASTPConversation(AbsASTPCypher code, int p) {
			this.cypher = code;
			this.messages = new List<string>();
			this.responses = new List<string>();
			port = p;
		}

		/// <summary>
		/// This will be the DoWork handler of a background thread when the network traffic is actually implemented.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ProcessMsg(object sender, DoWorkEventArgs e) {
			return;
		}
	}
}
