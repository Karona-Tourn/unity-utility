using UnityEngine;
using UnityEngine.EventSystems;

namespace TK
{
	public class UiMono : UIBehaviour
	{
#pragma warning disable 0109
		private Animator _animator = null;
		public Animator animator { get { if (_animator == null) _animator = GetComponent<Animator> (); return _animator; } }

		private Animation _animation = null;
		public new Animation animation { get { if (_animation == null) _animation = GetComponent<Animation> (); return _animation; } }

		private AudioSource _audio = null;
		public new AudioSource audio { get { if (_audio == null) _audio = GetComponent<AudioSource> (); return _audio; } }

		private GameObject _gameObject = null;
		public new GameObject gameObject { get { if (_gameObject == null) _gameObject = base.gameObject; return _gameObject; } }

		private Transform _transform = null;
		public new Transform transform { get { if (_transform == null) _transform = base.transform; return _transform; } }

		private RectTransform _rectTransform = null;
		public RectTransform rectTransform { get { if (_rectTransform == null) _rectTransform = transform as RectTransform; return _rectTransform; } }
#pragma warning restore 0109
	}
}
