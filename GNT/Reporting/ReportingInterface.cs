using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Reporting {

	public enum TestStatus { Running, Passed, Failed, Stopped }

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
		/// Write a new test case entry to the report.
		/// </summary>
		/// <param name="title">The test case title.</param>
		/// <param name="testId">The test id.</param>
		/// <param name="status">The current test execution status.</param>
		/// <param name="message">Any extra text which should be recorded.</param>
		/// <returns></returns>
		public abstract bool Update(string title, int testId, TestStatus status, string message);
		/// <summary>
		/// Clear the entries and re-start the file.
		/// </summary>
		/// <param name="roll">If this is true the previous data will be saved by rolling the report.</param>
		/// <returns>True if the operation succeeded.</returns>
		public abstract bool Restart(bool roll);
	}
}
