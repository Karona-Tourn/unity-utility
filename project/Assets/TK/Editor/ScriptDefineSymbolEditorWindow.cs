using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptDefineSymbolEditorWindow : EditorWindow
{
	private const string PREF_KEY = "script-define-symbol-index";
	private const string SRC_FILE = "ScriptDefineSymbol.txt";

	private string[] platforms = new string[0];
	private List<bool> activeList = new List<bool> ();
	private List<string> symbolList = new List<string> ();
	private BuildTargetGroup selectedBuildTarget = BuildTargetGroup.Standalone;
	private Vector2 scrollPos = Vector2.zero;
	private int tabIndex = 0;
	private string newSymbolName = "";

	[MenuItem (EditorConstants.MENU_TEAM_NAME + "Defined Symbol Editor")]
	private static void Open ()
	{
		GetWindow<ScriptDefineSymbolEditorWindow> ().titleContent = new GUIContent ("Script Symbol Define");
	}

	// Updated data file contents with new updated symbol list
	private void SaveDataFile ()
	{
		string content = "";
		if (symbolList.Count > 0)
			content = string.Join ("\n", symbolList.ToArray ());
		File.WriteAllText (EditorConstants.PATH_EDITOR_RESOURCE + SRC_FILE, content, System.Text.Encoding.UTF8);
		AssetDatabase.Refresh ();
	}

	private IEnumerable<string> GetDefinedSymbols (BuildTargetGroup target)
	{
		return PlayerSettings.GetScriptingDefineSymbolsForGroup (target).Split (';').Where (s => !string.IsNullOrEmpty (s)).Select (s => s.Trim ());
	}

	private void Init ()
	{
		if (!Directory.Exists (EditorConstants.PATH_EDITOR_RESOURCE))
		{
			AssetDatabase.CreateFolder (EditorConstants.FOLDER_ASSETS, EditorConstants.FOLDER_EDITOR_RESOURCE);
			AssetDatabase.Refresh ();
		}

		tabIndex = EditorPrefs.GetInt (PREF_KEY, 0);

		var file = EditorGUIUtility.Load (SRC_FILE);
		if (file)
		{
			symbolList.Clear ();
			activeList.Clear ();

			TextAsset ta = (TextAsset)file;
			string[] symbols = ta.text.Split ('\n')
				.Where (s => !string.IsNullOrEmpty (s))
				.Select (s => s.Trim ()).ToArray ();
			int length = symbols.Length;
			for (int i = 0; i < length; i++)
			{
				symbolList.Add (symbols[i]);
				activeList.Add (false);
			}
		}

		platforms = new string[] {
			BuildTargetGroup.Standalone.ToString(),
			BuildTargetGroup.Android.ToString(),
			BuildTargetGroup.iOS.ToString()
		};

		bool newUpdates = false;

		for (int i = 0; i < platforms.Length; i++)
		{
			var p = (BuildTargetGroup)Enum.Parse (typeof (BuildTargetGroup), platforms[i]);
			var symbols = GetDefinedSymbols (p);
			foreach (var s in symbols)
			{
				if (!symbolList.Contains (s))
				{
					symbolList.Add (s);
					activeList.Add (false);
					newUpdates = true;
				}
			}
		}

		if (newUpdates)
		{
			SaveDataFile ();
		}

		SetSelectedTab (tabIndex);
	}

	public void LoadBuildTargetSymbols (BuildTargetGroup buildTarget)
	{
		selectedBuildTarget = buildTarget;
		List<string> targetSymbols = new List<string> ();
		targetSymbols.AddRange (GetDefinedSymbols (buildTarget));
		for (int i = 0; i < symbolList.Count; i++)
		{
			activeList[i] = targetSymbols.Contains (symbolList[i]);
		}
		scrollPos = Vector2.zero;
	}

	private void OnEnable ()
	{
		Init ();
	}

	private void OnDisable ()
	{
		EditorPrefs.SetInt (PREF_KEY, tabIndex);
	}

	private void ClearAllSymboles ()
	{
		symbolList.Clear ();
		activeList.Clear ();
	}

	private void RemoveSymbol (int index)
	{
		symbolList.RemoveAt (index);
		activeList.RemoveAt (index);
	}

	private void DrawSymbolDefined (BuildTargetGroup buildTarget)
	{
		// Delete all symbole command
		if (GUILayout.Button ("Delete All", GUILayout.Width (100)))
		{
			bool yes = EditorUtility.DisplayDialog ("Delete All", "Are you sure to delete all symobls?", "Yes", "No");
			if (yes)
			{
				ClearAllSymboles ();
				SaveDataFile ();
				EditorUtility.DisplayDialog ("Delete All", "All symbols are deleted.", "OK");
			}
		}

		// List all symbols
		scrollPos = EditorGUILayout.BeginScrollView (scrollPos);

		for (int i = 0; i < symbolList.Count; i++)
		{
			EditorGUILayout.BeginHorizontal (EditorStyles.helpBox);
			activeList[i] = EditorGUILayout.Toggle (activeList[i], GUILayout.Width (20));
			bool deleted = false;
			if (GUILayout.Button ("X", GUILayout.Width (20)))
			{
				bool yes = EditorUtility.DisplayDialog ("Delete", string.Format ("Are you sure to delete symbol {0}?", symbolList[i]), "Yes", "No");
				if (yes)
				{
					RemoveSymbol (i);
					SaveDataFile ();
					EditorUtility.DisplayDialog ("Delete", "The symbol is deleted.", "OK");
					i--;
					deleted = true;
				}
			}
			if (!deleted)
			{
				EditorGUILayout.LabelField (symbolList [i], GUILayout.ExpandWidth (true));
			}
			EditorGUILayout.EndHorizontal ();
		}

		EditorGUILayout.EndScrollView ();
	}

	private void Save (BuildTargetGroup buildTarget)
	{
		List<string> selectedSymbols = new List<string> ();
		for (int i = 0; i < symbolList.Count; i++)
		{
			if (activeList[i])
				selectedSymbols.Add (symbolList[i]);
		}
		PlayerSettings.SetScriptingDefineSymbolsForGroup (buildTarget, string.Join (";", selectedSymbols.ToArray ()));
		LoadBuildTargetSymbols (buildTarget);
	}

	private void SetSelectedTab (int index)
	{
		tabIndex = index;
		LoadBuildTargetSymbols ((BuildTargetGroup)Enum.Parse (typeof (BuildTargetGroup), platforms[tabIndex]));
	}

	/// <summary>
	/// Draw panel that can add a new symbol
	/// </summary>
	private void DrawAddPanel ()
	{
		EditorGUILayout.BeginHorizontal ();

		newSymbolName = EditorGUILayout.TextField ("Symbol Name", newSymbolName, GUILayout.MaxWidth (500));

		if (GUILayout.Button ("X", GUILayout.Width (40)))
		{
			newSymbolName = "";
		}

		if (GUILayout.Button ("+", GUILayout.Width (40)))
		{
			newSymbolName = newSymbolName.Trim ();
			newSymbolName = newSymbolName.Replace ("\n", "");
			if (string.IsNullOrEmpty (newSymbolName))
			{
				EditorUtility.DisplayDialog ("Warn!", "Symbol name cannot be empty.", "OK");
			}
			else if (newSymbolName.Contains (" "))
			{
				EditorUtility.DisplayDialog ("Warn!", "Symbol name cannot contain space.", "OK");
			}
			else if (symbolList.Contains (newSymbolName))
			{
				EditorUtility.DisplayDialog ("Error!", "The symbol already exists.", "OK");
			}
			else
			{
				symbolList.Add (newSymbolName);
				activeList.Add (false);
				newSymbolName = "";
				SaveDataFile ();
				EditorUtility.DisplayDialog ("Saved", "The new symbol is saved.", "OK");
			}
		}

		EditorGUILayout.EndHorizontal ();
	}

	private void OnGUI ()
	{
		int newIndex = GUILayout.Toolbar (tabIndex, platforms);
		if (tabIndex != newIndex)
		{
			SetSelectedTab (newIndex);
		}
		DrawAddPanel ();
		DrawSymbolDefined (selectedBuildTarget);
		if (GUILayout.Button ("SAVE"))
		{
			Save (selectedBuildTarget);
		}
	}
}
