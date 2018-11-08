using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localizationeer
{
	public class CloseEnoughComparer
	{
		public static int Compare(string a, string b, StringComparison comparisonType)
		{
			if (a.Equals(b, comparisonType))
			{
				return 100;
			}
			else
			{
				int match = 0;
				if (b.Length > a.Length)
				{
					string c = a;
					a = b;
					b = c;
				}
				bool ltr = true;
				bool rtl = true;
				for (int i = 0; i < a.Length && i < b.Length && (ltr || rtl); i++)
				{
					if (ltr && a[i].ToString().Equals(b[i].ToString(), comparisonType))
					{
						match++;
					}
					else
					{
						ltr = false;
					}
					if (rtl && a[a.Length - 1 - i].ToString().Equals(b[b.Length - 1 - i].ToString(), comparisonType))
					{
						match++;
					}
					else
					{
						rtl = false;
					}
				}
				int result = match * 100 / a.Length;
				return result;
			}
		}
	}
}
