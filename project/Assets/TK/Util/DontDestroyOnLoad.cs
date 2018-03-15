using UnityEngine;

namespace TK
{

	public class DontDestroyOnLoad : MonoBehaviour
	{
		private void Awake ()
		{
			DontDestroyOnLoad (gameObject);
		}

		private void Start ()
		{
			Destroy (this);
		}
	}

}
