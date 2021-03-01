using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Blastproof.Tools.Elements
{
	/*
		A base UI element that has a button reference
	*/
	[RequireComponent(typeof(Image))]
	public abstract class ImageElement : MonoBehaviour
	{
        // ---- A reference to the attached image component
        private Image _image;
        [BoxGroup("References"), ReadOnly, ShowInInspector] public Image ThisImage => _image ?? (_image = GetComponentInChildren<Image>(true));

        // Method that sets the sprite of an UI.Image
        public virtual void SetImage(Sprite sprite) { ThisImage.sprite = sprite; }
	}
}