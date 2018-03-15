using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace PM
{
	public class UiNumericDisplay : MonoBehaviour
	{
		[SerializeField]
		private Text text = null;

		[SerializeField]
		private bool hasDigitDelimitter = false;

		public void SetDisplayNumber (double number)
		{
			if (hasDigitDelimitter)
			{
				text.text = ScriptUtil.AbbreviateLongNumber (number);
			}
			else
			{
				text.text = number.ToString ();
			}
		}
	}

}
