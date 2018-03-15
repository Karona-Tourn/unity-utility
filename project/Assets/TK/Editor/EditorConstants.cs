using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wrap all constants for working wtih scripting editor
/// </summary>
public class EditorConstants
{
	public const string FORWARD_SLASH = "/";

	// Root menu item names
	public const string TEAM_NAME = About.TEAM_NAME;
	public const string MENU_TEAM_NAME = TEAM_NAME + FORWARD_SLASH;
	public const string MENU_WINDOW = "Window" + FORWARD_SLASH + MENU_TEAM_NAME;
	public const string MENU_GAME_OBJECT = "GameObject" + FORWARD_SLASH + MENU_TEAM_NAME;
	public const string MENU_ASSETS = "Assets" + FORWARD_SLASH + MENU_TEAM_NAME;
	public const string MENU_EDIT = "Edit" + FORWARD_SLASH + MENU_TEAM_NAME;

	// Special hotkey for menu item
	public const string NO_KEY_MODIFY = "_";
	public const string HOTKEY_CTRL = "%";
	public const string HOTKEY_SHIFT = "#";
	public const string HOTKEY_ALT = "&";
	public const string HOTKEY_LEFT = "LEFT";
	public const string HOTKEY_RIGHT = "RIGHT";
	public const string HOTKEY_UP = "UP";
	public const string HOTKEY_DOWN = "DOWN";
	public const string HOTKEY_F1 = "F1";
	public const string HOTKEY_F2 = "F2";
	public const string HOTKEY_F3 = "F3";
	public const string HOTKEY_F4 = "F4";
	public const string HOTKEY_F5 = "F5";
	public const string HOTKEY_F6 = "F6";
	public const string HOTKEY_F7 = "F7";
	public const string HOTKEY_F8 = "F8";
	public const string HOTKEY_F9 = "F9";
	public const string HOTKEY_F10 = "F10";
	public const string HOTKEY_F11 = "F11";
	public const string HOTKEY_F12 = "F12";
	public const string HOTKEY_HOME = "HOME";
	public const string HOTKEY_END = "END";
	public const string HOTKEY_PAGE_UP = "PGUP";
	public const string HOTKEY_PAGE_DOWN = "PGDN";

	// Folder
	public const string FOLDER_ASSETS = "Assets";
	public const string FOLDER_EDITOR_RESOURCE = "Editor Default Resources";
	public const string FOLDER_RESOURCES = "Resources";
	public const string FOLDER_STREAMING_ASSETS = "StreamingAssets";
	public const string FOLDER_EDITOR = "Editor";
	public const string FOLDER_PLUGINS = "Plugins";
	public const string FOLDER_ANDROID = "Android";
	public const string FOLDER_IOS = "iOS";
	public const string FOLDER_STANDARD_ASSETS = "Standard Assets";

	// Path
	public const string PATH_ASSETS = FOLDER_ASSETS + FORWARD_SLASH;
	public const string PATH_EDITOR_RESOURCE = PATH_ASSETS + FOLDER_EDITOR_RESOURCE + FORWARD_SLASH;
	public const string PATH_RESOURCES = PATH_ASSETS + FOLDER_RESOURCES + FORWARD_SLASH;
	public const string PATH_STREAMING_ASSETS = PATH_ASSETS + FOLDER_STREAMING_ASSETS + FORWARD_SLASH;
	public const string PATH_EDITOR = PATH_ASSETS + FOLDER_EDITOR + FORWARD_SLASH;
	public const string PATH_PLUGINS = PATH_ASSETS + FOLDER_PLUGINS + FORWARD_SLASH;
	public const string PATH_ANDROID = PATH_PLUGINS + FOLDER_ANDROID + FORWARD_SLASH;
	public const string PATH_IOS = PATH_PLUGINS + FOLDER_IOS + FORWARD_SLASH;
	public const string PATH_STANDARD_ASSETS = PATH_ASSETS + FOLDER_STANDARD_ASSETS + FORWARD_SLASH;
}
