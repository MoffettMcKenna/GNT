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
		public override AbsReportWriter CreateReporter(string details) {
			try {
				FileInfo fi = new FileInfo(details);
				if (Directory.Exists(fi.Directory.FullName)) return new CSVReporter(details);
				else throw new ArgumentException("Directory " + fi.Directory.FullName + " does not exist");
			}
			catch (Exception e) {
				throw new ArgumentException("Error on checking the details in CSVWriterShop.", e);
			}
		} //end CreateReporter
	}

	/// <summary>
	/// Manages a csv report file.
	/// </summary>
	class CSVReporter : AbsReportWriter {
		private string fp;
		private const int MAX_FSIZE = 51200; //50 KB max file size
		private const string DT_FORMAT = "ddMMYY.HHmmss";

		/// <summary>
		/// Creates the object, and if the file doesn't exist creates it as well.  If the file does exist it is rolled.
		/// </summary>
		/// <param name="path">Full or relative path, including the filename, of the report.</param>
		public CSVReporter(string path) {
			System.Console.WriteLine("\t\t\tCreating a CSVReporter.");
			fp = path;
		}

		/// <summary>
		/// Either re-names the file or deletes it and starts a new one.
		/// </summary>
		/// <param name="roll">If this is true the previous data will be saved by re-naming the file with the created date/time stamp.</param>
		/// <returns>True if the operation succeeded.</returns>
		public override bool Restart(bool roll) {
			if (roll) {
				string ext = "";
				try {
					ext = DateTime.Now.ToString(DT_FORMAT);
				}
				#region Exception Handling
				catch (FormatException fe) {
					System.Console.WriteLine("CSVReporter.Restart Error in time format string: " + fe.Message);
					return false;
				}
				catch (ArgumentOutOfRangeException aooe) {
					System.Console.WriteLine("CSVReporter.Restart Date outside calendar range: " + aooe.Message);
					return false;
				}
				#endregion
				try {
					File.Move(fp, fp + ext);
					return true;
				}
				#region Exception Handling
				//TODO maybe handle some of these better
				catch (ArgumentNullException ane) {
					System.Console.WriteLine("CSVReporter.Restart Error on Move: " + ane.Message);
				}
				catch (ArgumentException ae) {
					System.Console.WriteLine("CSVReporter.Restart Error on Move: " + ae.Message);
				}
				catch (UnauthorizedAccessException uae) {
					System.Console.WriteLine("CSVReporter.Restart Error on Move: " + uae.Message);
				}
				catch (PathTooLongException ptle) {
					System.Console.WriteLine("CSVReporter.Restart Error on Move: " + ptle.Message);
				}
				catch (DirectoryNotFoundException dnfe) {
					System.Console.WriteLine("CSVReporter.Restart Error on Move: " + dnfe.Message);
				}
				catch (NotSupportedException nse) {
					System.Console.WriteLine("CSVReporter.Restart Error on Move: " + nse.Message);
				}
				catch (IOException ioe) {
					System.Console.WriteLine("CSVReporter.Restart Error on Move: " + ioe.Message);
				}
				#endregion

			} else {
				//just delete it and let the next Update() call recreate it
				try {
					File.Delete(fp);
				}
				#region Exception Handling
				//TODO maybe handle some of these better
				catch (ArgumentNullException ane) {
					System.Console.WriteLine("CSVReporter.Restart Error on Delete: " + ane.Message);
				}
				catch (ArgumentException ae) {
					System.Console.WriteLine("CSVReporter.Restart Error on Delete: " + ae.Message);
				}
				catch (UnauthorizedAccessException uae) {
					System.Console.WriteLine("CSVReporter.Restart Error on Delete: " + uae.Message);
				}
				catch (PathTooLongException ptle) {
					System.Console.WriteLine("CSVReporter.Restart Error on Delete: " + ptle.Message);
				}
				catch (DirectoryNotFoundException dnfe) {
					System.Console.WriteLine("CSVReporter.Restart Error on Delete: " + dnfe.Message);
				}
				catch (NotSupportedException nse) {
					System.Console.WriteLine("CSVReporter.Restart Error on Delete: " + nse.Message);
				}
				catch (IOException ioe) {
					System.Console.WriteLine("CSVReporter.Restart Error on Delete: " + ioe.Message);
				}
				#endregion
			} //end else
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
						printer.WriteLine(
							String.Format("{0},{1},{2},{3},{4},{5}", 
										new object[] { DateTime.Now.Date, DateTime.Now.ToShortTimeString(), testId, title, status, message })
						);
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
				} //end using printer

				//if the file is now larger than 
				if ((new FileInfo(fp)).Length > MAX_FSIZE) {
					this.Restart(true);
				}

				return true;
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
