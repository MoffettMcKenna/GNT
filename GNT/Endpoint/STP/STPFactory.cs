using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint.STP {
	class STPFactory : AbsEndptFactory {
		private static STPFactory instance = new STPFactory();	//minimal constructor, no performance issues in doing eager instantiation

		private STPFactory() { }

		public static STPFactory Instance { get { return instance; } }

		/// <summary>
		/// Creates a client.
		/// </summary>
		/// <returns>The client object.</returns>
		public override AbsClientEndpt CreateClient() {
			System.Console.WriteLine("\t\t\tSTPFactory.CreateClient: Creating a client with default options.");
			return new STPClient();
		}

		/// <summary>
		/// Creates a cliente.  The type is ignored for STP Clients.
		/// </summary>
		/// <param name="type">Ignored</param>
		/// <returns>The client object.</returns>
		public override AbsClientEndpt CreateClient(string type) {
			System.Console.WriteLine("\t\t\tSTPFactory.CreateClient: Creating a client with " + type + " option.");
			return new STPClient();
		}

		/// <summary>
		/// Creates a server.
		/// </summary>
		/// <returns>The server object.</returns>
		public override AbsServerEndpt CreateServer() {
			System.Console.WriteLine("\t\t\tSTPFactory.CreateServer: Creating a server with default options.");
			return new STPServer();
		}

		/// <summary>
		/// Creates a server.  For STP the type specification is ignored.
		/// </summary>
		/// <param name="type">This value is ignored for this protocol.</param>
		/// <returns>The server object.</returns>
		public override AbsServerEndpt CreateServer(string type) {
			System.Console.WriteLine("\t\t\tSTPFactory.CreateServer: Creating a server with " + type + " option.");
			return new STPServer();
		}

	}
}
