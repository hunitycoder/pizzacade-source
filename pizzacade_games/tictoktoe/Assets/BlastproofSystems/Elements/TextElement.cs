using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blastproof.Tools.Elements
{
	/*
		A base UI element that has a text reference
	*/
	public abstract class TextElement : MonoBehaviour
	{
        // ---- A reference to the attached text component
        private TextMeshProUGUI _text;
        [BoxGroup("References"), ReadOnly, ShowInInspector] protected TextMeshProUGUI ThisText 
            => _text ?? (_text = GetComponent<TextMeshProUGUI>());

		// ---- Method that sets the text of an UI.Text
		protected virtual void SetText(string text) { ThisText.text = text; }
	}
}