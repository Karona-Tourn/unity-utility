using UnityEngine;

namespace TK
{
	public class EndlessRotate : Mono
	{
		[SerializeField]
		private Transform target = null;
		[SerializeField]
		private Vector3 targetPoint = Vector3.zero;
		[SerializeField]
		private Vector3 axis = Vector3.up;
		[SerializeField]
		private float rotateAngle = 20f;

		private Vector3 point = Vector3.zero;

		private void Update ()
		{
			if (target)
				point = target.position;
			else
				point = targetPoint;
			transform.RotateAround (point, axis, rotateAngle * Time.deltaTime);
		}
	}
}
