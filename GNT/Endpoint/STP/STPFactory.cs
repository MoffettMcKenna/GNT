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
		/// Creates a client who will use the default interface.
		/// </summary>
		/// <returns>The client object.</returns>
		public override AbsClientEndpt CreateClient() {
			return new STPClient();
		}

		/// <summary>
		/// Creates a cliente.  The type is ignored for STP Clients.
		/// </summary>
		/// <param name="type">Ignored</param>
		/// <returns>The client object.</returns>
		public override AbsClientEndpt CreateClient(string type) {
			return new STPClient();
		}

		/// <summary>
		/// Creates a server who will use the default interface.
		/// </summary>
		/// <returns>The server object.</returns>
		public override AbsServerEndpt CreateServer() {
			return new STPServer();
		}

		/// <summary>
		/// Creates a server.  For STP the type specification is ignored.
		/// </summary>
		/// <param name="type">This value is ignored for this protocol.</param>
		/// <returns>The server object.</returns>
		public override AbsServerEndpt CreateServer(string type) {
			return new STPServer();
		}

	}
}
