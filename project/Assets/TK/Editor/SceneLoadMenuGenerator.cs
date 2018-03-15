using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;

public static class SceneLoadMenuGenerator
{
	[MenuItem (EditorConstants.MENU_TEAM_NAME + "Generate Scene Menu")]
	private static void GenerateMenu ()
	{
		const string codeFormat = @"using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneMenu
{
	#CODE
	private static void LoadScene(string sceneName)
	{
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo ())
		{
			EditorSceneManager.OpenScene(sceneName);
		}
	}
}
";
		const string funcCodeFormat = @"
	[MenuItem(""Scenes/#NAME"")]
	private static void LoadScene#NAME()
	{
		LoadScene (""#PATH"");
	}
";
		var paths = EditorUtil
			.GetAssets ("*.unity", SearchOption.AllDirectories)
			.Select (p => new{path = p, name = EditorUtil.GetShortAssetName (p, false)});
		StringBuilder builder = new StringBuilder ();
		foreach (var path in paths)
		{
			builder.Append (funcCodeFormat.Replace ("#NAME", path.name.Replace (' ', '_')).Replace ("#PATH", path.path));
		}

		EditorUtil.CreateAssetFolderPathIfNotExist ("Assets", "Editor");

		string content = codeFormat.Replace ("#CODE", builder.ToString ());
		File.WriteAllText (Application.dataPath + "/Editor/SceneMenu.cs", content, Encoding.UTF8);
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
	}
}
