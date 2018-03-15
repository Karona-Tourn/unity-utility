using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor (typeof(SiblingOrder))]
public class SiblingOrderEditor : Editor
{
	private SerializedProperty propOrder = null;
	private SerializedProperty propIndex = null;
	private SerializedProperty propTarget = null;
	private SerializedProperty propOrderCase = null;

	private void OnEnable ()
	{
		propTarget = serializedObject.FindProperty ("target");
		propOrder = serializedObject.FindProperty ("order");
		propIndex = serializedObject.FindProperty ("index");
		propOrderCase = serializedObject.FindProperty ("orderCase");
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		EditorGUILayout.PropertyField (propTarget);
		EditorGUILayout.PropertyField (propOrder);
		EditorGUILayout.PropertyField (propOrderCase);

		if (propOrder.enumValueIndex == 2)
		{
			EditorGUILayout.PropertyField (propIndex);
		}

		serializedObject.ApplyModifiedProperties ();
	}
}

#endif

/// <summary>
/// Control setting Sibling order.
/// </summary>
public class SiblingOrder : MonoBehaviour
{
	public enum Order
	{
		First,
		Last,
		Index,
	}

	public enum OrderCase
	{
		OnEnable,
		OnDisable
	}

	[SerializeField, Tooltip("Target to order sibling")]
	private Transform target = null;

	/// <summary>
	/// The order of this sibling that can be first, last or at any index.
	/// </summary>
	[SerializeField]
	private Order order = default (Order);

	[SerializeField]
	private OrderCase orderCase = default (OrderCase);

	/// <summary>
	/// The index of sibling. Set it when you chose Index order.
	/// </summary>
	[SerializeField]
	private int index = 0;

	private Transform GetTarget ()
	{
		if (target)
		{
			return target;
		}
		return transform;
	}

	private void OnEnable ()
	{
		if (orderCase == OrderCase.OnEnable)
		{
			Reorder ();
		}
	}

	private void OnDisable ()
	{
		if (orderCase == OrderCase.OnDisable)
		{
			Reorder ();
		}
	}

	protected void Reorder ()
	{
		switch (order)
		{
			case Order.First:
				GetTarget ().SetAsFirstSibling ();
				break;
			case Order.Last:
				GetTarget ().SetAsLastSibling ();
				break;
			case Order.Index:
				GetTarget ().SetSiblingIndex (index);
				break;
		}
	}
}
