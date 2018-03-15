using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public static class UIEditorMenu
{
	[MenuItem ("GameObject/Reset Default Material for UI Objects", false, 10)]
	private static void ResetDefaultMaterialForUIObjects ()
	{
		var gos = GameObject.FindObjectsOfType<GameObject> ();
		foreach (var go in gos)
		{
			ScriptUtil.EnumerateChildren (go.transform, child =>
			{
				var txt = child.gameObject.GetComponent<Text> ();
				if (txt)
					txt.material = null;

				var img = child.gameObject.GetComponent<Image> ();
				if (img)
					img.material = null;
			});
		}
	}
}
