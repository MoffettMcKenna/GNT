using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNT.Reporting;
using GNT.Endpoint;
using System.Reflection;

namespace GNT.Engine {
	/// <summary>
	/// 
	/// </summary>
	abstract class AbsMechanic {
		protected AbsEngine eng;
		private const char DELIM = ':';

		//The required information which MUST be set at least once for the engine to run correctly
		//while these are technically a bitmask, we can be lazy and use addition
		protected const byte SERVER_SET = 1;
		protected const byte CLIENT_SET = 2;
		protected const byte REPORT_SET = 4;
		protected const byte SOURCE_SET = 8; //test script locations
		protected const byte READY = SERVER_SET + CLIENT_SET + REPORT_SET + SOURCE_SET;  //protected for over-rides, in case an engine has an extra requriement

		//when this is the sum of the constants above, we're done (at least one of everythign is set)
		protected byte done = 0;

		protected string proto = string.Empty;	//protocol to use
		protected string clientType = string.Empty; //any special information for the client
		protected string serverType = String.Empty;

		/// <summary>
		/// 
		/// </summary>
		public bool Complete { get { return (done >= READY); } }

		/// <summary>
		/// The engine which was being built.  It should not be retreived until Complete is true - while Complete is false this will return null;
		/// </summary>
		public AbsEngine Engine {  get { return (done >= READY ? eng : null); } }

		/// <summary>
		/// Gets a reporter of the designated type through shop factory method.
		/// </summary>
		public string Reporter {
			set {
				//split into the type and details
				string[] parts = value.Split(new char[] { DELIM });

				//if too many details error - bad file
				if (parts.Length > 2) throw new ArgumentException("AbsMechanic.Reporter - Extra deliminators in Reporter Value " + value);

				//build the full type string
				string shopType = "GNT.Reporting." + parts[0] + "WriterShop";

				//get the constructor data and invoke it
				Type t = Type.GetType(shopType);
				AbsWriterShop shop = (Activator.CreateInstance(t) as AbsWriterShop);

				//add a reporter to the engine
				eng.AddReporter(shop.CreateReporter(parts[1]));

				//update the completion
				done += REPORT_SET;
			}
		} //end Reporter Property

		/// <summary>
		/// Gets a client of the correct type.  This is not necessary as a base server is created when
		/// the protocol is set, but some types might need to specify further details.
		/// </summary>
		public string Client {
			set {
				//with those set we are ready to make the client if the protocol is set
				if (!isEmpty(proto)) makeClient(); 
			}
		} //end Client Property

		/* Extracted so it can be called from either Client or 
		 * Prototype based on what is called first.
		 */
		protected void makeClient() {
			//build the full type string
			string factType = "GNT.Endpoint." + proto + "." + proto + "Factory";

			//get the type
			Type t = Type.GetType(factType);

			//get the Instance property and invoke it
			PropertyInfo inst = t.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
			MethodInfo getter = inst.GetMethod;
			AbsEndptFactory fact = (getter.Invoke(null, null) as AbsEndptFactory);

			//create and save the client
			AbsClientEndpt client = null;
			if (!isEmpty(clientType)) client = fact.CreateClient(clientType);
			else client = fact.CreateClient();
			eng.Client = client;

			//mark the client as available
			done += CLIENT_SET;
		} //end makeClient

		/// <summary>
		/// Gets a server of the correct type.  This is not necessary as a base server is created when
		/// the protocol is set, but some types might need to specify further details.
		/// </summary>
		public string Server {
			set {
				serverType = value;
				if (!isEmpty(proto)) makeServer();
			}
		} //end Server Property

		/* Extracted so it can be called from either Server or 
		 * Prototype based on what is called first.
		 */
		protected void makeServer() {
			//build the full type string
			string factType = "GNT.Endpoint." + proto + "." + proto + "Factory";

			//get the type
			Type t = Type.GetType(factType);

			//get the Instance property and invoke it
			PropertyInfo inst = t.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
			MethodInfo getter = inst.GetMethod;
			AbsEndptFactory fact = (getter.Invoke(null, null) as AbsEndptFactory);

			//create and save the server
			AbsServerEndpt srvr = null;
			if (!isEmpty(serverType)) srvr = fact.CreateServer(serverType);
			else srvr = fact.CreateServer();
			eng.Server = srvr;

			//mark the server as available
			done += SERVER_SET;
		} //end makeServer

		/// <summary>
		/// Set the protocol used, which also triggers the creaton of the base client and server if not already made.
		/// </summary>
		public string Protocol {
			set {
				//save the protocol to use
				proto = value;

				//create a server once we know the protocol
				if((done & SERVER_SET) == 0) makeServer();
				//if the client info has already been set make it
				if ((done & CLIENT_SET) == 0) makeClient();
			}
		}

		/// <summary>
		/// Set the nickname for the engine.
		/// </summary>
		public string NickName {
			set {
				eng.Nickname = value;
			}
		}

		private bool isEmpty(string check) {
			return (check.Length == 0);
		}

	} //end class AbsMechanic

	/// <summary>
	/// Builds a RawTextEngine.
	/// </summary>
	class RawTextMechanic : AbsMechanic {

		public RawTextMechanic() {
			System.Console.WriteLine("\tCreating RawTextmechanic to build a RawTextEngine.");
			eng = new RawTextEngine();
		}

		/// <summary>
		/// Add a new folder to the list of the source directories.
		/// </summary>
		public string Folder {
			set {
				(eng as RawTextEngine).AddFolder(value);
				done += SOURCE_SET;
			}
		}

	} //end class RawTextMechanic
} //end namespace GNT.Engine
