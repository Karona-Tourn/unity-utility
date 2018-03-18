using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ScriptDefineSymbolEditorWindow : EditorWindow
{
	public class ScriptDefinedSymbolManager
	{
		public class Symbol
		{
			public string name = "";
			public bool selected = false;

			public Symbol () { }

			public Symbol (string name, bool selected)
			{
				this.name = name;
				this.selected = selected;
			}
		}

		private const string SRC_FILE = "ScriptDefineSymbol.txt";
		private List<Symbol>						_symbols			= new List<Symbol>();
		private BuildTargetGroup					_selectedTarget		= BuildTargetGroup.Standalone;
		private ScriptDefineSymbolEditorWindow		_window				= null;
		private BuildTargetGroup[]                  _deprecatedTargets	= null;

		private string[] GetDefinedSymbols ( BuildTargetGroup target )
		{
			return PlayerSettings
				.GetScriptingDefineSymbolsForGroup ( target )
				.Split ( ';' )
				.Where ( s => !string.IsNullOrEmpty ( s ) )
				.Select ( s => s.Trim () ).ToArray ();
		}

		private void SetDefinedSymbols ( BuildTargetGroup target, string[] symbols )
		{
			try
			{
				PlayerSettings.SetScriptingDefineSymbolsForGroup ( target, string.Join ( ";", symbols ) );
			}
			catch ( Exception ex )
			{
				Debug.LogWarning ( ex.Message );
			}
		}

		public bool HasAnySymbol { get { return _symbols.Count > 0; } }

		public List<Symbol> Symbols
		{
			get { return _symbols; }
		}

		//public BuildTargetGroup[] GetInstalledBuildTargets ()
		//{
		//	List<BuildTarget> installedTargets = new List<BuildTarget>();
		//	var moduleManager = Type.GetType("UnityEditor.Modules.ModuleManager,UnityEditor.dll");
		//	var isPlatformSupportLoaded = moduleManager.GetMethod("IsPlatformSupportLoaded", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
		//	var getTargetStringFromBuildTarget = moduleManager.GetMethod("GetTargetStringFromBuildTarget", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

		//	var targets = Enum.GetValues(typeof(BuildTarget));
		//	for ( int i = 0; i < targets.Length; i++ )
		//	{
		//		var target = (BuildTarget)targets.GetValue ( i );
		//		bool isInstalled = (bool)isPlatformSupportLoaded.Invoke ( null, new object[] { (string)getTargetStringFromBuildTarget.Invoke ( null, new object[] { target } ) } );
		//		if ( !isInstalled ) continue;
		//		installedTargets.Add ( target );
		//	}

		//	return installedTargets
		//		.Select ( t => (BuildTargetGroup)Enum.Parse ( typeof ( BuildTargetGroup ), t.ToString () + "Group" ) )
		//		.ToArray ();
		//}

		public ScriptDefinedSymbolManager ( ScriptDefineSymbolEditorWindow window )
		{
#pragma warning disable 0618
			_deprecatedTargets = new BuildTargetGroup[]
			{
				BuildTargetGroup.Unknown,
				(BuildTargetGroup)2, // WebPlayer
				BuildTargetGroup.XBOX360,
				BuildTargetGroup.PS3,
				BuildTargetGroup.WP8,
				BuildTargetGroup.BlackBerry,
				BuildTargetGroup.SamsungTV,
			};
#pragma warning restore 0618
			_window = window;
		}

		public BuildTargetGroup SelectedTarget
		{
			get { return _selectedTarget; }
			set
			{
				_selectedTarget = value;

				UnselectAll ();

				var definedSymbols = GetDefinedSymbols(_selectedTarget);

				for ( int i = definedSymbols.Length - 1; i >= 0; i-- )
				{
					_symbols.Find ( s => s.name == definedSymbols[i] ).selected = true;
				}
			}
		}

		public void UnselectAll ()
		{
			foreach(var s in _symbols)
			{
				s.selected = false;
			}
		}

		public void Save ()
		{
			SetDefinedSymbols ( _selectedTarget, _symbols.Where ( s => s.selected ).Select ( s => s.name ).ToArray () );
			_window.ShowNotification ( new GUIContent ( "Saved" ) );
		}

		public void DeleteSymbolAt (int index)
		{
			var deletedSymbol = _symbols[index];
			_symbols.RemoveAt ( index );

			SaveDataFile ();

			var targets = Enum.GetValues(typeof(BuildTargetGroup));
			for ( int i = 0; i < targets.Length; i++ )
			{
				var target = (BuildTargetGroup)targets.GetValue(i);

				if ( _deprecatedTargets.Contains ( target ) ) continue;

				List<string> symbols = new List<string>(GetDefinedSymbols ( target ));
				symbols.Remove ( deletedSymbol.name );
				SetDefinedSymbols ( target, symbols.ToArray () );
			}
		}

		public void DeleteAllSymbols ()
		{
			_symbols.Clear ();
			SaveDataFile ();
			var targets = Enum.GetValues(typeof(BuildTargetGroup));
			for ( int i = 0; i < targets.Length; i++ )
			{
				var target = (BuildTargetGroup)targets.GetValue(i);

				if ( _deprecatedTargets.Contains ( target ) ) continue;

				SetDefinedSymbols ( target, new string[0] );
			}
		}

		// Updated data file contents with new updated symbol list
		private void SaveDataFile ()
		{
			string content = "";

			if ( _symbols.Count > 0 ) content = string.Join ( "\n", _symbols.Select ( s => s.name ).ToArray () );

			File.WriteAllText ( EditorConstants.PATH_EDITOR_RESOURCE + SRC_FILE, content, System.Text.Encoding.UTF8 );

			AssetDatabase.Refresh ();
		}

		public bool AddNewSymbol (string name)
		{
			if ( _symbols.Exists ( s => s.name == name ) ) return false;
			_symbols.Add ( new Symbol ( name, false ) );
			SaveDataFile ();
			return true;
		}

		public void Init ()
		{
			_symbols.Clear ();

			if ( !Directory.Exists ( EditorConstants.PATH_EDITOR_RESOURCE ) )
			{
				AssetDatabase.CreateFolder ( EditorConstants.FOLDER_ASSETS, EditorConstants.FOLDER_EDITOR_RESOURCE );
				AssetDatabase.Refresh ();
			}

			var file = EditorGUIUtility.Load (SRC_FILE);
			if ( file )
			{
				TextAsset ta = (TextAsset)file;
				var symbols = ta.text.Split ('\n')
				.Where (s => !string.IsNullOrEmpty (s))
				.Select (s => new Symbol(){ name = s.Trim(), selected = false })
				.ToArray ();

				_symbols.AddRange ( symbols );
			}

			bool haveNewSymbol = false;

			Array targets = Enum.GetValues(typeof(BuildTargetGroup));
			for ( int i = 0; i < targets.Length; i++ )
			{
				var targetSymbols = GetDefinedSymbols ( (BuildTargetGroup)targets.GetValue ( i ) );
				for ( int j = 0; j < targetSymbols.Length; j++ )
				{
					if ( _symbols.Exists ( s => s.name == targetSymbols[j] ) ) continue;

					_symbols.Add ( new Symbol ()
					{
						name = targetSymbols[j],
						selected = false
					} );

					haveNewSymbol = true;
				}
			}

			if( haveNewSymbol )
			{
				SaveDataFile ();
			}

			SelectedTarget = BuildTargetGroup.Standalone;
		}
	}

	private Vector2		_scrollPos		= Vector2.zero;
	private string		_newSymbolName	= "";
	private ScriptDefinedSymbolManager _symbolManager = null;

	[MenuItem (EditorConstants.MENU_TEAM_NAME + "Defined Symbol Editor")]
	private static void Open ()
	{
		GetWindow<ScriptDefineSymbolEditorWindow> ().titleContent = new GUIContent ("Script Symbol Define");
	}

	private void OnEnable ()
	{
		minSize = new Vector2 (400, 300);
		_symbolManager = new ScriptDefinedSymbolManager (this);
		_symbolManager.Init ();
	}

	private void DrawSymbolList ()
	{
		// List all symbols
		_scrollPos = EditorGUILayout.BeginScrollView (_scrollPos);

		var symbols = _symbolManager.Symbols;

		for (int i = 0; i < symbols.Count; i++)
		{
			var symbol = symbols[i];
			EditorGUILayout.BeginHorizontal (EditorStyles.helpBox);
			symbol.selected = EditorGUILayout.Toggle ( symbol.selected, GUILayout.Width ( 20 ) );

			bool deleted = false;

			if (GUILayout.Button ("X", GUILayout.Width (20)))
			{
				bool yes = EditorUtility.DisplayDialog ("Delete", string.Format ("Are you sure to delete symbol {0}?", symbol.name), "Yes", "No");
				if (yes)
				{
					_symbolManager.DeleteSymbolAt ( i );
					i--;
					deleted = true;
					ShowNotification ( new GUIContent ( "The symbol is deleted." ) );
				}
			}

			if (!deleted)
			{
				EditorGUILayout.LabelField (symbol.name, GUILayout.ExpandWidth (true));
			}

			EditorGUILayout.EndHorizontal ();

			if ( deleted ) break;
		}

		EditorGUILayout.EndScrollView ();
	}

	/// <summary>
	/// Draw panel that can add a new symbol
	/// </summary>
	private void DrawHead ()
	{
		EditorGUILayout.BeginHorizontal ();

		_newSymbolName = EditorGUILayout.TextField ( "Symbol Name", _newSymbolName, GUILayout.MaxWidth ( 450 ) );

		if ( GUILayout.Button ( "Add", GUILayout.Width ( 40 ) ) )
		{
			string safeSymbolName =  _newSymbolName.Trim ().Replace ("\n", "");

			if ( string.IsNullOrEmpty ( safeSymbolName ) )
			{
				EditorUtility.DisplayDialog ( "Warn!", "Symbol name cannot be empty.", "OK" );
			}
			else if ( _newSymbolName.Contains ( " " ) )
			{
				EditorUtility.DisplayDialog ( "Warn!", "Symbol name cannot contain space.", "OK" );
			}
			else
			{
				bool success = _symbolManager.AddNewSymbol ( safeSymbolName );
				if ( success )
				{
					_newSymbolName = "";
					ShowNotification ( new GUIContent ( "New symbol is added" ) );

					// Leave focus cursor from text field of symbol name
					GUI.FocusControl ( null );
				}
				else
				{
					EditorUtility.DisplayDialog ( "Error!", "The symbol already exists.", "OK" );
				}
			}
		}

		// Delete all symbole command
		if ( GUILayout.Button ( "Delete All", GUILayout.MaxWidth ( 90 ) ) )
		{
			if ( _symbolManager.HasAnySymbol )
			{
				bool yes = EditorUtility.DisplayDialog ("Delete All", "Are you sure to delete all symobls?", "Yes", "No");
				if ( yes )
				{
					_symbolManager.DeleteAllSymbols ();
					ShowNotification ( new GUIContent ( "All symbols are deleted." ) );
				}
			}
		}

		if ( GUILayout.Button ( "Save", GUILayout.MaxWidth ( 60 ) ) )
		{
			_symbolManager.Save ();
		}

		EditorGUILayout.EndHorizontal ();
	}

	private void OnGUI ()
	{
		var target = (BuildTargetGroup)EditorGUILayout.EnumPopup ( "Platform", _symbolManager.SelectedTarget );
		if ( target != _symbolManager.SelectedTarget )
		{
			_scrollPos = Vector2.zero;
			_symbolManager.SelectedTarget = target;
		}

		DrawHead ();
		DrawSymbolList ();
	}
}
