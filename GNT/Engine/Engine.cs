using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNT.Reporting;
using GNT.Endpoint;

namespace GNT.Engine {
	/// <summary>
	/// The abstract engine definition
	/// </summary>
	abstract class AbsEngine {
		private List<AbsReportWriter> reporters;
		private AbsClientEndpt client;
		private AbsServerEndpt server;
		private string nickname;

		/// <summary>
		/// Base constructor to initialize all structures.
		/// </summary>
		protected AbsEngine() {
			System.Console.WriteLine("Created new AbsEngine.");
			reporters = new List<AbsReportWriter>();
		}

		/// <summary>
		/// Adds a new reporter to the engine.
		/// </summary>
		/// <param name="writer">The report writer to add.</param>
		public void AddReporter(AbsReportWriter writer) {
			if (!reporters.Contains(writer)) reporters.Add(writer);
		}

		/// <summary>
		/// Adds a set of reporters to the engine.
		/// </summary>
		/// <param name="writers">The report writers to add.</param>
		public void AddReporters(AbsReportWriter[] writers) {
			foreach (AbsReportWriter w in writers) this.AddReporter(w);
		}

		/// <summary>
		/// Run a single test case.
		/// </summary>
		/// <param name="id">The id of the test case to run.</param>
		public abstract void Run(int id);
		/// <summary>
		/// Run several, but not all, test cases.
		/// </summary>
		/// <param name="ids">A list of the test case id's to run.</param>
		public abstract void RunMultiple(int[] ids);
		/// <summary>
		/// Runs all the test cases the engine has.
		/// </summary>
		public abstract void RunAll();

		/// <summary>
		/// The client endpoint for testing.
		/// </summary>
		internal AbsClientEndpt Client { get => client; set => client = value; }
		/// <summary>
		/// The server for testing.
		/// </summary>
		internal AbsServerEndpt Server { get => server; set => server = value; }
		/// <summary>
		/// All the reports which need to be generated.
		/// </summary>
		internal List<AbsReportWriter> Reporters { get => reporters; }

		/// <summary>
		/// A quick handle for the testing this engine does.
		/// </summary>
		public string Nickname { get => nickname; set => nickname = value; }
	}

	/// <summary>
	/// A file with test cases consisting only of the message to send and the expected value back.
	/// Format:
	///	[test id] : [to send] : [expected response]
	/// </summary>
	class RawTextEngine : AbsEngine {
		private List<string> folders;
		private Dictionary<int, string> tcLookup; //used to find the file containing a particular test case
		private const string EXT = ".txt";

		/// <summary>
		/// Initializes the data structures.
		/// </summary>
		public RawTextEngine() {
			folders = new List<string>();
			tcLookup = new Dictionary<int, string>();
			System.Console.WriteLine("I am a new RawTextEngine!");
		}

		#region AbsEngine
		/// <summary>
		/// Run a single test case.
		/// </summary>
		/// <param name="id">The id of the test case to run.</param>
		public override void Run(int id) {
			System.Console.WriteLine("RawTextEngine.Run - Running " + id);
		}

		/// <summary>
		/// Runs all the test cases the engine has.
		/// </summary>
		public override void RunAll() {
			System.Console.WriteLine("RawTextEngine.RunAll - Running all tests.");
		}

		/// <summary>
		/// Run several, but not all, test cases.
		/// </summary>
		/// <param name="ids">A list of the test case id's to run.</param>
		public override void RunMultiple(int[] ids) {
			System.Console.WriteLine("RawTextEngine.RunMultiple - Running these tests: " + string.Join(", ", ids));
		}
		#endregion

		/// <summary>
		/// Adds a new folder to find test case files in.
		/// </summary>
		/// <param name="path">The full or relative path to find test case files in.</param>
		public void AddFolder(string path) {
			folders.Add(path); //TODO validate this is a folder, derive the folder if not
						 
			//TODO read the test files and fill in the lookup
		}
	}
}
