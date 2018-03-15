using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;
using System.Linq;

public class EditorBase<T> : Editor where T : MonoBehaviour
{
	private GUIStyle m_PreviewLabelStyle = null;

	protected GUIStyle previewLabelStyle
	{
		get
		{
			if (this.m_PreviewLabelStyle == null)
			{
				GUIStyle style = new GUIStyle ("PreOverlayLabel") {
					richText = true,
					alignment = TextAnchor.UpperLeft,
					fontStyle = FontStyle.Normal
				};
				this.m_PreviewLabelStyle = style;
			}
			return this.m_PreviewLabelStyle;
		}
	}

	protected T Target { get; private set; }

	/// <summary>
	/// Gets the serialized properties.
	/// </summary>
	/// <returns>The serialized properties.</returns>
	protected SerializedProperty[] GetSerializedProperties ()
	{
		Type type = typeof(T);
		var fields = type.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where (f =>
		{
			bool isSerialized = false;
			if (f.IsPublic)
			{
				isSerialized = f.GetCustomAttributes (false).Where (a => a is HideInInspector).Count () == 0;
			}
			else
			{
				isSerialized = f.GetCustomAttributes (false).Where (a => a is SerializeField).Count () > 0;
			}
			return isSerialized;
		}).Select (f => serializedObject.FindProperty (f.Name)).ToArray ();
		return fields;
	}

	protected void DrawPropertyFields (bool disabled, params SerializedProperty[] props)
	{
		EditorGUI.BeginDisabledGroup (disabled);
		int length = props.Length;
		for (int i = 0; i < length; i++)
		{
			EditorGUILayout.PropertyField (props[i], true);
		}
		EditorGUI.EndDisabledGroup ();
	}
	
	protected virtual void OnEnable()
	{
		Target = target as T;
	}

	protected virtual void OnSceneGUI()
	{
	}
}
