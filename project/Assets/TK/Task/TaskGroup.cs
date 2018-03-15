using System.Collections.Generic;
using System.Linq;

namespace TK.Task
{
	public class TaskGroup : TaskBase
	{
		private LinkedList<TaskBase> tasks = new LinkedList<TaskBase> ();
		private LinkedListNode<TaskBase> pendingTask = null;

		public override float Progress
		{
			get
			{
				if (tasks.Count == 0)
				{
					return 1f;
				}
				return tasks.Sum (t => t.Progress) / tasks.Count;
			}
		}

		public override void Add (TaskBase task)
		{
			tasks.AddLast (task);
		}

		public override void Clear ()
		{
			pendingTask = null;
			tasks.Clear ();
		}

		public override TaskStatus Update ()
		{
			TaskStatus status = TaskStatus.Pending;
			if (pendingTask != null)
			{
				status = pendingTask.Value.Update ();
				if (status == TaskStatus.Done)
				{
					pendingTask = pendingTask.Next;
					if (pendingTask != null)
					{
						pendingTask.Value.Reset ();
						status = TaskStatus.Pending;
					}
				}
			}
			return status;
		}

		public override void Reset ()
		{
			pendingTask = tasks.First;
			if (pendingTask != null)
			{
				pendingTask.Value.Reset ();
			}
		}
	}
}
