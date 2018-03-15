using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorWindowBase<T> : EditorWindow where T : EditorWindowBase<T>
{
	public enum ShowMode
	{
		Basic,
		Tab,
		Util,
	}

	public static void Open(string title, ShowMode show)
	{
		var win = EditorWindow.CreateInstance<T> ();
		win.titleContent = new GUIContent (title);
		switch (show)
		{
			case ShowMode.Basic: win.Show (); break;
			case ShowMode.Tab: win.ShowTab (); break;
			case ShowMode.Util: win.ShowUtility (); break;
		}
	}

	protected virtual void OnGUI ()
	{
	}
}
