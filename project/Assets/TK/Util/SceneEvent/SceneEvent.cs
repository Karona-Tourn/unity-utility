using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TK
{

	public static class SceneEvent
	{
		public class ActivedSceneChangedEvent : UnityEvent<Scene, Scene> { }
		public class SceneLoadedEvent : UnityEvent<Scene, LoadSceneMode> { }
		public class SceneUnloadedEvent : UnityEvent<Scene> { }

		public static ActivedSceneChangedEvent activeSceneChanged = new ActivedSceneChangedEvent ();
		public static SceneLoadedEvent sceneLoaded = new SceneLoadedEvent ();
		public static SceneUnloadedEvent sceneUnloaded = new SceneUnloadedEvent ();

		static SceneEvent ()
		{
			if (Application.isPlaying)
			{
				SceneManager.activeSceneChanged += (arg0, arg1) => activeSceneChanged.Invoke (arg0, arg1);
				SceneManager.sceneLoaded += (scene, loadMode) => sceneLoaded.Invoke (scene, loadMode);
				SceneManager.sceneUnloaded += (scene) => sceneUnloaded.Invoke (scene);
			}
		}

		public static void RegisterListener (MonoBehaviour mono)
		{
			if (mono is ISceneEventListener)
			{
				ISceneEventListener handler = mono as ISceneEventListener;
				activeSceneChanged.AddListener (handler.ActiveSceneChanged);
				sceneLoaded.AddListener (handler.SceneLoaded);
				sceneUnloaded.AddListener (handler.SceneUnloaded);
			}
		}

		public static void UnregisterListener (MonoBehaviour mono)
		{
			if (mono is ISceneEventListener)
			{
				ISceneEventListener handler = mono as ISceneEventListener;
				activeSceneChanged.RemoveListener (handler.ActiveSceneChanged);
				sceneLoaded.RemoveListener (handler.SceneLoaded);
				sceneUnloaded.RemoveListener (handler.SceneUnloaded);
			}
		}
	}

}
