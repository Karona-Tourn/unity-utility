using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiSwitcher : MonoBehaviour
{
	[Serializable]
	public class ButtonSpriteState
	{
		public Sprite normal = null;
		public Sprite pressed = null;

		public void SetButtonSpriteState (Button button)
		{
			var state = button.spriteState;
			state.pressedSprite = pressed;
			button.spriteState = state;
			button.GetComponent<Image> ().sprite = normal;
		}
	}

	[SerializeField]
	private Button button = null;

	[SerializeField]
	private ButtonSpriteState on = new ButtonSpriteState ();

	[SerializeField]
	private ButtonSpriteState off = new ButtonSpriteState ();

	[SerializeField]
	private bool isOn = false;

	[Serializable]
	public class Callback : UnityEvent<bool> { }
	public Callback onValueChanged = new Callback ();

	public bool IsOn
	{
		get
		{
			return isOn;
		}
		set
		{
			isOn = value;
			var state = isOn ? on : off;
			state.SetButtonSpriteState (button);
			onValueChanged.Invoke (isOn);
		}
	}

	private void Awake ()
	{
		IsOn = isOn;
	}

	private void Start ()
	{
		if (!button) { button = GetComponent<Button> (); }
	}

	public void OnClick ()
	{
		IsOn = !IsOn;
	}

}
