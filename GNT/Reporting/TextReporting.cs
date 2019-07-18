using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Reporting {

	class TextWriterShop : AbsWriterShop {

		public TextWriterShop() { }

		/// <summary>
		/// Creates a report which writes the data to a text file.
		/// </summary>
		/// <param name="details">The full path to the file, including name.</param>
		/// <returns>The reporter object.</returns>
		public override AbsReportWriter CreateReporter(string details) => new TextReporter(details);
	}

	/// <summary>
	/// Manages a text report file.
	/// </summary>
	class TextReporter : AbsReportWriter {
		private string fp;
		
		/// <summary>
		/// Creates the object, and if the file doesn't exist creates it as well.  If the file does exist it is rolled.
		/// </summary>
		/// <param name="path">Full or relative path, including the filename, of the report.</param>
		public TextReporter(string path) {
			fp = path;
		}

		/// <summary>
		/// Either re-names the file or deletes it and starts a new one.
		/// </summary>
		/// <param name="roll">If this is true the previous data will be saved by re-naming the file with the created date/time stamp.</param>
		/// <returns>True if the operation succeeded.</returns>
		public override bool Restart(bool roll) {
			if (roll) {
				System.Console.WriteLine("TextReporter: restarting the file - rolling");
			} else {
				System.Console.WriteLine("TextReporter: restarting the file - not rolling");
			}
			return true;
		}

		/// <summary>
		/// Write a new line to the file.
		/// </summary>
		/// <param name="title">The test case title.</param>
		/// <param name="testId">The test id.</param>
		/// <param name="status">The current test execution status.</param>
		/// <param name="message">Any extra text which should be recorded.</param>
		/// <returns>True if the write succeeded.</returns>
		public override bool Update(string title, int testId, TestStatus status, string message) {
			string sample = "";

			//build the message to write to file
			/* In these messages:
			 *	0 = test case id
			 *	1 = test case title
			 *	2 = the message
			 */
			switch(status) {
				case TestStatus.Failed:
					sample = "{0}: {1} - Test Case Failed ({2})";
					break;
				case TestStatus.Stopped:
					sample = "{0}: {1} - Test Case Execution Halted ({2})";
					break;
				case TestStatus.Running:
					sample = "{0}: {1} - {2}";
					break;
				case TestStatus.Passed:
					sample = "{0}: {1} - Test Case Passed ({2})";
					break;
				default:
					break;
			}

			try {
				using (StreamWriter printer = new StreamWriter(fp, true)) {
					try {
						//write the line
						printer.WriteLine(String.Format(sample, new object[] { testId, title, message }));
					}
					#region Exception Handling
					catch (ObjectDisposedException ode) {
						System.Console.WriteLine("TextReporter.Update writer closed before update: " + ode.ToString());
						return false;
					}
					catch (IOException ioe) {
						System.Console.WriteLine("TextReporter.Update Error when writing: " + ioe.ToString());
						return false;
					}
					#endregion

					//safety
					try {
						printer.Flush();
					}
					#region Exception Handling
					catch (ObjectDisposedException ode) {
						System.Console.WriteLine("TextReporter.Update writer closed before flush: " + ode.ToString());
					}
					catch(IOException ioe) {
						System.Console.WriteLine("TextReporter.Update general error when flushing: " + ioe.ToString());
					}
					catch(EncoderFallbackException efe) {
						System.Console.WriteLine("TextReporter.Update wtf encoding error: " + efe.ToString());
					}
					#endregion
					return true;
				} //end using printer
			} //end outer try
			#region Exception Handling
			catch (Exception e) {
				System.Console.WriteLine("TextReporter.Update error opening file: " + e.ToString());
				return false;
			}
			//TODO flush out the exception list here
			#endregion
		} //end Update
	} //end TextReporter
}
