using UnityEngine;
using System;
using System.Reflection;

namespace TK
{
	public abstract class MonoSingleton<T> : Mono where T : MonoSingleton<T>
	{
		private static T instance = null;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = GameObject.FindObjectOfType<T> ();
					if (instance == null)
					{
						instance = new GameObject (typeof (T).Name, typeof (T)).GetComponent<T> ();
					}
				}
				return instance;
			}
		}

		public abstract bool DestroyOnLoad { get; }

		protected virtual void Awake ()
		{
			if (instance == null)
			{
				instance = this as T;
				if (DestroyOnLoad == false)
				{
					DontDestroyOnLoad (gameObject);
				}
			}
		}

		protected virtual void Start ()
		{
			if (instance != null && !instance.Equals (this))
			{
				Destroy (gameObject);
				return;
			}
		}
	}

}
