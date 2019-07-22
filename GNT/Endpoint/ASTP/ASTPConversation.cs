using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Concurrent;

namespace GNT.Endpoint.ASTP {
	class ASTPConversation {
		private AbsASTPCypher cypher;
		private int port;  //TODO create a socket and start listening and sending on it instead of saving it
		private ConcurrentQueue<string> messages;
		private ConcurrentQueue<string> responses;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="code">The encryption for messages.</param>
		/// <param name="p">The port to use with the socket.</param>
		public ASTPConversation(AbsASTPCypher code, int p) {
			this.cypher = code;
			this.messages = new ConcurrentQueue<string>();
			this.responses = new ConcurrentQueue<string>();
			port = p;
		}

		/// <summary>
		/// This will be the DoWork handler of a background thread when the network traffic is actually implemented.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ProcessMsg(object sender, DoWorkEventArgs e) {
			//TODO fix this to actually maange listening for messages and sending responses - involves a couple of layers of callbacks and such processing

			//TEMPORARY - for now pretend we got an echo response
			string data = String.Empty;
			while(messages.TryDequeue(out data)) {
				//encrypt and shove onto responses
				string enc = cypher.Encrpyt(data);
				System.Console.WriteLine("\t\t" + data + " encrypted to " + enc);
				responses.Enqueue(data); 
				/* Normally we would be decrypting someone else's message, but since we're not yet
				 * there and ASTP is not reflective, we can't decrypt it.
				 */
			}
		}

		/// <summary>
		/// Adds a new message onto the outgoing Q.  When processed it will be encrypted and sent.
		/// </summary>
		/// <param name="msg">The message to add.</param>
		public void AddMessage(string msg) {
			messages.Enqueue(msg);
		}
		/// <summary>
		/// Returns the latest response.
		/// </summary>
		/// <param name="rsp">The response.  If there was none available this was the empty string.</param>
		/// <returns>True if there was a response, else false.</returns>
		public bool GetResponse(out string rsp) {
			rsp = string.Empty;
			return responses.TryDequeue(out rsp);
		}

	}
}
