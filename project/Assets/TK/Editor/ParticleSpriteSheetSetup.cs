using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ParticleSpriteSheetSetup
{
	[MenuItem(EditorConstants.MENU_GAME_OBJECT + "Particle Sprite Sheet")]
	public static void Open ()
	{
		var path = EditorUtility.OpenFilePanelWithFilters ("Open Texture", Application.dataPath, new string[] { "Image files", "png,jpg,jpeg" });

		if (string.IsNullOrEmpty (path)) { return; }

		var go = Selection.activeGameObject;

		if (go == null) { return; }

		var p = go.GetComponent<ParticleSystem> ();

		if (p == null) { return; }

		path = path.Replace (Application.dataPath, "Assets");

		var assets = AssetDatabase.LoadAllAssetRepresentationsAtPath (path);

		if (assets.Length == 0) { return; }

		var m = p.textureSheetAnimation;

		m.enabled = true;
		m.mode = ParticleSystemAnimationMode.Sprites;

		while (m.spriteCount > 0)
		{
			m.RemoveSprite (0);
		}

		for (int i = 0; i < assets.Length; i++)
		{
			m.AddSprite ((Sprite)assets[i]);
		}
	}
}
