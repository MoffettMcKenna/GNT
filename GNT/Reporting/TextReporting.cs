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
		public override AbsReportWriter CreateReporter(string details) {
			try {
				FileInfo fi = new FileInfo(details);
				if (Directory.Exists(fi.Directory.FullName)) return new TextReporter(details);
				else throw new ArgumentException("Directory " + fi.Directory.FullName + " does not exist");
				//TODO try and create it instead
			}
			catch (Exception e) {
				//TODO improve the exception handling
				throw new ArgumentException("Error on checking the details in TextWriterShop.", e);
			}
		} //end CreateReporter

	}

	/// <summary>
	/// Manages a text report file.
	/// </summary>
	class TextReporter : AbsReportWriter {
		private string fp;
		private const int MAX_FSIZE = 51200; //50 KB max file size
		private const string DT_FORMAT = "ddMMYY.HHmmss";

		/// <summary>
		/// Creates the object, and if the file doesn't exist creates it as well.  If the file does exist it is rolled.
		/// </summary>
		/// <param name="path">Full or relative path, including the filename, of the report.</param>
		public TextReporter(string path) {
			System.Console.WriteLine("\t\t\tCreating a TextReporter.");
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
					System.Console.WriteLine("TextReporter.Restart Error in time format string: " + fe.Message);
					return false;
				}
				catch (ArgumentOutOfRangeException aooe) {
					System.Console.WriteLine("TextReporter.Restart Date outside calendar range: " + aooe.Message);
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
					System.Console.WriteLine("TextReporter.Restart Error on Move: " + ane.Message);
				}
				catch (ArgumentException ae) {
					System.Console.WriteLine("TextReporter.Restart Error on Move: " + ae.Message);
				}
				catch (UnauthorizedAccessException uae) {
					System.Console.WriteLine("TextReporter.Restart Error on Move: " + uae.Message);
				}
				catch (PathTooLongException ptle) {
					System.Console.WriteLine("TextReporter.Restart Error on Move: " + ptle.Message);
				}
				catch (DirectoryNotFoundException dnfe) {
					System.Console.WriteLine("TextReporter.Restart Error on Move: " + dnfe.Message);
				}
				catch (NotSupportedException nse) {
					System.Console.WriteLine("TextReporter.Restart Error on Move: " + nse.Message);
				}
				catch (IOException ioe) {
					System.Console.WriteLine("TextReporter.Restart Error on Move: " + ioe.Message);
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
					System.Console.WriteLine("TextReporter.Restart Error on Delete: " + ane.Message);
				}
				catch (ArgumentException ae) {
					System.Console.WriteLine("TextReporter.Restart Error on Delete: " + ae.Message);
				}
				catch (UnauthorizedAccessException uae) {
					System.Console.WriteLine("TextReporter.Restart Error on Delete: " + uae.Message);
				}
				catch (PathTooLongException ptle) {
					System.Console.WriteLine("TextReporter.Restart Error on Delete: " + ptle.Message);
				}
				catch (DirectoryNotFoundException dnfe) {
					System.Console.WriteLine("TextReporter.Restart Error on Delete: " + dnfe.Message);
				}
				catch (NotSupportedException nse) {
					System.Console.WriteLine("TextReporter.Restart Error on Delete: " + nse.Message);
				}
				catch (IOException ioe) {
					System.Console.WriteLine("TextReporter.Restart Error on Delete: " + ioe.Message);
				}
				#endregion
			} //end else
			return true;
		} //end Restart()

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
			 * 0 = timestamp
			 *	1 = test case id
			 *	2 = test case title
			 *	3 = the message
			 */
			switch(status) {
				case TestStatus.Failed:
					sample = "{0}: Test Case {1}: {2} Failed ({3})";
					break;
				case TestStatus.Stopped:
					sample = "{0}: Test Case {1}: {2} Execution Halted ({3})";
					break;
				case TestStatus.Running:
					sample = "{0}: Test Case {1}: {2} - {3}";
					break;
				case TestStatus.Passed:
					sample = "{0}: Test Case {1}: {2} Passed ({3})";
					break;
				default:
					break;
			}

			try {
				using (StreamWriter printer = new StreamWriter(fp, true)) {
					try {
						//write the line
						printer.WriteLine(String.Format(sample, new object[] { DateTime.Now.ToString(), testId, title, message }));
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
					catch (FormatException fe) {
						System.Console.WriteLine("TextReporter.Update [" + sample + "] is a bad format: " + fe.ToString());
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

				} //end using printer

				//if the file is now larger than 
				if ((new FileInfo(fp)).Length > MAX_FSIZE) {
					this.Restart(true);
				}

				//no matter if the file rolled or not, the write succeeded
				return true;

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
