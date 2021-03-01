using Blastproof.Systems.Core.Variables;
using Blastproof.Systems.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.Utility
{
    public class BoolVariableActivator : MonoBehaviour
    {
        [BoxGroup("References"), SerializeField] private BoolVariable _variable;
        [BoxGroup("References"), SerializeField] private GameObject[] _activeObjects;
        [BoxGroup("References"), SerializeField] private GameObject[] _inactiveObjects;

        private void OnEnable()
        {
            ToggleObjectActivation(_variable.Value);
            _variable.onValueChanged += ToggleObjectActivation;
        }

        private void OnDisable()
        {
            _variable.onValueChanged -= ToggleObjectActivation;
        }

        private void ToggleObjectActivation(bool value)
        {
            foreach (var obj in _activeObjects)
                obj.SetActive(value);
            foreach (var obj in _inactiveObjects)
                obj.SetActive(!value);

            EnsureStateActivation();
        }

        private void EnsureStateActivation()
        {
            MEC.Timing.CallDelayed(.01f, () =>
            {
                var parentBehaviour = GetComponentInParent<UIBehaviour>();
                parentBehaviour.currentActiveState.Activate();
            });
        }
    }
}