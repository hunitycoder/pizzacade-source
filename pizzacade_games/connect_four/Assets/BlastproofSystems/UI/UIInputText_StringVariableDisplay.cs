using Assets.Blastproof.Scripts._Systems.Elements;
using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using UnityEngine;

public class UIInputText_StringVariableDisplay : TextInputElement
{
    [SerializeField] private StringVariable _variable;
    [SerializeField] private string _context;

    // ---- Subscribe/ Unsubscribe to changes
    private void OnEnable()
    {
        _variable.onValueChanged += UpdateText;
    }

    private void OnDisable()
    {
        _variable.onValueChanged -= UpdateText;
    }

    private void Start()
    {
        UpdateText();
    }

    // ---- Update interace on every change
    private void UpdateText()
    {
        if (_context.EmptyOrNull())
            ThisText.text = _variable.Value;
        else
            ThisText.text = string.Format(_context, _variable.Value);
    }
}
