using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;
using System.IO;
using System.Linq;

public static class EditorProjectSetup
{
	[MenuItem(EditorConstants.MENU_TEAM_NAME + "Generate Tag Variables", priority = 1000)]
	private static void GenerateTagVariables()
	{
		const string PREF_SAVE = "editor-tag-save";

		string path	= EditorPrefs.GetString (PREF_SAVE, Application.dataPath);

		// Setup saving path
		path = EditorUtility.SaveFilePanel ("Save", path, "GameTag", "cs");

		// Exit the function if the path is null or empty
		if (string.IsNullOrEmpty (path))
			return;

		EditorPrefs.SetString (PREF_SAVE, path);

		string className = Path.GetFileNameWithoutExtension (path);
		
		string mainCodeFormat = @"public static class #ClassName#
{
#Content#
}".Replace("#ClassName#", className);
		StringBuilder sb = new StringBuilder ();
		foreach (var t in UnityEditorInternal.InternalEditorUtility.tags)
		{
			sb.AppendLine (string.Format ("\tpublic const string {0} = \"{1}\";", t.Replace (" ", ""), t));
		}

		string finalCode = mainCodeFormat.Replace ("#Content#", sb.ToString ());

		System.IO.File.WriteAllText (path, finalCode, Encoding.UTF8);

		AssetDatabase.Refresh ();
	}

	[MenuItem(EditorConstants.MENU_TEAM_NAME + "Generate Scene Variables", priority = 1001)]
	private static void GenerateSceneVariables()
	{
		const string PREF_SAVE = "editor-scene-save";

		string path	= EditorPrefs.GetString (PREF_SAVE, Application.dataPath);

		// Setup saving path
		path = EditorUtility.SaveFilePanel ("Save", path, "SceneName", "cs");

		// Exit the function if the path is null or empty
		if (string.IsNullOrEmpty (path))
			return;

		EditorPrefs.SetString (PREF_SAVE, path);

		string className = Path.GetFileNameWithoutExtension (path);

		string mainCodeFormat = @"public static class #ClassName#
{
#Content#
}".Replace("#ClassName#", className);

		// Filter scene names
		string[] sceneNames = Directory.GetFiles (Application.dataPath, "*.unity", SearchOption.AllDirectories).Select (p => Path.GetFileNameWithoutExtension (p)).ToArray ();

		StringBuilder sb = new StringBuilder ();
		foreach (var scene in sceneNames)
		{
			sb.AppendLine (string.Format ("\tpublic const string {0} = \"{1}\";", scene.Replace (" ", ""), scene));
		}

		string finalCode = mainCodeFormat.Replace ("#Content#", sb.ToString ());

		System.IO.File.WriteAllText (path, finalCode, Encoding.UTF8);

		AssetDatabase.Refresh ();
	}
}
