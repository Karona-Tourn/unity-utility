using UnityEngine;
using UnityEngine.UI;

namespace PM
{

	[AddComponentMenu ("UI/Effects/Gradient")]
	public class GradientV1 : BaseMeshEffect
	{
		[SerializeField]
		private Color32 topColor = Color.white;
		[SerializeField]
		private Color32 bottomColor = Color.black;

		public Color32 TopColor
		{
			get { return topColor; }
			set { topColor = value; }
		}

		public Color32 BottomColor
		{
			get { return bottomColor; }
			set { bottomColor = value; }
		}

		public override void ModifyMesh (VertexHelper vh)
		{
			if (!IsActive () || vh.currentVertCount == 0)
			{
				return;
			}

			int count = vh.currentVertCount;
			UIVertex v = new UIVertex ();
			vh.PopulateUIVertex (ref v, 0);
			float bottomY = v.position.y;
			float topY = v.position.y;

			for (int i = 1; i < count; i++)
			{
				vh.PopulateUIVertex (ref v, i);
				float y = v.position.y;
				if (y > topY)
				{
					topY = y;
				}
				else if (y < bottomY)
				{
					bottomY = y;
				}
			}

			float uiElementHeight = topY - bottomY;

			for (int i = 0; i < count; i++)
			{
				vh.PopulateUIVertex (ref v, i);
				v.color = Color32.Lerp (bottomColor, topColor, (v.position.y - bottomY) / uiElementHeight);
				vh.SetUIVertex (v, i);
			}
		}
	}
}
