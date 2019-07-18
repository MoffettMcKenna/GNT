using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GNT.Reporting;
using GNT.Endpoint;
using System.IO;

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
	/// A file with test cases consisting only of the message to send and the expected value back.  This engine will be used to create clients to test servers.
	/// Format:
	///	[test id] : [to send] : [expected response]
	/// It is up to the writer to ensure these 
	/// </summary>
	class RawTextEngine : AbsEngine {
		private List<string> folders;
		private Dictionary<int, string> tcLookup; //a lookup - each entry is the test case id and the line for the test case
		private const string EXT = ".txt";
		private const char DELIM = ':';

		//indices for the columns in the file - which parts map to what
		private const int ID_DEX = 0;
		private const int MSG_DEX = 1;
		private const int RSP_DEX = 2;
		private const int COL_CNT = 3; //number of columns to find post-split

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
			//break it into pieces
			string[] parts = tcLookup[id].Split(new char[] { DELIM });

			//check that we had exactly the correct number of fields
			if (parts.Length == COL_CNT) {
				//send the message
				string rsp = Client.SendMsg(parts[MSG_DEX]);

				//check the response
				if (rsp.CompareTo(parts[RSP_DEX]) == 0) {
					System.Console.WriteLine("RawTextEngine.Run - Test Case " + parts[ID_DEX] + " Passed.");
					reportPass(parts[MSG_DEX], id, rsp);
				} else {
					System.Console.WriteLine("RawTextEngine.Run - Test Case " + parts[ID_DEX] + " Failed.");
					reportFail(parts[MSG_DEX], id, rsp);
				}
			} else {
				System.Console.WriteLine("RawTextEngine.Run - " + tcLookup[id] + " is not formatted correctly");
			}
		}

		/// <summary>
		/// Runs all the test cases the engine has.
		/// </summary>
		public override void RunAll() {
			System.Console.WriteLine("RawTextEngine.RunAll - Running all tests.");
			//use Run() with the whole list
			foreach(int id in tcLookup.Keys)	Run(id);
		}

		/// <summary>
		/// Run several, but not all, test cases.
		/// </summary>
		/// <param name="ids">A list of the test case id's to run.</param>
		public override void RunMultiple(int[] ids) {
			System.Console.WriteLine("RawTextEngine.RunMultiple - Running these tests: " + string.Join(", ", ids));
			//use Run() with all the ids
			foreach (int id in ids) Run(id);
		}
		#endregion

		/// <summary>
		/// Adds a new folder to find test case files in.
		/// </summary>
		/// <param name="path">The full or relative path to find test case files in.</param>
		public void AddFolder(string path) {
			
			//test if this is a valid directory
			if(Directory.Exists(path)) {
				//get all the files in this folder and subfolders
				string[] files = Directory.GetFiles(path, EXT, SearchOption.AllDirectories);

				//process all the files
				foreach (string file in files) {

					//open the file for reading
					using (StreamReader reader = new StreamReader(file)) {
						string line = "";

						//read each line
						while((line = reader.ReadLine()) != null ) {

							//break the line up
							string[] parts = line.Split(new char[] { DELIM });

							//sanity check that the line was as expected
							if(parts.Length != COL_CNT) {
								System.Console.WriteLine("RawTextEngine.AddFolder - cannot parse line " + line);
								continue; //skip this running
							} 
							else {
								int id = 0;
								//if the id field can be parsed add it adn the line to the test case lookup
								if (int.TryParse(parts[ID_DEX], out id)) tcLookup.Add(id, line);
							}

						} //end while readLine()

					} //end using StreamReader

				} //end foreach file

			} //end if Directory.Exists()
			else {

			} //end else [Directory.Exists()]
		} //end AddFolder()

		/* Updates all the reports with the same data for a passed test case. */
		private void reportPass(string msg, int id, string rsp) {
			foreach(AbsReportWriter writer in this.Reporters) {
				writer.Update(msg, id, TestStatus.Passed, rsp);
			}
		} //end reportPass()

		/* Updates all the reports with the same data for a passed test case. */
		private void reportFail(string msg, int id, string rsp) {
			foreach (AbsReportWriter writer in this.Reporters) {
				writer.Update(msg, id, TestStatus.Failed, rsp);
			}
		} //end reportFail()

	} //end class RawTextEngine
} //end namespace GNT.Engine
