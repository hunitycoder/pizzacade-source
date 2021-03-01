using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Blastproof.Tools.Elements
{
	/*
		A base UI element that has a raw image reference
	*/
	[RequireComponent(typeof(RawImage))]
	public abstract class RawImageElement : MonoBehaviour
	{
        // ----A reference to the attached raw image component
        [BoxGroup("References"), ReadOnly] private RawImage _image;
        protected RawImage ThisImage => _image ?? (_image = GetComponent<RawImage>());

		// ---- Method that sets the sprite of an UI.Image
		protected virtual void SetTexture(Texture2D texture) { ThisImage.texture = texture; }
	}
}