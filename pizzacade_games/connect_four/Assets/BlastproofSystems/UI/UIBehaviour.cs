using System;
using System.Collections.Generic;
using System.Linq;
using Blastproof.Systems.Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.UI
{
    public class UIBehaviour : MonoBehaviour
    {
        [BoxGroup("MonoBehaviour"), SerializeField] private GameObject content;
        [BoxGroup("MonoBehaviour"), SerializeField] private bool keepActive;
        [BoxGroup("MonoBehaviour"), SerializeField] private Transform activePoint;
        [BoxGroup("MenuBehaviour"), SerializeField] private Transform inactivePoint;
        [BoxGroup("MenuBehaviour"), SerializeField] protected float time;

        [BoxGroup("References"), SerializeField] protected UISystem uiSystem;
        [BoxGroup("References"), SerializeField] protected List<UIState> activeStates;

        [BoxGroup("Info"), SerializeField, ReadOnly] public UIState currentActiveState;
        [BoxGroup("Info"), ShowInInspector, ReadOnly] private List<UIBehaviour_Component> comps = new List<UIBehaviour_Component>();
        
        [BoxGroup("Info"), SerializeField, ReadOnly] protected bool isOpen;

        [HideInInspector] public Action onOpen;
        [HideInInspector] public Action onClose;

        protected virtual void OnEnable()
        {
            comps = GetComponentsInChildren<UIBehaviour_Component>(true).ToList();
            
            uiSystem.onStateChanged += UISystem_OnStateChanged;
        }

        protected virtual void OnDisable()
        {
            uiSystem.onStateChanged -= UISystem_OnStateChanged;
        }

        protected virtual void UISystem_OnStateChanged(UIState state)
        {
            if (state.isPopUp && !activeStates.Contains(state)) return;
            
            var shouldOpen = activeStates.Contains(state);
            if(shouldOpen)
                ActivateContent(state);
            else DeactivateContent();

            if (!isOpen && shouldOpen)
            {
                OnOpened();
                currentActiveState = state;
            }
            else if (isOpen && !shouldOpen)
            {
                OnClosed();
                currentActiveState = null;
            }
        }

        private void ActivateContent(UIState state)
        {
            // Handle (or not) animation
            if (isOpen || inactivePoint == null || activePoint == null)
            {
                if(!keepActive) content.gameObject.SetActive(true);
            }
            else
            {
                content.transform.position = inactivePoint.position;
                if (!keepActive) content.gameObject.SetActive(true);
                var mover = content.transform.DOMove(activePoint.position, time).SetEase(Ease.Linear);
                mover.SetAutoKill();
                mover.onComplete += () =>
                {
                    content.transform.position = activePoint.position;
                };
            }

            MEC.Timing.CallDelayed(.1f, () =>
            {
                foreach (var comp in comps)
                {
                    comp.Activated();
                }
            });
        }

        private void DeactivateContent()
        {
            if (inactivePoint == null || activePoint == null)
            { if (!keepActive) content.gameObject.SetActive(false); return; }

            content.transform.position = activePoint.position;
            var mover = content.transform.DOMove(inactivePoint.position, time).SetEase(Ease.Linear);
            mover.SetAutoKill();
            mover.onComplete += () =>
            {
                if (!isOpen)
                {
                    content.transform.position = activePoint.position;
                    if (!keepActive) content.gameObject.SetActive(false);
                }
            };
            foreach (var comp in comps)
            {
                comp.Deactivated();
            }
        }

        protected virtual void OnOpened()
        {
            isOpen = true;
            onOpen.Fire();
        }
        
        protected virtual void OnClosed()
        {
            isOpen = false;
            onClose.Fire();
        }
    }
}