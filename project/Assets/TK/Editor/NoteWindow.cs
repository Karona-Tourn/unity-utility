using System.IO;
using UnityEngine;
using UnityEditor;

namespace Assets.PM.Editor
{
	public class NoteWindow : EditorWindowBase<NoteWindow>
	{
		private string log = "";
		private Vector2 scroll = Vector2.zero;
		private const string PATH = EditorConstants.PATH_EDITOR_RESOURCE + "note.txt";

		[MenuItem(EditorConstants.MENU_TEAM_NAME + "Note")]
		private static void Open ()
		{
			Open ("Note", EditorWindowBase<NoteWindow>.ShowMode.Basic);
		}

		private void OnEnable ()
		{
			if (File.Exists (PATH))
			{
				log = File.ReadAllText (PATH);
			}
		}

		protected override void OnGUI ()
		{
			scroll = EditorGUILayout.BeginScrollView (scroll);
			log = EditorGUILayout.TextArea (log, GUILayout.ExpandHeight (true));
			EditorGUILayout.EndScrollView ();
			if (GUILayout.Button ("Save"))
			{
				File.WriteAllText (PATH, log);
				AssetDatabase.SaveAssets ();
				AssetDatabase.Refresh ();
			}
		}
	}
}
