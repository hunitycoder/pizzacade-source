using Assets.Blastproof.Scripts._Systems.Elements;
using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Blastproof.Tools.Elements;
using UnityEngine;

public class UIText_BoolVariableDisplay : TextElement
{
    [SerializeField] private BoolVariable _variable;
    [SerializeField] private string _context;
    [SerializeField] private string _trueText;
    [SerializeField] private string _falseText;
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
        UpdateText(false);
    }

    // ---- Update interace on every change
    private void UpdateText(bool value)
    {
        ThisText.text = value ? _trueText : _falseText;
        //if (_context.EmptyOrNull())
        //    ThisText.text = _variable.Value;
        //else
        //    ThisText.text = string.Format(_context, _variable.Value);
    }
}
