using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Assets.Blastproof.Scripts._Systems.Elements
{
    public abstract class TextInputElement : MonoBehaviour
    {
        /*
        A base UI element that has a text reference
        */
        private TMP_InputField _inputField;
        [BoxGroup("References"), ReadOnly, ShowInInspector]
        protected TMP_InputField ThisText
            => _inputField ?? (_inputField = GetComponent<TMP_InputField>());

        // ---- Method that sets the text of an UI.Text
        protected virtual void SetText(string text) { ThisText.text = text; }
    }

    
    
}

