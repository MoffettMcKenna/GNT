using GNT.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT {
	class Program {
		static void Main(string[] args) {
			//sanity check
			if (args.Length == 0) {
				System.Console.WriteLine("Re-run with config as argument.");
			}
			else if(args.Length > 1) {
				System.Console.WriteLine("Only one config file at a time is supported for now.");
			}
			else {
				System.Console.WriteLine("Processing Config file " + args[0]);
				Conductor con = new Conductor(args[0]);
				AbsEngine[] engs = con.BuildEngines();

				System.Console.WriteLine("Read out " + engs.Length + " engines.");

				//keep it simple for now - a GUI would allow the user to select which to run
				foreach (AbsEngine e in engs) {
					System.Console.WriteLine("Running all test cases with engine " + e.Nickname);
					e.RunAll();
				}
			}

			//pause
			System.Console.WriteLine("Hit a key to exit...");
			System.Console.ReadKey();
		} //end main()
	} //end class Program
} //end namespace GNT
