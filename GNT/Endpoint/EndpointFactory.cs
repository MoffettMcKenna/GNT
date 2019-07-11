using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint {
	abstract class AbsEndptFactory {
		/// <summary>
		/// Creates a client who will use the default interface.
		/// </summary>
		/// <returns>The client object.</returns>
		public abstract AbsClientEndpt CreateClient();
		/// <summary>
		/// Creates a client using the specified interface.
		/// </summary>
		/// <param name="nic">The interface the socket will use.</param>
		/// <returns>The client object.</returns>
		public abstract AbsClientEndpt CreateClient(string nic);
		/// <summary>
		/// Creates a client using the specified interface and the type variation.
		/// </summary>
		/// <param name="nic">The interface the socket will use.</param>
		/// <param name="type">Specification as to the type of the client which should be made.</param>
		/// <returns>The client object.</returns>
		public abstract AbsClientEndpt CreateClient(string nic, string type);
		/// <summary>
		/// Creates a server who will use the default interface.
		/// </summary>
		/// <returns>The server object.</returns>
		public abstract AbsServerEndpt CreateServer();
		/// <summary>
		/// Creates a server using the specified interface.
		/// </summary>
		/// <param name="nic">The interface the socket will use.</param>
		/// <returns>The server object.</returns>
		public abstract AbsServerEndpt CreateServer(string nic);
	}


}
