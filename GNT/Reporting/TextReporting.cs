using System;
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
	/// Manages a 
	/// </summary>
	class TextReporter : AbsReportWriter {
		private string fp;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
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
		/// <param name="msg">The text to write.</param>
		/// <returns>True if the write succeeded.</returns>
		public override bool Update(string msg) {
			System.Console.WriteLine("TextReporter: writing to file - " + msg);
			return true;
		}
	}
}
