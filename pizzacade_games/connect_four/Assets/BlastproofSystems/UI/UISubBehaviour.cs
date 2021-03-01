using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.UI
{
    public class UISubBehaviour : MonoBehaviour
    {
        [BoxGroup("MonoBehaviour"), SerializeField] private GameObject content;

        [BoxGroup("References"), SerializeField] protected UISystem uiSystem;
        [BoxGroup("References"), SerializeField] protected List<UISubState> activeSubStates;

        [BoxGroup("Info"), SerializeField, ReadOnly] protected bool isOpen;

        protected virtual void OnEnable()
        {
            //uiSystem.onSubStateChanged += UISystem_OnSubStateChanged;
            foreach(var ss in activeSubStates) { ss.Register(this); }
        }

        protected virtual void OnDisable()
        {
            //uiSystem.onSubStateChanged -= UISystem_OnSubStateChanged;
            foreach (var ss in activeSubStates) { ss.UnRegister(this); }
        }

        /*
        protected virtual void UISystem_OnSubStateChanged(UISubState subState)
        {
            var shouldOpen = activeSubStates.Contains(subState);
            if (shouldOpen)
                ActivateContent();
            else DeactivateContent();

            if (!isOpen && shouldOpen)
                OnOpened();
            else if (isOpen && !shouldOpen) OnClosed();
        }
        */

        private void ActivateContent() { content.gameObject.SetActive(true); }
        private void DeactivateContent() { content.gameObject.SetActive(false); }

        protected virtual void OnOpened() { isOpen = true; }
        protected virtual void OnClosed()  { isOpen = false; }
    }
}