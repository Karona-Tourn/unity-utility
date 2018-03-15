using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TK.Util
{
	public class CoroutineRunner : MonoBehaviour
	{
		/// <summary>
		/// Store all running coroutines
		/// </summary>
		private static List<CoroutineRunner> runningCoroutines = new List<CoroutineRunner> ();

		public static void AbortAllRunningCoroutines ()
		{
			lock (runningCoroutines)
			{
				foreach (var c in runningCoroutines)
				{
					if (c)
					{
						c.Abort ();
					}
				}
				runningCoroutines.Clear ();
			}
		}

		private void Awake ()
		{
			runningCoroutines.Add (this);
		}

		private void OnDestroy ()
		{
			runningCoroutines.Remove (this);
		}

		public void Abort ()
		{
			enabled = false;
			Destroy (gameObject);
		}

		private IEnumerator Coro_Start (IEnumerator routine)
		{
			yield return StartCoroutine (routine);
			Destroy (gameObject);
		}

		public static CoroutineRunner Start (IEnumerator routine)
		{
			CoroutineRunner runner = new GameObject ("Coro >>>").AddComponent<CoroutineRunner> ();
			runner.StartCoroutine (routine);
			return runner;
		}
	}
}
