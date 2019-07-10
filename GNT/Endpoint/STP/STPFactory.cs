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
		/// Creates a client using the specified interface.
		/// </summary>
		/// <param name="nic">The interface the socket will use.</param>
		/// <returns>The client object.</returns>
		public override AbsClientEndpt CreateClient(string nic) {
			return new STPClient(nic);
		}

		/// <summary>
		/// Creates a server who will use the default interface.
		/// </summary>
		/// <returns>The server object.</returns>
		public override AbsServerEndpt CreateServer() {
			return new STPServer();
		}

		/// <summary>
		/// Creates a server using the specified interface.
		/// </summary>
		/// <param name="nic">The interface the socket will use.</param>
		/// <returns>The server object.</returns>
		public override AbsServerEndpt CreateServer(string nic) {
			return new STPServer(nic);
		}
	}
}
