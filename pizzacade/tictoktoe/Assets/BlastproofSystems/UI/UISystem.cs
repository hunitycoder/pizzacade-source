using System;
using System.Linq;
using Blastproof.Systems.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.UI
{ 
    [CreateAssetMenu(menuName = "Blastproof/Systems/UI/UISystem")]
    public class UISystem : BlastproofSystem
    {
        [BoxGroup("Info"), ShowInInspector, ReadOnly] public UIState CurrentState;

        [HideInInspector] public Action<UIState> onStateChanged;

        [BoxGroup("Info"), ShowInInspector, ReadOnly] private UIState lockState;
        [BoxGroup("Info"), ShowInInspector, ReadOnly] private UIState memorizedState;

        public override void Initialize()
        {
            base.Initialize();
            var referencer = FindObjectOfType<ScriptableObjectReferencer>();
            var allSubStates = referencer.scriptableObjects.FindAll(x => x is UISubState).Cast<UISubState>();
            foreach (var ss in allSubStates) ss.Reset();

            onSystemInitialized.Fire();
        }

        [Button]
        public void ChangeState(UIState newState)
        {
            Debug.Log("Activating state: " + newState);
            if (newState.isPopUp)
                memorizedState = CurrentState;

            if (!newState.isPopUp && memorizedState != null)
            {
                var cacheState = memorizedState;
                memorizedState = null;
                ChangeState(cacheState);
                return;
            }
            
            if (lockState == null || lockState == newState)
            {
                CurrentState = newState;
                onStateChanged.Fire(CurrentState);
            }
        }

        public void ResetMemorizedState()
        {
            memorizedState = null;
        }

        [Button]
        public override void Reset()
        {
            base.Reset();
            CurrentState = null;
            ResetMemorizedState();
        }

        public void LockStateFor(UIState state)
        {
            lockState = state;
        }
    }
}