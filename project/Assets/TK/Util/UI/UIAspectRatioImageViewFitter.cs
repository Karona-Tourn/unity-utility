using UnityEngine;
using UnityEngine.UI;

namespace TK.UI
{

	[RequireComponent (typeof (RawImage))]
	public class UIAspectRatioImageViewFitter : MonoBehaviour
	{
		public enum AspectMode
		{
			FitInParent,
			EnvelopeParent
		}

		[SerializeField]
		private RectTransform parent = null;

		[SerializeField]
		private RawImage rawImage = null;

		[SerializeField]
		private AspectMode aspectMode = AspectMode.EnvelopeParent;

		[SerializeField, Tooltip ("Tell the fitter to update rect whenver the transform dimension is changed.")]
		private bool refresh = true;

		[SerializeField]
		private bool useAdvanceCalculation = true;

		private RawImage RawImage
		{
			get
			{
				if (rawImage == null) { rawImage = GetComponent<RawImage> (); }
				return rawImage;
			}
		}

		/// <summary>
		/// Tell whether has a texture assigned or not
		/// </summary>
		public bool HasTexture
		{
			get { return RawImage.texture != null; }
		}

		/// <summary>
		/// Get or set texture displayed
		/// </summary>
		public virtual Texture2D Texture
		{
			get { return HasTexture ? (Texture2D)RawImage.texture : null; }
			set
			{
				RawImage.enabled = (value != null);
				RawImage.texture = value;
				if (value)
				{
					UpdateRect ();
				}
			}
		}

		/// <summary>
		/// Get aspect ratio
		/// </summary>
		public float AspectRatio
		{
			get
			{
				var tex = RawImage.texture;
				if (tex == null)
				{
					return 0;
				}

				return ScriptUtil.CalAspectRatio (tex.width, tex.height);
			}
		}

		public void SetTextureWithoutFitting (Texture2D texture)
		{
			RawImage.enabled = texture != null;
			RawImage.texture = texture;
		}

		private Vector2 GetParentSize ()
		{
			RectTransform parent = this.parent;
			if (parent == null)
			{
				parent = transform.parent as RectTransform;
			}

			if (parent == null)
			{
				return Vector2.zero;
			}
			if ((int)transform.eulerAngles.z % 180 == 0)
			{
				return parent.rect.size;
			}
			else
			{
				return new Vector2 (parent.rect.size.y, parent.rect.size.x);
			}
		}

		private void OnRectTransformDimensionsChange ()
		{
			if (refresh && HasTexture)
			{
				UpdateRect ();
			}
		}

		private float GetSizeDeltaToProduceSize (float size, int axis)
		{
			var rectTransform = GetComponent<RectTransform> ();
			return (size - (this.GetParentSize ()[axis] * (rectTransform.anchorMax[axis] - rectTransform.anchorMin[axis])));
		}

		public void UpdateRect ()
		{
			if (useAdvanceCalculation)
			{
				UpdateAdvanceRect ();
			}
			else
			{
				UpdateBasicRect ();
			}
		}

		private void UpdateBasicRect ()
		{
			float aspectRatio = AspectRatio;
			var rectTransform = GetComponent<RectTransform> ();
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.anchoredPosition = Vector2.zero;
			Vector2 zero = Vector2.zero;
			Vector2 parentSize = this.GetParentSize ();
			if (!((ScriptUtil.CalWidthByAspectRatio (parentSize.y, aspectRatio) < parentSize.x) ^ (aspectMode == AspectMode.FitInParent)))
			{
				zero.x = this.GetSizeDeltaToProduceSize (ScriptUtil.CalWidthByAspectRatio (parentSize.y, aspectRatio), 0);
			}
			else
			{
				zero.y = this.GetSizeDeltaToProduceSize (ScriptUtil.CalHeightByAspectRatio (parentSize.x, aspectRatio), 1);
			}
			rectTransform.sizeDelta = zero;
		}

		public void UpdateAdvanceRect ()
		{
			float aspectRatio = AspectRatio;
			var rectTransform = GetComponent<RectTransform> ();
			rectTransform.anchorMin = Vector2.one * 0.5f;
			rectTransform.anchorMax = Vector2.one * 0.5f;
			rectTransform.anchoredPosition = Vector2.zero;
			Vector2 parentSize = this.GetParentSize ();
			Vector2 size = parentSize;
			if (!((ScriptUtil.CalWidthByAspectRatio (parentSize.y, aspectRatio) < parentSize.x) ^ (aspectMode == AspectMode.FitInParent)))
			{
				size.x = this.GetSizeDeltaToProduceSize (ScriptUtil.CalWidthByAspectRatio (parentSize.y, aspectRatio), 0);
			}
			else
			{
				size.y = this.GetSizeDeltaToProduceSize (ScriptUtil.CalHeightByAspectRatio (parentSize.x, aspectRatio), 1);
			}
			rectTransform.sizeDelta = size;
		}
	}

}