using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint {
	abstract class AbsServerEndpt {
		/// <summary>
		/// Start listening for an incoming connection.  Implementing classes will need to determine how to manage connections.
		/// </summary>
		public abstract void Listen();
		/// <summary>
		/// Halts the server, cleaning up any connections.
		/// </summary>
		public abstract void Stop();
	}
}
