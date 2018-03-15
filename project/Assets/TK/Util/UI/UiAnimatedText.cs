using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TK.UI
{

	public class UiAnimatedText : UIBehaviour
	{
		[SerializeField]
		private bool m_IgnoreTimeScale = true;

		[SerializeField]
		private float m_Delay = .2f;

		[SerializeField]
		private Text m_Text = null;

		[SerializeField]
		private string[] m_TextValues = new string[0];

		protected override void Awake ()
		{
			base.Awake ();
			if (m_Text == null)
				m_Text = gameObject.GetComponent<Text> ();
		}

		private void UpdateTextByIndex (int index)
		{
			m_Text.text = m_TextValues[index];
		}

		protected override void OnEnable ()
		{
			base.OnEnable ();
			UpdateTextByIndex (0);
			StartCoroutine ("Animate");
		}

		protected override void OnDisable ()
		{
			base.OnDisable ();
			StopCoroutine ("Animate");
		}

		private IEnumerator Animate ()
		{
			int index = 0;
			while (true)
			{
				UpdateTextByIndex (index);

				index++;
				if (index >= m_TextValues.Length)
					index = 0;

				if (m_IgnoreTimeScale)
					yield return new WaitForSecondsRealtime (m_Delay);
				else
					yield return new WaitForSeconds (m_Delay);
			}
		}
	}

}
