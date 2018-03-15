using UnityEngine;
using UnityEditor;

public class ScreenCaptureWindow : EditorWindowBase<ScreenCaptureWindow>
{
	private const string SAVE = "src_cap";
	private string filename = "";
	private int supersize = 1;

	[MenuItem (EditorConstants.MENU_TEAM_NAME + "Screen Capture")]
	public static void Open ()
	{
		Open ("Screen Capture", EditorWindowBase<ScreenCaptureWindow>.ShowMode.Basic);
	}

	private void OnEnable ()
	{
		// Load saved prefs
		if (EditorPrefs.HasKey (SAVE))
		{
			var save = EditorPrefsX.GetStringArray (SAVE);
			filename = save[0];
			supersize = int.Parse (save[1]);
		}
	}

	private void OnDisable ()
	{
		// Save prefs
		EditorPrefsX.SetStringArray (SAVE, new string[] { filename, supersize.ToString () });
	}

	protected override void OnGUI ()
	{
		EditorGUILayout.Space ();
		filename = EditorGUILayout.TextField ("File name", filename);
		supersize = EditorGUILayout.IntSlider ("Super size factor", supersize, 1, 5);
		EditorGUILayout.Space ();
		if (GUILayout.Button ("Capture"))
		{
			// Check if file name is empty
			if (string.IsNullOrEmpty (filename))
			{
				EditorUtility.DisplayDialog ("Warn", "File name is required.", "OK");
			}
			// Check if in play mode
			else if (!Application.isPlaying)
			{
				EditorUtility.DisplayDialog ("Warn", "Capture is available in play mode", "OK");
			}
			else
			{
				ScreenCapture.CaptureScreenshot (filename, supersize);
				EditorUtil.ShowExplorer (Application.dataPath.Replace("Assets", "") + filename);
			}
		}
	}
}
