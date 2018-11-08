using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Localizationeer
{
	public class LevenshteinComparer
	{
		public static int ComputeDistance(string a, string b)
		{
			int lenA = a.Length;
			int lenB = b.Length;

			// step 1
			if (lenA == 0)
			{
				return lenB;
			}

			if (lenB == 0)
			{
				return lenA;
			}

			int[,] distance = new int[lenA + 1, lenB + 1];

			// step 2
			for (int i = 0; i <= lenA; distance[i, 0] = i++) ;
			for (int j = 0; j <= lenB; distance[0, j] = j++) ;

			// step 3
			for (int i = 1; i <= lenA; i++)
			{
				// step 4
				for (int j = 1; j <= lenB; j++)
				{
					// step 5
					int cost = a[i - 1] == b[j - 1] ? 0 : 1;

					// step 6
					distance[i, j] = Math.Min(
						Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
						distance[i - 1, j - 1] + cost);
				}
			}

			// step 7
			return distance[lenA, lenB];
		}

		public static int Compare(string a, string b, StringComparison comparisonType)
		{
			if (a == null || b == null)
			{
				return 0;
			}

			if (a.Length == 0 || b.Length == 0)
			{
				return 0;
			}

			if (comparisonType == StringComparison.CurrentCultureIgnoreCase ||
				comparisonType == StringComparison.InvariantCultureIgnoreCase ||
				comparisonType == StringComparison.OrdinalIgnoreCase)
			{
				a = a.ToLower();
				b = b.ToLower();
			}

			if (a.Equals(b))
			{
				return 100;
			}

			int distance = ComputeDistance(a, b);
			int result = 100 - distance * 100 / Math.Max(a.Length, b.Length);
			return result;
		}
	}
}
