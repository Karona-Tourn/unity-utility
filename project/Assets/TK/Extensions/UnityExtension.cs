using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public static class UnityExtension
{
	public static void EnumerateChildRecursively (this Component parent, UnityAction<Transform> callback)
	{
		foreach (Transform child in parent.transform)
		{
			if (callback != null) { callback (child); }
			EnumerateChildRecursively (child, callback);
		}
	}

	public static void EnumerateChildRecursivelyOfType<T> (this Component parent, UnityAction<T> callback) where T : Component
	{
		foreach (Transform child in parent.transform)
		{
			if (callback != null)
			{
				T com = child.GetComponent<T> ();
				if (com) { callback (com); }
			}
			EnumerateChildRecursivelyOfType<T> (child, callback);
		}
	}

	public static IList<T> FindChildrenOfType<T> (this Component parent) where T : Component
	{
		IList<T> list = new List<T> ();
		parent.EnumerateChildRecursivelyOfType<T> (child => list.Add (child));
		return list;
	}

	public static float LocalPositionX(this Transform target)
	{
		return target.localPosition.x;
	}

	public static float LocalPositionY (this Transform target)
	{
		return target.localPosition.y;
	}

	public static float LocalPositionZ (this Transform target)
	{
		return target.localPosition.z;
	}

	public static void LocalPositionX (this Transform target, float x)
	{
		Vector3 pos = target.localPosition;
		pos.x = x;
		target.localPosition = pos;
	}

	public static void LocalPositionY (this Transform target, float y)
	{
		Vector3 pos = target.localPosition;
		pos.y = y;
		target.localPosition = pos;
	}

	public static void LocalPositionZ (this Transform target, float z)
	{
		Vector3 pos = target.localPosition;
		pos.z = z;
		target.localPosition = pos;
	}

	public static float PositionX (this Transform target)
	{
		return target.localPosition.x;
	}

	public static float PositionY (this Transform target)
	{
		return target.localPosition.y;
	}

	public static float PositionZ (this Transform target)
	{
		return target.localPosition.z;
	}

	public static void PositionY (this Transform target, float y)
	{
		Vector3 pos = target.position;
		pos.y = y;
		target.position = pos;
	}

	public static void PositionX (this Transform target, float x)
	{
		Vector3 pos = target.position;
		pos.x = x;
		target.position = pos;
	}

	public static void PositionZ (this Transform target, float z)
	{
		Vector3 pos = target.position;
		pos.z = z;
		target.position = pos;
	}

	public static void LocalScaleY (this RectTransform target, float y)
	{
		Vector3 scale = target.localScale;
		scale.y = y;
		target.localScale = scale;
	}

	/// <summary>
	/// Return list of children
	/// </summary>
	/// <param name="parent"></param>
	/// <returns></returns>
	public static Transform[] GetChildren (this Transform parent)
	{
		// Count of children
		int count = parent.childCount;

		Transform[] children = new Transform[count];

		for (int i = 0; i < count; i++)
		{
			children[i] = parent.GetChild (i);
		}

		return children;
	}

	public static Texture2D CompressDXT1 (this Texture2D texture)
	{
		var clone = new Texture2D (texture.width, texture.height, TextureFormat.RGB24, false);
		clone.SetPixels (texture.GetPixels ());
		clone.Apply ();
		return clone;
	}

	public static Sprite ConvertToSprite (this Texture2D sprite)
	{
		return Sprite.Create (sprite, new Rect (0, 0, sprite.width, sprite.height), new Vector2 (.5f, .5f));
	}

	#region Rich Text

	// Note that Color and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
	private static string ColorToHex (Color color)
	{
		int r = (int)(color.r * 255);
		int g = (int)(color.g * 255);
		int b = (int)(color.b * 255);
		int a = (int)(color.a * 255);
		string hex = r.ToString ("X2") + g.ToString ("X2") + b.ToString ("X2") + a.ToString ("X2");
		return hex;
	}

	private static Color HexToColor (string hex)
	{
		byte r = byte.Parse (hex.Substring (0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse (hex.Substring (2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse (hex.Substring (4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32 (r, g, b, 255);
	}

	public static string Size (this string value, int size)
	{
		return string.Format ("<size={0}>{1}</size>", size, value);
	}

	public static string Color (this string value, Color color)
	{
		return string.Format ("<color=#{0}>{1}</color>", ColorToHex (color), value);
	}

	public static string Bold (this string value)
	{
		return string.Format ("<b>{0}</b>", value);
	}

	public static string Italic (this string value)
	{
		return string.Format ("<i>{0}</i>", value);
	}

	#endregion

	#region UI
	public static bool TryEnableCanvas (this GameObject go, bool value)
	{
		Canvas c = go.GetComponent<Canvas> ();
		if (c)
		{
			c.enabled = value;
			return true;
		}
		return false;
	}

	public static bool TryCheckCanvasEnabled (this GameObject go)
	{
		Canvas c = go.GetComponent<Canvas> ();
		if (c)
		{
			return c.enabled;
		}
		return go.activeSelf;
	}

	public static bool TryShowCanvasGroup (this GameObject go, bool value)
	{
		CanvasGroup c = go.GetComponent<CanvasGroup> ();
		if (c)
		{
			c.alpha = value ? 1 : 0;
			c.blocksRaycasts = value;
			c.interactable = value;
			return true;
		}
		return false;
	}

	public static bool TryCheckCanvasGroupVisible (this GameObject go)
	{
		CanvasGroup c = go.GetComponent<CanvasGroup> ();
		if (c)
		{
			return c.alpha != 0;
		}
		return go.activeSelf;
	}

	public static Texture2D ToTexture (this Sprite sprite)
	{
		var croppedTexture = new Texture2D ((int)sprite.rect.width, (int)sprite.rect.height);
		var pixels = sprite.texture.GetPixels ((int)sprite.textureRect.x,
												(int)sprite.textureRect.y,
												(int)sprite.textureRect.width,
												(int)sprite.textureRect.height);
		croppedTexture.SetPixels (pixels);
		croppedTexture.Apply ();
		return croppedTexture;
	}

	public static void SetAlpha (this MaskableGraphic g, float a)
	{
		var color = g.color;
		color.a = a;
		g.color = color;
	}

	public static bool IsEmpty (this InputField input)
	{
		return string.IsNullOrEmpty (input.text);
	}

	public static bool IsEmpty (this Text text)
	{
		return string.IsNullOrEmpty (text.text);
	}

	public static void SetText (this Text text, string value)
	{
		text.text = value;
	}

	public static void SetText (this Text text, int value)
	{
		text.text = value.ToString ();
	}

	public static void SetText (this Text text, float value)
	{
		text.text = value.ToString ();
	}

	public static void AnchoredPositionX (this RectTransform target, float x)
	{
		Vector2 position = target.anchoredPosition;
		position.x = x;
		target.anchoredPosition = position;
	}

	public static void AnchoredPositionY (this RectTransform target, float y)
	{
		Vector2 position = target.anchoredPosition;
		position.y = y;
		target.anchoredPosition = position;
	}

	private static void SetAlpha<T> (T graphic, float alpha) where T : MaskableGraphic
	{
		Color color = graphic.color;
		color.a = alpha;
		graphic.color = color;
	}

	public static void SetAlpha (this Image image, float alpha)
	{
		SetAlpha<Image> (image, alpha);
	}

	public static void SetAlpha (this Text text, float alpha)
	{
		SetAlpha<Text> (text, alpha);
	}

	public static void SetAlpha (this RawImage image, float alpha)
	{
		SetAlpha<RawImage> (image, alpha);
	}
	#endregion
}
