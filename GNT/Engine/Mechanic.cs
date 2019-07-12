using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNT.Reporting;
using System.Reflection;

namespace GNT.Engine {
	/// <summary>
	/// 
	/// </summary>
	abstract class AbsMechanic {
		protected AbsEngine eng;
		protected bool done;
		private const char DELIM = ':';

		/// <summary>
		/// True if the engine is in a state to execute tests.
		/// </summary>
		public bool Complete {  get { return done; } }
		/// <summary>
		/// The engine which was being built.  It should not be retreived until Complete is true - while Complete is false this will return null;
		/// </summary>
		public AbsEngine Engine {  get { return ( done ? eng : null); } }

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
				ConstructorInfo con = t.GetConstructor(new Type[] { });
				AbsWriterShop shop = (con.Invoke(null) as AbsWriterShop);

				//add a reporter to the engine
				eng.AddReporter(shop.CreateReporter(parts[1]));
			}
		} //end Reporter Property

	} //end class AbsMechanic

	/// <summary>
	/// Builds a RawTextEngine.
	/// </summary>
	class RawTextMechanic : AbsMechanic {

		public RawTextMechanic() {
			eng = new RawTextEngine();
		}

		/// <summary>
		/// Add a new folder to the list of the source directories.
		/// </summary>
		public string Folder { set { (eng as RawTextEngine).AddFolder(value); } }

		}
	} //end class RawTextMechanic
} //end namespace GNT.Engine
