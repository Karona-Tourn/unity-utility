using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace TK.Task
{
	public class TaskAssetBundleLoadData
	{
		public string bundleName = "";
		public string url = "";
		public uint crc = 0;
		public uint version = 0;
		public string assetName = "";
		public Type assetType = null;
		public bool abortIfFailed = false;
		public string error = "";
		public UnityEngine.Object loadedAsset = null;
		public UnityAction<TaskAssetBundleLoadData> onSuccess = null;
		public UnityAction<TaskAssetBundleLoadData> onFail = null;
	}

	public class TaskAssetBundleCache
	{
		public string name = "";
		public uint version = 0;
		public AssetBundle bundle = null;

		private static Dictionary<string, TaskAssetBundleCache> caches = new Dictionary<string, TaskAssetBundleCache> ();

		public static TaskAssetBundleCache GetCachedBundle (string name, uint version)
		{
			lock (caches)
			{
				if (caches.ContainsKey (name + version))
				{
					return caches[name];
				}
				return null;
			}
		}

		public static TaskAssetBundleCache Cache (string name, AssetBundle bundle, uint version)
		{
			TaskAssetBundleCache newCache = null;
			lock (caches)
			{
				newCache = new TaskAssetBundleCache ()
				{
					name = name,
					bundle = bundle,
					version = version
				};
				caches.Add (name + version, newCache);
			}
			return newCache;
		}

		public static void Unload (string name, uint version, bool unloadAllLoadedObjects = false)
		{
			lock (caches)
			{
				string key = name + version;
				TaskAssetBundleCache cache = null;
				if (caches.TryGetValue (key, out cache))
				{
					cache.bundle.Unload (unloadAllLoadedObjects);
				}
			}
		}

		public static void UnloadAll (bool unloadAllLoadedObjects = false)
		{
			lock (caches)
			{
				foreach (var c in caches)
				{
					c.Value.bundle.Unload (unloadAllLoadedObjects);
				}
				caches.Clear ();
			}
		}
	}

	public class TaskAssetBundleLoad : Task
	{
		public enum State
		{
			NextReq,
			LoadBundle,
			LoadAsset,
		}

		private Queue<TaskAssetBundleLoadData> queue = null;
		private AsyncOperation ao = null;
		private TaskAssetBundleLoadData pendingData = null;
		private UnityWebRequest webReq = null;
		private AssetBundleRequest bundleReq = null;
		private float progress = 0f;
		private int count = 0;
		private State state = State.NextReq;
		private TaskAssetBundleCache cachedBundle = null;

		public override float Progress
		{
			get
			{
				float p = 0f;
				if (ao != null) { p += ao.progress; }
				if (bundleReq != null) { p += bundleReq.progress; }
				p /= 2f;
				return Mathf.Clamp01 ((progress + p) / count);
			}
		}

		public TaskAssetBundleLoad (TaskAssetBundleLoadData data)
		{
			queue = new Queue<TaskAssetBundleLoadData> ();
			queue.Enqueue (data);
			count = queue.Count;
		}

		public TaskAssetBundleLoad (List<TaskAssetBundleLoadData> dataList)
		{
			queue = new Queue<TaskAssetBundleLoadData> (dataList);
			count = queue.Count;
		}

		public override void Reset ()
		{
			state = State.NextReq;
			cachedBundle = null;
			pendingData = null;
			webReq = null;
			bundleReq = null;
			ao = null;
		}

		public override TaskStatus Update ()
		{
			switch (state)
			{
				case State.NextReq:
					if (queue.Count > 0)
					{
						pendingData = queue.Dequeue ();

						cachedBundle = TaskAssetBundleCache.GetCachedBundle (pendingData.bundleName, pendingData.version);

						if (cachedBundle == null)
						{
							webReq = UnityWebRequest.GetAssetBundle (pendingData.url, pendingData.version, pendingData.crc);
							ao = webReq.Send ();
							state = State.LoadBundle;
						}
						else
						{
							state = State.LoadAsset;
						}
					}
					else
					{
						return TaskStatus.Done;
					}
					break;
				case State.LoadBundle:
					// If loaded asset bundle
					if (ao.isDone)
					{
						if (webReq.isHttpError || webReq.isNetworkError)
						{
							var data = pendingData;
							progress += 1f;
							Reset ();
							data.error = webReq.error;
							if (data.onFail != null)
							{
								data.onFail (data);
							}

							if (data.abortIfFailed)
							{
								return TaskStatus.Abort;
							}
						}
						else
						{
							DownloadHandlerAssetBundle download = webReq.downloadHandler as DownloadHandlerAssetBundle;
							cachedBundle = TaskAssetBundleCache.Cache (pendingData.bundleName, download.assetBundle, pendingData.version);
							state = State.LoadAsset;
						}
					}
					break;
				case State.LoadAsset:
					if (bundleReq == null)
					{
						bundleReq = cachedBundle.bundle.LoadAssetAsync (pendingData.assetName, pendingData.assetType);
					}

					if (bundleReq.isDone)
					{
						var data = pendingData;
						progress += 1f;
						Reset ();
						data.loadedAsset = bundleReq.asset;
						if (data.onSuccess != null)
						{
							data.onSuccess (data);
						}
					}
					break;
			}

			return TaskStatus.Pending;
		}
	}
}
