using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Reporting {

	class CSVWriterShop : AbsWriterShop {

		public CSVWriterShop() { }

		/// <summary>
		/// Creates a report which writes the data to a text file.
		/// </summary>
		/// <param name="details">The full path to the file, including name.</param>
		/// <returns>The reporter object.</returns>
		public override AbsReportWriter CreateReporter(string details) => new CSVReporter(details);
	}

	/// <summary>
	/// Manages a csv report file.
	/// </summary>
	class CSVReporter : AbsReportWriter {
		private string fp;

		/// <summary>
		/// Creates the object, and if the file doesn't exist creates it as well.  If the file does exist it is rolled.
		/// </summary>
		/// <param name="path">Full or relative path, including the filename, of the report.</param>
		public CSVReporter(string path) {
			fp = path;
		}

		/// <summary>
		/// Either re-names the file or deletes it and starts a new one.
		/// </summary>
		/// <param name="roll">If this is true the previous data will be saved by re-naming the file with the created date/time stamp.</param>
		/// <returns>True if the operation succeeded.</returns>
		public override bool Restart(bool roll) {
			if (roll) {
				System.Console.WriteLine("CSVReporter: restarting the file - rolling");
			} else {
				System.Console.WriteLine("CSVReporter: restarting the file - not rolling");
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
			try {
				using (StreamWriter printer = new StreamWriter(fp, true)) {
					try {
						//write the line
						printer.WriteLine(String.Format("{0},{1},{2},{3}", new object[] { testId, title, status, message }));
					}
					#region Exception Handling
					catch (ObjectDisposedException ode) {
						System.Console.WriteLine("CSVReporter.Update writer closed before update: " + ode.ToString());
						return false;
					}
					catch (IOException ioe) {
						System.Console.WriteLine("CSVReporter.Update Error when writing: " + ioe.ToString());
						return false;
					}
					#endregion

					//safety
					try {
						printer.Flush();
					}
					#region Exception Handling
					catch (ObjectDisposedException ode) {
						System.Console.WriteLine("CSVReporter.Update writer closed before flush: " + ode.ToString());
					}
					catch (IOException ioe) {
						System.Console.WriteLine("CSVReporter.Update general error when flushing: " + ioe.ToString());
					}
					catch (EncoderFallbackException efe) {
						System.Console.WriteLine("CSVReporter.Update wtf encoding error: " + efe.ToString());
					}
					#endregion
					return true;
				} //end using printer
			} //end outer try
			#region Exception Handling
			catch (Exception e) {
				System.Console.WriteLine("CSVReporter.Update error opening file: " + e.ToString());
				return false;
			}
			#endregion
		} //end Update
	} //end CSVReporter
}
