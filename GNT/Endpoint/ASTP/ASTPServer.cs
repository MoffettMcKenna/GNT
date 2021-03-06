﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Endpoint.ASTP {
	class ASTPServer : AbsServerEndpt {
		private LeftShiftCypher left;
		private RightShiftCypher right;

		public ASTPServer() { }

		#region AbsServerEndpt
		/// <summary>
		/// Start listening for an incoming connection.  Implementing classes will need to determine how to manage connections.
		/// </summary>
		public override void Listen() {
			System.Console.WriteLine("\t\tASTPServer is listening for incoming connections on interface.");
			System.Console.WriteLine("\t\tOnce a message is received to start an ASPConversation a port is selected, the conversation created (including cloning the readonly left/rigth object), and a backgroundworker spawned for the conversation.");
		}

		/// <summary>
		/// Halts the server, cleaning up any connections.
		/// </summary>
		public override void Stop() {
			System.Console.WriteLine("\t\tASTPServer is stopping.");
		}
		#endregion

		public LeftShiftCypher Left { get => left; set => left = value; }
		public RightShiftCypher Right { get => right; set => right = value; }
	}
}
