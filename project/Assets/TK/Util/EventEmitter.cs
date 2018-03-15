using System.Collections.Generic;
using System;

namespace TK.Events
{
	public class EventEmitter
	{
		private static EventEmitter instance = new EventEmitter ();

		private SortedDictionary<object, Delegate> listeners = new SortedDictionary<object, Delegate> ();

		private EventEmitter ()
		{
		}

		/// <summary>
		/// Clear all listeners
		/// </summary>
		public static void ClearAllListeners ()
		{
			instance.listeners.Clear ();
		}

		public static void ClearListeners (object key)
		{
			if (instance.listeners.ContainsKey (key))
			{
				instance.listeners[key] = null;
			}
		}

		private void InternalAddListener (object key, Delegate listener)
		{
			if (!listeners.ContainsKey (key))
			{
				listeners.Add (key, null);
			}
			listeners[key] = Delegate.Combine (listeners[key], listener);
		}

		private void InternalBroadcast (object key, params object[] args)
		{
			Delegate listener = null;
			if (listeners.TryGetValue (key, out listener))
			{
				if (listener == null)
				{
					return;
				}

				listener.DynamicInvoke (args);
			}
		}

		private void InternalRemoveListener (object key, Delegate listener)
		{
			Delegate sources = null;
			if (listeners.TryGetValue (key, out sources))
			{
				listeners[key] = Delegate.Remove (sources, listener);
			}
		}

		#region Add listeners
		public static void AddListener (object key, Act listener)
		{
			instance.InternalAddListener (key, listener);
		}

		public static void AddListener<T> (object key, Act<T> listener)
		{
			instance.InternalAddListener (key, listener);
		}

		public static void AddListener<T1, T2> (object key, Act<T1, T2> listener)
		{
			instance.InternalAddListener (key, listener);
		}

		public static void AddListener<T1, T2, T3> (object key, Act<T1, T2, T3> listener)
		{
			instance.InternalAddListener (key, listener);
		}

		public static void AddListener<T1, T2, T3, T4> (object key, Act<T1, T2, T3, T4> listener)
		{
			instance.InternalAddListener (key, listener);
		}
		#endregion

		#region Broadcast listeners
		public static void Broadcast (object key)
		{
			instance.InternalBroadcast (key);
		}

		public static void Broadcast<T> (object key, T arg)
		{
			instance.InternalBroadcast (key, arg);
		}

		public static void Broadcast<T1, T2> (object key, T1 arg1, T2 arg2)
		{
			instance.InternalBroadcast (key, arg1, arg2);
		}

		public static void Broadcast<T1, T2, T3> (object key, T1 arg1, T2 arg2, T3 arg3)
		{
			instance.InternalBroadcast (key, arg1, arg2, arg3);
		}

		public static void Broadcast<T1, T2, T3, T4> (object key, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			instance.InternalBroadcast (key, arg1, arg2, arg3, arg4);
		}
		#endregion

		#region Remove listeners
		public static void RemoveListener (object key, Act listener)
		{
			instance.InternalRemoveListener (key, listener);
		}

		public static void RemoveListener<T> (object key, Act<T> listener)
		{
			instance.InternalRemoveListener (key, listener);
		}

		public static void RemoveListener<T1, T2> (object key, Act<T1, T2> listener)
		{
			instance.InternalRemoveListener (key, listener);
		}

		public static void RemoveListener<T1, T2, T3> (object key, Act<T1, T2, T3> listener)
		{
			instance.InternalRemoveListener (key, listener);
		}

		public static void RemoveListener<T1, T2, T3, T4> (object key, Act<T1, T2, T3, T4> listener)
		{
			instance.InternalRemoveListener (key, listener);
		}
		#endregion
	}
}