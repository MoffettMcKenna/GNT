using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint {
	abstract class AbsEndptFactory {
		/// <summary>
		/// Creates a client.
		/// </summary>
		/// <returns>The client object.</returns>
		public abstract AbsClientEndpt CreateClient();
		/// <summary>
		/// Creates a client using the specified type variation.
		/// </summary>
		/// <param name="type">Specification as to the type of the client which should be made.</param>
		/// <returns>The client object.</returns>
		public abstract AbsClientEndpt CreateClient(string type);
		/// <summary>
		/// Creates a server.
		/// </summary>
		/// <returns>The server object.</returns>
		public abstract AbsServerEndpt CreateServer();
		/// <summary>
		/// Creates a server using the specified type variation.
		/// </summary>
		/// <param name="type">Specification as to the type of the server which should be made.</param>
		/// <returns>The server object.</returns>
		public abstract AbsServerEndpt CreateServer(string type);
	}


}
