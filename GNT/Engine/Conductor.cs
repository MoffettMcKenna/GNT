using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNT.Engine {
	class Conductor {
		private string cfgfile;

		public Conductor(string file) {
			cfgfile = file;
		}

		public AbsEngine BuildEngine() {
			AbsEngine eng = null;


			return eng;
		}

		public string Cfgfile { get => cfgfile; set => cfgfile = value; }
	}
}
