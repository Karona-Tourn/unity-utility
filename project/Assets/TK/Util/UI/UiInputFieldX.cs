using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class UiInputFieldX : InputField
{
    [Serializable]
    public class KeyboardDoneEvent : UnityEvent{}

    [SerializeField]
    private KeyboardDoneEvent m_keyboardDone = new KeyboardDoneEvent ();

	public bool Done{ get { return m_Keyboard.done; } }

    public KeyboardDoneEvent onKeyboardDone {
        get { return m_keyboardDone; }
        set { m_keyboardDone = value; }
    }

    void Update ()
    {
        if (m_Keyboard != null && m_Keyboard.done && !m_Keyboard.wasCanceled) {
            m_keyboardDone.Invoke ();
        }
    }
}
