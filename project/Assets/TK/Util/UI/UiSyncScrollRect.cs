using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiSyncScrollRect : UIBehaviour
{
	public class AdapterIterator
	{
		private Adapter m_Adapter = null;
		private int m_Index = -1;

		public GameObject Item
		{
			get
			{
				if (m_Index + 1 > m_Adapter.Count)
					return null;
				else
					return m_Adapter.GetItem (m_Index, m_Adapter.MainScroll.m_ScrollRect);
			}
		}

		public AdapterIterator(Adapter adapter)
		{
			this.m_Adapter = adapter;
		}

		public void First()
		{
			m_Index = 0;
		}

		public bool Next()
		{
			if (m_Index < m_Adapter.Count)
			{
				m_Index++;
				return m_Index < m_Adapter.Count;
			}
			return false;
		}
	}

	public abstract class Adapter : IDisposable
	{
		private AdapterIterator m_Iterator = null;
		private bool m_Disposed = false;

		public UiSyncScrollRect MainScroll{ get; set; }

		public abstract object this [int index]{ get; }

		public abstract int Count { get; }

		~Adapter ()
		{
			Dispose (false);
		}

		private void Dispose (bool disposing)
		{
			if (m_Disposed)
				return;

			if (disposing)
			{
				OnDisposing ();
			}

			m_Disposed = true;
		}

		public void Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		protected virtual void OnDisposing ()
		{
		}

		public AdapterIterator GetIterator()
		{
			if (m_Iterator == null)
				m_Iterator = new AdapterIterator (this);
			return m_Iterator;
		}

		public abstract GameObject GetItem (int index, ScrollRect scroll);

		public abstract void OnRefreshed (UnityAction onRefreshScrollRect);

		public abstract bool OnScrolledToEnd (bool isTop, UnityAction onRefreshScrollRect);

		protected void Log(object message)
		{
#if DEBUG_LOG
			Debug.Log (message);
#endif
		}

		protected void LogWarning (object message)
		{
#if DEBUG_LOG
			Debug.LogWarning (message);
#endif
		}
	}

	[SerializeField]
	private ScrollRect m_ScrollRect = null;
	[SerializeField]
	private float bottomEndDistance = 0;

	protected List<GameObject> m_Store = new List<GameObject> ();
	protected Adapter m_Adapter = null;

	public bool IsLoading { get; private set; }

	public int Count{ get { return m_Store.Count; } }

	public ScrollRect GetScrollRect ()
	{
		return m_ScrollRect;
	}

	public GameObject GetItem (int index)
	{
		return m_Store [index];
	}

	public void AddItem (GameObject newItem)
	{
		m_Store.Add (newItem);
	}

	public void SetListAdapter (Adapter adapter)
	{
		if (m_Adapter != null && m_Adapter is IDisposable)
		{
			((IDisposable)m_Adapter).Dispose ();
			m_Adapter = null;
		}

		m_Adapter = adapter;

		if (m_Adapter != null)
		{
			m_Adapter.GetIterator ().First ();
			m_Adapter.MainScroll = this;
		}
	}

	private void GetMoreItems ()
	{
		if (m_Adapter != null)
		{
			var iterator = m_Adapter.GetIterator ();
			do
			{
				GameObject newItem = iterator.Item;
				if (newItem)
				{
					m_Store.Add (newItem);
					if (newItem.transform.parent == null || !newItem.transform.parent.Equals (m_ScrollRect.content.transform))
					{
						newItem.transform.SetParent (m_ScrollRect.content, false);
						newItem.transform.SetAsLastSibling ();
					}
				}
			} while(iterator.Next ());
		}
		IsLoading = false;
	}

	public virtual void ClearAll ()
	{
		m_ScrollRect.StopMovement ();
		m_ScrollRect.verticalNormalizedPosition = 0;
		m_ScrollRect.content.anchoredPosition = Vector2.zero;
		for (int i = m_Store.Count - 1; i >= 0; i--)
			Destroy (m_Store [i]);
		m_Store.Clear ();
		if (m_Adapter != null)
			m_Adapter.GetIterator ().First ();
		IsLoading = false;
	}

	public void Cleanup ()
	{
		SetListAdapter (null);
		ClearAll ();
	}

	public void Refresh ()
	{
		if (!IsLoading)
		{
			ClearAll ();
			if (m_Adapter != null)
			{
				IsLoading = true;
				m_Adapter.OnRefreshed (GetMoreItems);
			}
		}
	}

	public void OnScrollValueChanged (Vector2 position)
	{
		if (!IsLoading)
		{
			if (m_Adapter != null)
			{
				float vPos = m_ScrollRect.verticalNormalizedPosition;
				if (vPos == 1 || vPos == 0 || vPos <= bottomEndDistance)
					if (m_Adapter.OnScrolledToEnd (vPos == 1, GetMoreItems))
						IsLoading = true;
			}
		}
	}

	public void LoadMoreOffset ()
	{
		if (!IsLoading)
		{
			if (m_Adapter != null)
			{
				IsLoading = true;
				m_Adapter.OnScrolledToEnd (false, GetMoreItems);
			}
		}
	}
}
