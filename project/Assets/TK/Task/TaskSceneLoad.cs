using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace TK.Task
{
	public class TaskSceneLoadData
	{
		public string sceneName = "";
		public int sceneIndex = -1;
		public bool additive = false;
		public UnityAction<Scene, LoadSceneMode> onLoaded = null;
		public UnityAction<Scene> onUnloaded = null;
	}

	public class TaskSceneLoad : Task
	{
		private AsyncOperation op = null;
		private TaskSceneLoadData data = null;

		public override float Progress
		{
			get
			{
				return op == null ? 0f : op.progress;
			}
		}

		public TaskSceneLoad(TaskSceneLoadData data)
		{
			this.data = data;
		}

		public override TaskStatus Update ()
		{
			TaskStatus status = TaskStatus.Pending;
			if (op != null)
			{
				if (op.isDone)
				{
					status = TaskStatus.Done;
					SceneManager.sceneLoaded -= data.onLoaded;
					SceneManager.sceneUnloaded -= data.onUnloaded;
				}
			}
			return status;
		}

		public override void Reset ()
		{
			SceneManager.sceneLoaded += data.onLoaded;
			SceneManager.sceneUnloaded += data.onUnloaded;
			if (string.IsNullOrEmpty (data.sceneName))
			{
				op = SceneManager.LoadSceneAsync (data.sceneIndex, data.additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
			}
			else
			{
				op = SceneManager.LoadSceneAsync (data.sceneName, data.additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
			}
		}
	}
}
