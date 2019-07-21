using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;

namespace GNT.Engine {
	class Conductor {
		private string cfgfile;
		private const string SRC = "Source";
		List<AbsEngine> motors;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="file"></param>
		public Conductor(string file) {
			cfgfile = file;
			motors = new List<AbsEngine>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public void BuildEngines() {
			//using the List<KeyValuePair> structure allows me to have duplicate keys - multiple folders, reporters, etc..
			Dictionary<string, List<KeyValuePair<string, string>>> sections;
			
			//translate the file into a multi-layer 
			CfgProcessor processor = new CfgProcessor();
			sections = processor.Process(cfgfile);

			foreach(string sec in sections.Keys) {
				System.Console.WriteLine("Processing Section " + sec);
				string src = "";

				//Sanity check for Source entry
				try {
					src = sections[sec].Single(kvp => kvp.Key == SRC).Value;
					if (src.Length == 0) {
						System.Console.WriteLine("The Source entry is empty.");
						continue;
					}
				}
				#region Exception Handling
				catch (ArgumentNullException ane) {
					System.Console.WriteLine("Section " + sec + " does not have an entry.\n" + ane.Message);
					//skip me
					continue;
				}
				catch (InvalidOperationException ioe) {
					System.Console.WriteLine("Section " + sec + " does not have a Source.\n" + ioe.Message);
					//skip me
					continue;
				}
				#endregion

				//sanity check for Protocol
				try {
					if (sections[sec].Single(kvp => kvp.Key == "Protocol").Value.Length == 0) throw new Exception("Section " + sec + " does not have a Source");
				}
				#region Exception Handling
				catch (ArgumentNullException ane) {
					System.Console.WriteLine("Section " + sec + " does not have an entry.\n" + ane.Message);
					//skip me
					continue;
				}
				catch (InvalidOperationException ioe) {
					System.Console.WriteLine("Section " + sec + " does not have a Protocol.\n" + ioe.Message);
					//skip me
					continue;
				}
				#endregion

				//make the type
				string mechType = "GNT.Engine." + src + "Mechanic";
				System.Console.WriteLine("\tBuilding Mechanic " + mechType);

				//convert the string to a mechanic
				Type t = Type.GetType(mechType);
				AbsMechanic mech = (Activator.CreateInstance(t) as AbsMechanic);

				//set the nickname to the name of the section
				mech.NickName = sec;

				//now add all the values
				foreach(KeyValuePair<string, string> pair in sections[sec]) {
					//skip source - this was used to create the mechanic
					if (pair.Key.CompareTo(SRC) == 0) continue;

					//use reflection to set the property
					try {
						PropertyInfo prop = mech.GetType().GetProperty(pair.Key);
						prop.SetValue(mech, pair.Value);
					}
					#region Exception Handling
					catch (AmbiguousMatchException ame) {
						System.Console.WriteLine("Conductor.BuildEngines - Trouble matching " + pair.Key + " Property in mechanic: " + ame.Message);
						continue;
					}
					catch(TargetException te) {
						System.Console.WriteLine("Conductor.BuildEngines - There was an issue invoking the setter on the property: " + te.Message);
						continue;
					}
					catch (TargetInvocationException tie) {
						System.Console.WriteLine("Conductor.BuildEngines - There was an issue invoking the setter on the property: " + tie.Message);
						continue;
					}
					catch (MethodAccessException mae) {
						System.Console.WriteLine("Conductor.BuildEngines - There was an issue invoking the setter on the property: " + mae.Message);
						continue;
					}
					#endregion
				}

				if (mech.Complete) motors.Add(mech.Engine);
				else System.Console.WriteLine("Engine was incomplete and not added.");
			} //end foreach section

		}

		/// <summary>
		/// 
		/// </summary>
		public string Cfgfile { get => cfgfile; set => cfgfile = value; }

		public AbsEngine[] Engines {  get { return motors.ToArray(); } }
	} //end class Conductor

	/// <summary>
	/// Converts a config file into a nested dictionary structure.
	/// </summary>
	public class CfgProcessor {
		private Dictionary<string, List<KeyValuePair<string, string>>> data;
		private const string HDR_REG = @"^\[[a-zA-Z0-9\s]*\]";
		private Regex hdr_rx;

		/// <summary>
		/// Creates a config file processor.
		/// </summary>
		public CfgProcessor() {
			hdr_rx = new Regex(HDR_REG, RegexOptions.Multiline);
		}

		/// <summary>
		/// Converts the file given into a data structure which can be used.
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		public Dictionary<string, List<KeyValuePair<string, string>>> Process(string file) {
			//re-init data to clear any previos runs
			data = new Dictionary<string, List<KeyValuePair<string, string>>>();

			using (StreamReader reader = new StreamReader(file)) {
				//start with a section called general for system configurations
				processSection("General", reader);
			}

			return data;
		}

		/* Tests a line for a section header.  If there is one it is returned in the name param and the function returns true.
		 */
		private bool checkForSection(string line, out string name) {
			//test for actually being a match to the regex for section headers
			if (hdr_rx.IsMatch(line)) {

				int stdex = line.IndexOf('[');
				int endex = line.IndexOf(']');
				
				if(stdex < 0 || endex < 0) {
					//missing a brace, not a section 
					name = string.Empty;
					return false;
				}
				else {
					//get the name and return true
					name = line.Substring(stdex + 1, endex - stdex - 1);
					return true;
				}
			} 

			//not a section, fail
			name = string.Empty;
			return false;
		}

		/* Processes a section from the config file.  It assumes the section header has been read and
		 * that the name of the section is 
		 */
		private void processSection(string name, StreamReader reader) {
			string line = "";
			string secName;
			List < KeyValuePair<string, string>> section = new List<KeyValuePair<string, string>>(); //hold the fields for this section

			//process the file - if there is another section we will abort and call another
			while ((line = reader.ReadLine()) != null) {
				//System.Console.WriteLine("Read line " + line);
				if(checkForSection(line, out secName)) {
					//System.Console.WriteLine("\tLine " + line + " is new section header " + secName);
					processSection(secName, reader);
					//on return we should have read any further sections and hit the EoF
					break;
				}
				//if there's an equal sign this is most likely a field
				else if (line.Contains("=")) {
					//System.Console.WriteLine("\tProcessing line as field");
					int lb = line.IndexOf('#');
					int sc = line.IndexOf(';');

					//get rid of the comments
					if (lb >= 0 || sc >= 0) line = line.Substring(0, (lb > sc ? sc : lb));

					//divide into key and value
					string[] parts = line.Split(new char[] { '=' });
					if (parts.Length > 2) throw new InvalidDataException("CfgProcessor.processSection - too many ='s in line " + line);

					//return the key-value pair
					section.Add(
						new KeyValuePair<string, string>(parts[0], (parts.Length == 2 ? parts[1] : string.Empty))
					);
				}
				//no need for other else's - the rest will be comments and empty lines

			} //end while line = readline

			if (section.Count > 0) data.Add(name, section); //skip any sections which were empty
		} //end processSection

	} //end class CfgProcessor


}
