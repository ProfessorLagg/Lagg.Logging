using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lagg.Logging {
		internal static class Utils {
				public static string PadCenter(this string str, int len, char c = ' ') {
						bool padLeft = false;
						while (str.Length < len) {
								if (padLeft) {
										str = c + str;
										padLeft = false;
								}
								else {
										str = str + c;
										padLeft = true;
								}
						}
						return str;
				}
		}
}
