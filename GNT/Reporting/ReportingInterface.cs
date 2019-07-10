using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Reporting {
	abstract class AbsWriterShop {
		/// <summary>
		/// Creates a report object.
		/// </summary>
		/// <param name="details">Specific information about the logfile.</param>
		/// <returns>The reporter object.</returns>
		public abstract AbsReportWriter CreateReporter(string details);
	}

	abstract class AbsReportWriter {
		/// <summary>
		/// Write a new entry to the report.
		/// </summary>
		/// <param name="msg">The data to write.</param>
		/// <returns>True if the write succeeded.</returns>
		public abstract bool Update(string msg); //TODO rework the signature to include variable arguments and an exception for arg mis-match
		/// <summary>
		/// Clear the entries and re-start the file.
		/// </summary>
		/// <param name="roll">If this is true the previous data will be saved by rolling the report.</param>
		/// <returns>True if the operation succeeded.</returns>
		public abstract bool Restart(bool roll);
	}
}
