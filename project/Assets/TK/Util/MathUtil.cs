using UnityEngine;

namespace TK
{
	/// <summary>
	/// Math utility to help working with math
	/// </summary>
	public static class MathUtil
	{
		/// <summary>
		/// Choose probability
		/// </summary>
		/// <param name="probs"></param>
		/// <returns></returns>
		public static int ChooseProbs (params float[] probs)
		{
			float total = 0;
			int length = probs.Length;

			for (int i = 0; i < length; i++)
			{
				total += probs[i];
			}

			float randomPoint = Random.value * total;

			for (int i = 0; i < length; i++)
			{
				if (randomPoint < probs[i])
				{
					return i;
				}
				else
				{
					randomPoint -= probs[i];
				}
			}

			return length - 1;
		}

		/// <summary>
		/// Calculate percent of value between min and max
		/// </summary>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static float CalculatePercent (float min, float max, float value)
		{
			if ((max - min) == 0)
			{
				return 0;
			}
			return Mathf.Clamp01 ((value - min) / (max - min));
		}
	}
}
