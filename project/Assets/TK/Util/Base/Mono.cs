using UnityEngine;

namespace TK
{
	public class Mono : MonoBehaviour
	{
#pragma warning disable 0109

		private Animator _animator;
		public Animator animator { get { if (_animator == null) _animator = GetComponent<Animator> (); return _animator; } }

		private Animation _animation = null;
		public new Animation animation { get { if (_animation == null) _animation = GetComponent<Animation> (); return _animation; } }

		private AudioSource _audio = null;
		public new AudioSource audio { get { if (_audio == null) _audio = GetComponent<AudioSource> (); return _audio; } }

		private Camera _camera = null;
		public new Camera camera { get { if (_camera == null) _camera = GetComponent<Camera> (); return _camera; } }

		private Collider _collider = null;
		public new Collider collider { get { if (_collider == null) _collider = GetComponent<Collider> (); return _collider; } }

		private Collider2D _collider2D = null;
		public new Collider2D collider2D { get { if (_collider2D == null) _collider2D = GetComponent<Collider2D> (); return _collider2D; } }

		private ConstantForce _constantForce = null;
		public new ConstantForce constantForce { get { if (_constantForce == null) _constantForce = GetComponent<ConstantForce> (); return _constantForce; } }

		protected GameObject _gameObject = null;
		public new GameObject gameObject { get { if (_gameObject == null) _gameObject = base.gameObject; return _gameObject; } }

		private GUIElement _guiElement = null;
		public new GUIElement guiElement { get { if (_guiElement == null) _guiElement = GetComponent<GUIElement> (); return _guiElement; } }

#if !UNITY_2017_2_OR_NEWER
		private GUIText _guiText = null;
		public new GUIText guiText { get { if (_guiText == null) _guiText = GetComponent<GUIText> (); return _guiText; } }

		private GUITexture _guiTexture = null;
		public new GUITexture guiTexture { get { if (_guiTexture == null) _guiTexture = GetComponent<GUITexture> (); return _guiTexture; } }
#endif

		private HingeJoint _hingeJoint = null;
		public new HingeJoint hingeJoint { get { if (_hingeJoint == null) _hingeJoint = GetComponent<HingeJoint> (); return _hingeJoint; } }

		private Light _light = null;
		public new Light light { get { if (_light == null) _light = GetComponent<Light> (); return _light; } }

#if !UNITY_2017_1_OR_NEWER
	private NetworkView _networkView = null;
    public new NetworkView networkView { get { if (_networkView == null) _networkView = GetComponent<NetworkView>(); return _networkView; } }
#endif

		private ParticleSystem _particleSystem = null;
		public new ParticleSystem particleSystem { get { if (_particleSystem == null) _particleSystem = GetComponent<ParticleSystem> (); return _particleSystem; } }

		private Renderer _renderer = null;
		public new Renderer renderer { get { if (_renderer == null) _renderer = GetComponent<Renderer> (); return _renderer; } }

		private Rigidbody _rigidbody = null;
		public new Rigidbody rigidbody { get { if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody> (); return _rigidbody; } }

		private Rigidbody2D _rigidbody2D = null;
		public new Rigidbody2D rigidbody2D { get { if (_rigidbody2D == null) _rigidbody2D = GetComponent<Rigidbody2D> (); return _rigidbody2D; } }

		private Transform _transform = null;
		public new Transform transform { get { if (_transform == null) _transform = base.transform; return _transform; } }

		private RectTransform _rectTransform = null;
		public RectTransform rectTransform { get { if (_rectTransform == null) _rectTransform = transform as RectTransform; return _rectTransform; } }

		private CharacterController _characterController;
		public CharacterController characterController { get { if (_characterController == null) _characterController = GetComponent<CharacterController> (); return _characterController; } }

#pragma warning restore 0109
	}
}
