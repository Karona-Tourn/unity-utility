using UnityEngine;

namespace TK
{
	public abstract class UiMonoSingleton<T> : UiMono where T : UiMonoSingleton<T>
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
						new GameObject (typeof (T).Name, typeof (T));
					}
				}
				return instance;
			}
		}

		public abstract bool DestroyOnLoad { get; }

		protected override void Awake ()
		{
			if (instance == null)
			{
				instance = this as T;
				if (DestroyOnLoad == false)
				{
					DontDestroyOnLoad (gameObject);
				}
			}
			base.Awake ();
		}

		protected override void Start ()
		{
			if (instance != null && !instance.Equals (this))
			{
				Destroy (gameObject);
				return;
			}
			base.Start ();
		}
	}

}
