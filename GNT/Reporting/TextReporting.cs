﻿using System;
using System.Collections.Generic;
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
				System.Console.WriteLine(String.Format(sample, new object[] { testId, title, message }));
			}
			catch(ArgumentNullException ane) {
				System.Console.WriteLine("TextReporter.Update ArgumentNullException: " + ane.ToString());
				return false;
			}
			catch(FormatException fe) {
				System.Console.WriteLine("TextReporter.Update FormatException: " + fe.ToString());
				return false;
			}
			return true;
		} //end Update
	} //end TextReporter

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
				System.Console.WriteLine(String.Format("{0},{1},{2},{3}", new object[] { testId, title, status, message }));
			}
			catch (ArgumentNullException ane) {
				System.Console.WriteLine("CSVReporter.Update ArgumentNullException: " + ane.ToString());
				return false;
			}
			catch (FormatException fe) {
				System.Console.WriteLine("CSVReporter.Update FormatException: " + fe.ToString());
				return false;
			}
			return true;
		} //end Update
	} //end CSVReporter

}