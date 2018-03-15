using UnityEngine.Events;

namespace TK.Events
{
	public class UE<T> : UnityEvent<T> { }
	public class UE<T1, T2> : UnityEvent<T1, T2> { }
	public class UE<T1, T2, T3> : UnityEvent<T1, T2, T3> { }
	public class UE<T1, T2, T3, T4> : UnityEvent<T1, T2, T3, T4> { }

	public delegate void Act ();
	public delegate void Act<T> (T arg);
	public delegate void Act<T0, T1> (T0 arg0, T1 arg1);
	public delegate void Act<T0, T1, T2> (T0 arg0, T1 arg1, T2 arg2);
	public delegate void Act<T0, T1, T2, T3> (T0 arg0, T1 arg1, T2 arg2, T3 arg3);
	public delegate void Act<T0, T1, T2, T3, T4> (T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4);
	public delegate void Act<T0, T1, T2, T3, T4, T5> (T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
	public delegate void Act<T0, T1, T2, T3, T4, T5, T6> (T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
}
