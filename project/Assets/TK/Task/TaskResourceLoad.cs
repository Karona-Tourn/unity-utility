using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TK.Task
{
	public class TaskResourceLoadData
	{
		public Type loadType = null;
		public string name = "";
		public string path = "";
		public bool abortIfFailed = false;
		public UnityEngine.Object loadedAsset = null;
		public UnityAction<TaskResourceLoadData> onSuccess = null;
		public UnityAction<TaskResourceLoadData> onFail = null;
	}

	public class TaskResourceLoad : Task
	{
		private ResourceRequest req = null;
		private Queue<TaskResourceLoadData> queue = null;
		private TaskResourceLoadData pendingData = null;
		private float progress = 0f;
		private int count = 0;

		public override float Progress
		{
			get
			{
				float p = progress;
				if (req != null) { p += req.progress; }
				p /= count;
				return p;
			}
		}

		public TaskResourceLoad (TaskResourceLoadData data)
		{
			queue = new Queue<TaskResourceLoadData> ();
			queue.Enqueue (data);
			count = queue.Count;
		}

		public TaskResourceLoad (List<TaskResourceLoadData> dataList)
		{
			queue = new Queue<TaskResourceLoadData> (dataList);
			count = queue.Count;
		}

		public override TaskStatus Update ()
		{
			if (req == null)
			{
				if (queue.Count > 0)
				{
					pendingData = queue.Dequeue ();
					req = Resources.LoadAsync (System.IO.Path.Combine (pendingData.path, pendingData.name), pendingData.loadType);
				}
				else
				{
					return TaskStatus.Done;
				}
			}

			if (req.isDone)
			{
				var asset = req.asset;
				req = null;
				progress += 1f;

				if (asset == null)
				{
					if (pendingData.onFail != null)
					{
						pendingData.onFail (pendingData);
					}

					if (pendingData.abortIfFailed)
					{
						return TaskStatus.Abort;
					}
				}
				else
				{
					pendingData.loadedAsset = asset;
					if (pendingData.onSuccess != null)
					{
						pendingData.onSuccess (pendingData);
					}
				}

				pendingData = null;
			}

			return TaskStatus.Pending;
		}

		public override void Reset ()
		{
			pendingData = null;
			req = null;
		}
	}
}
