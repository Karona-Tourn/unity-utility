using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

public static class EditorUtil
{
	/// <summary>
	/// Gets the assets.
	/// </summary>
	/// <returns>The paths of assets.</returns>
	/// <param name="searchPattern">Search pattern.</param>
	/// <param name="searchOption">Search option.</param>
	public static string[] GetAssets(string searchPattern, SearchOption searchOption)
	{
		return Directory.GetFiles (Application.dataPath, searchPattern, searchOption)
			.Select (p => FileUtil.GetProjectRelativePath (p.Replace ('\\', '/')))
			.ToArray ();
	}

	/// <summary>
	/// Gets the short name of the asset.
	/// </summary>
	/// <returns>The short asset name.</returns>
	/// <param name="path">Path.</param>
	/// <param name="includeExtension">If set to <c>true</c> include extension.</param>
	public static string GetShortAssetName(string path, bool includeExtension = true)
	{
		string name = path.Substring (path.LastIndexOf ('/') + 1);
		if (!includeExtension)
		{
			name = name.Replace (".unity", "");
		}
		return name;
	}

	/// <summary>
	/// Creates the asset folder path if not exist.
	/// </summary>
	/// <param name="parentFolder">Parent folder name.</param>
	/// <param name="folder">Folder name</param>
	public static void CreateAssetFolderPathIfNotExist(string parentFolder, string folder)
	{
		if (string.IsNullOrEmpty (AssetDatabase.AssetPathToGUID (parentFolder + '/' + folder)))
		{
			AssetDatabase.CreateFolder (parentFolder, folder);
		}
	}

	public static string GetFullAssetPath (Object obj)
	{
		return string.Format ("{0}{1}", Application.dataPath, AssetDatabase.GetAssetPath (obj).Replace ("Assets", ""));
	}

	public static void ShowExplorer (string itemPath)
	{
		itemPath = itemPath.Replace (@"/", @"\");   // explorer doesn't like front slashes
		System.Diagnostics.Process.Start ("explorer.exe", "/select," + itemPath);
	}

	/// <summary>
	/// Used to set execution order for the mon scripts
	/// </summary>
	/// <typeparam name="T">Type inherited from MonoBehaviour</typeparam>
	/// <param name="order">Exeuction order can be negative or positive value</param>
	public static void SetExecutionOrder<T> (int order) where T : MonoBehaviour
	{
		// Get the name of the script we want to change it's execution order
		string scriptName = typeof (T).Name;

		// Iterate through all scripts (Might be a better way to do this?)
		foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts ())
		{
			// If found our script
			if (monoScript.name == scriptName)
			{
				// And it's not at the execution time we want already
				// (Without this we will get stuck in an infinite loop)
				if (MonoImporter.GetExecutionOrder (monoScript) != order)
				{
					MonoImporter.SetExecutionOrder (monoScript, order);
				}
				break;
			}
		}
	}
}
