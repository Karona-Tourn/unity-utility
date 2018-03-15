using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TK.Task
{
	public abstract class Task : TaskBase
	{
		public sealed override void Add (TaskBase other)
		{
			throw new NotImplementedException ();
		}

		public sealed override void Clear ()
		{
			throw new NotImplementedException ();
		}
	}
}
