using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint.ASTP {

	class ASTPFactory : AbsEndptFactory {
		private static ASTPFactory instance = new ASTPFactory();   //minimal constructor, no performance issues in doing eager instantiation
		private readonly LeftShiftCypher left;
		private readonly RightShiftCypher right;

		private ASTPFactory() {
			left = new LeftShiftCypher();
			right = new RightShiftCypher();
		}

		public static ASTPFactory Instance { get { return instance; } }

		/// <summary>
		/// Creates a client who will use the default interface.  It will default to the left shift cypher.
		/// </summary>
		/// <returns>The client object.</returns>
		public override AbsClientEndpt CreateClient() {
			ASTPClient client = new ASTPClient {
				Cypher = left.Clone()
			};
			return client;
		}

		/// <summary>
		/// Creates a client using the specified interface.  It will default to the left shift cypher.
		/// </summary>
		/// <param name="nic">The interface the socket will use.</param>
		/// <returns>The client object.</returns>
		public override AbsClientEndpt CreateClient(string nic) {
			ASTPClient client = new ASTPClient(nic) {
				Cypher = left.Clone()
			};
			return client;
		}

		/// <summary>
		/// Creates a client using the specified interface and the type variation.
		/// </summary>
		/// <param name="nic">The interface the socket will use.  If this is the empty string it is ignored.</param>
		/// <param name="type">Specifies the encryption algorithm, either right or left.  It is case insensitive.</param>
		/// <returns>The client object.</returns>
		public override AbsClientEndpt CreateClient(string nic, string type) {
			ASTPClient client;

			//determine if we need this argument - ignore on string.empty
			if (nic.CompareTo(String.Empty) != 0) client = new ASTPClient(nic);
			else client = new ASTPClient();

			//find the right encryption algorithm
			if (type.ToLower().CompareTo("right") == 0) client.Cypher = right.Clone();
			else client.Cypher = left.Clone();  //as above it defaults to left shift

			//all built, return it
			return client;
		}

		/// <summary>
		/// Creates a server who will use the default interface.
		/// </summary>
		/// <returns>The server object.</returns>
		public override AbsServerEndpt CreateServer() {
			ASTPServer server = new ASTPServer {
				Left = (left.Clone() as LeftShiftCypher),
				Right = (right.Clone() as RightShiftCypher)
			};
			return server;
		}

		/// <summary>
		/// Creates a server using the specified interface.
		/// </summary>
		/// <param name="nic">The interface the socket will use.</param>
		/// <returns>The server object.</returns>
		public override AbsServerEndpt CreateServer(string nic) {
			ASTPServer server = new ASTPServer(nic) {
				Left = (left.Clone() as LeftShiftCypher),
				Right = (right.Clone() as RightShiftCypher)
			};
			return server;
		}
	}
}
