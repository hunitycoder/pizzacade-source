using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ToggleElement : MonoBehaviour
{
    // ---- A reference to the attached toggle component
    private Toggle _toggle;
    [BoxGroup("References"), ReadOnly, ShowInInspector] public Toggle ThisToggle => _toggle ?? (_toggle = GetComponent<Toggle>());

    // Method that sets the value
    protected void SetValue(bool value)
    {
        ThisToggle.isOn = value;
    }
}
