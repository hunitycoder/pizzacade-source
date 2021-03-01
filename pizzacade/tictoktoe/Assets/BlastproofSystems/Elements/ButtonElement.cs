using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Blastproof.Tools.Elements
{
	/*
		A base UI element that has a button reference
	*/
	[RequireComponent(typeof(Button))]
	public abstract class ButtonElement : MonoBehaviour
	{
        // ---- A reference to the attached button component
        private Button _button;
        [BoxGroup("References"), ReadOnly, ShowInInspector] public Button ThisButton => _button ?? (_button = GetComponentInChildren<Button>(true));

        protected virtual void OnEnable() { SubscribeToClick(); }
        protected virtual void OnDisable() { UnsubscribeToClick(); }

        protected virtual void OnButtonClick()
        {

        }

		// ---- Subscribe to the click event via an action
		private void SubscribeToClick() { ThisButton.onClick.AddListener(OnButtonClick); }
		private void UnsubscribeToClick() { ThisButton.onClick.RemoveListener(OnButtonClick); }
	}
}