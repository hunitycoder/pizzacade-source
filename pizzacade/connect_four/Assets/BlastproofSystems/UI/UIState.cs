using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Blastproof.Systems.UI
{
    [CreateAssetMenu(menuName = "Blastproof/Systems/UI/UIState")]
    public class UIState : ScriptableObject
    {
        [BoxGroup("System"), SerializeField] private UISystem _system;

        [BoxGroup("Info")] public bool isPopUp = false;

        [BoxGroup("Sub - states")] public List<UISubState> subStates;

        [Button]
        public void Activate()
        {
            _system.ChangeState(this);
        }
    }
}