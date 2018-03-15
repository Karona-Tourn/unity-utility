using System.Collections;

namespace TK.Task
{
	public class TaskCoroutine : Task
	{
		private IEnumerator routine = null;
		private float progress = 0f;

		public override float Progress
		{
			get
			{
				return progress;
			}
		}

		public TaskCoroutine (IEnumerator routine)
		{
			this.routine = routine;
		}

		public override void Reset ()
		{
			routine.Reset ();
			progress = 0f;
		}

		public override TaskStatus Update ()
		{
			var hasNext = routine.MoveNext ();
			if (!hasNext) { progress = 1f; }
			return hasNext ? TaskStatus.Pending : TaskStatus.Done;
		}
	}
}
