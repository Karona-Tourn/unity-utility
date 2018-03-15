using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectFolderEditorWindow : EditorWindow
{
	public class Folder
	{
		public string path = "";
		public bool created = false;
	}

	public class FolderList : List<Folder>, IComparer<Folder>
	{
		public void Add (string path, bool created = false)
		{
			Add (new Folder () { path = path, created = created });
		}

		public int Compare (Folder x, Folder y)
		{
			return string.Compare (x.path, y.path);
		}
	}

	private const string TITLE = "Folder Generator";

	private FolderList folderList = new FolderList ();
	private Vector2 scrollPos = Vector2.zero;
	private int count = 0;

	[MenuItem (EditorConstants.MENU_ASSETS + TITLE)]
	private static void Open ()
	{
		GetWindow<ProjectFolderEditorWindow> ().titleContent = new GUIContent (TITLE);
	}

	private bool FolderExists (string folder)
	{
		return Directory.Exists (folder);
	}

	private void Init ()
	{
		folderList.Add (EditorConstants.PATH_EDITOR, FolderExists (EditorConstants.PATH_EDITOR));
		folderList.Add (EditorConstants.PATH_RESOURCES, FolderExists (EditorConstants.PATH_RESOURCES));
		folderList.Add (EditorConstants.PATH_STREAMING_ASSETS, FolderExists (EditorConstants.PATH_STREAMING_ASSETS));
		folderList.Add (EditorConstants.PATH_EDITOR_RESOURCE, FolderExists (EditorConstants.PATH_EDITOR_RESOURCE));
		folderList.Add (EditorConstants.PATH_STANDARD_ASSETS, FolderExists (EditorConstants.PATH_STANDARD_ASSETS));
		folderList.Add (EditorConstants.PATH_ASSETS + "Scripts/", FolderExists (EditorConstants.PATH_ASSETS + "Scripts/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Scenes/", FolderExists (EditorConstants.PATH_ASSETS + "Scenes/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Fonts/", FolderExists (EditorConstants.PATH_ASSETS + "Fonts/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Icon/", FolderExists (EditorConstants.PATH_ASSETS + "Icon/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Models/", FolderExists (EditorConstants.PATH_ASSETS + "Models/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Textures/", FolderExists (EditorConstants.PATH_ASSETS + "Textures/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Sounds/", FolderExists (EditorConstants.PATH_ASSETS + "Sounds/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Test/", FolderExists (EditorConstants.PATH_ASSETS + "Test/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Prefabs/", FolderExists (EditorConstants.PATH_ASSETS + "Prefabs/"));
		folderList.Add (EditorConstants.PATH_PLUGINS, FolderExists (EditorConstants.PATH_PLUGINS));
		folderList.Add (EditorConstants.PATH_ANDROID, FolderExists (EditorConstants.PATH_ANDROID));
		folderList.Add (EditorConstants.PATH_ANDROID + "libs/", FolderExists (EditorConstants.PATH_ANDROID + "libs/"));
		folderList.Add (EditorConstants.PATH_IOS, FolderExists (EditorConstants.PATH_IOS));
		folderList.Add (EditorConstants.PATH_ASSETS + "Shaders/", FolderExists (EditorConstants.PATH_ASSETS + "Shaders/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Materials/", FolderExists (EditorConstants.PATH_ASSETS + "Materials/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "ScriptableObjects/", FolderExists (EditorConstants.PATH_ASSETS + "ScriptableObjects/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Animations/", FolderExists (EditorConstants.PATH_ASSETS + "Animations/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Videos/", FolderExists (EditorConstants.PATH_ASSETS + "Videos/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Gizmos/", FolderExists (EditorConstants.PATH_ASSETS + "Gizmos/"));
		folderList.Add (EditorConstants.PATH_ASSETS + "Terrain/", FolderExists (EditorConstants.PATH_ASSETS + "Terrain/"));

		folderList.Sort (folderList);

		count = folderList.Count;
	}

	private void OnFocus ()
	{
		RefreshFolderStat ();
	}

	private void RefreshFolderStat ()
	{
		for (int i = 0; i < count; i++)
		{
			Folder folder = folderList[i];
			folder.created = Directory.Exists (folder.path);
		}
	}

	private void CreateFolder (string folderPath, string parentFolder = "")
	{
		if (string.IsNullOrEmpty (folderPath))
			return;
		string folder = folderPath.Substring (0, folderPath.IndexOf ('/'));
		if (string.IsNullOrEmpty (folder))
			return;
		if (folder != EditorConstants.FOLDER_ASSETS)
		{
			string dir = parentFolder + folder;
			if (!Directory.Exists (dir))
			{
				Directory.CreateDirectory (dir);
				AssetDatabase.Refresh ();
			}
		}
		parentFolder += (folder + '/');
		CreateFolder (folderPath.Substring (folderPath.IndexOf ('/') + 1), parentFolder);
	}

	private void DrawCommands ()
	{
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos);

		for (int i = 0; i < count; i++)
		{
			Folder folder = folderList[i];

			EditorGUILayout.BeginHorizontal (EditorStyles.helpBox);

			EditorGUILayout.Toggle (folder.created, GUILayout.Width (20));

			EditorGUI.BeginDisabledGroup (folder.created);
			if (GUILayout.Button ("Create", GUILayout.Width (50)))
			{
				CreateFolder (folder.path);
				RefreshFolderStat ();
			}
			EditorGUI.EndDisabledGroup ();

			EditorGUILayout.LabelField (folder.path, GUILayout.ExpandWidth (true));

			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.EndScrollView ();
	}

	private void OnEnable ()
	{
		Init ();
	}

	private void OnGUI ()
	{
		DrawCommands ();
	}
}
