using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TK.Task
{
	public abstract class TaskBase
	{
		public abstract float Progress { get; }

		public virtual void Reset ()
		{
		}

		public abstract TaskStatus Update ();

		public abstract void Add (TaskBase other);

		public abstract void Clear ();
	}
}
