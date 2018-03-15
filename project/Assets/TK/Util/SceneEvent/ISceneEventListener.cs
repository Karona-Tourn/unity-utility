using UnityEngine.SceneManagement;

namespace TK
{

	public interface ISceneEventListener
	{
		void ActiveSceneChanged (Scene activeScene, Scene deactiveScene);
		void SceneLoaded (Scene loadedScene, LoadSceneMode loadMode);
		void SceneUnloaded (Scene unloadedScene);
	}

}
