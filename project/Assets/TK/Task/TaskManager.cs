using System.Collections;
using UnityEngine;

namespace TK.Task
{
	public class TaskManager : MonoSingleton<TaskManager>, IEnumerator
	{
		private TaskGroup group = new TaskGroup ();

		public float Progress { get { return group.Progress; } }

		public bool IsDone { get { return Progress == 1; } }

		public object Current { get { return null; } }

		public override bool DestroyOnLoad
		{
			get
			{
				return false;
			}
		}

		public bool MoveNext ()
		{
			var status = group.Update ();
			switch (status)
			{
				case TaskStatus.Abort:
				case TaskStatus.Done:
					group.Clear ();
					return false;
				default:
					return true;
			}
		}

		public void Add (TaskBase task)
		{
			group.Add (task);
		}

		public void Reset ()
		{
			group.Reset ();
		}

		public void Execute ()
		{
			StartCoroutine (this);
		}

		public void Abort ()
		{
			StopCoroutine (this);
			group.Clear ();
		}
	}
}
