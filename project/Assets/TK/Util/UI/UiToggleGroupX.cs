using UnityEngine;
using UnityEngine.UI;

public class UiToggleGroupX : MonoBehaviour
{
	[SerializeField]
	private ToggleGroup m_ToggleGroup = null;

	[SerializeField]
	private Toggle[] m_Toggles = new Toggle[0];

	public Toggle[] Toggles{ get { return m_Toggles; } }

	public void ToggleOn(int index, bool forceInvokeCallback = false)
	{
		for (int i = 0; i < m_Toggles.Length; i++)
		{
			m_ToggleGroup.RegisterToggle (m_Toggles [i]);
		}

		if (m_Toggles [index].isOn)
		{
			if (forceInvokeCallback)
				m_Toggles [index].onValueChanged.Invoke (true);
		}
		else
		{
			m_Toggles [index].isOn = true;
			m_ToggleGroup.NotifyToggleOn (m_Toggles [index]);
		}
	}
}
