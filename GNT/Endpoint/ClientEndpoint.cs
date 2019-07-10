using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint {
	abstract class AbsClientEndpt {
		/// <summary>
		/// Builds and sends a message and then waits on the response.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <returns>The response.</returns>
		public abstract string SendMsg(string msg); //TODO add exceptions to the signature?
		/// <summary>
		/// Builds and sends a message without waiting on a response.
		/// </summary>
		/// <param name="msg">The message to send.</param>
		/// <returns></returns>
		public abstract bool SendAlert(string msg); //TODO add exceptions to the signature?
	}
}
