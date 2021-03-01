using Assets.Blastproof.Scripts._Systems.Elements;
using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Blastproof.Tools.Elements;
using UnityEngine;

public class UIText_StringVariableDisplay : TextElement
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
            ThisText.text = !_variable.Value.EmptyOrNull() ? _variable.Value.Replace("\n", System.Environment.NewLine) : "";
        else
            ThisText.text = string.Format(_context, _variable.Value).Replace("\\n", System.Environment.NewLine);

        string aa = ThisText.text;
        if (aa.Length > 6 && (aa.Contains("Friend")== true || aa.Contains("Random")==true))
        {
            if (aa.Contains("Friend"))
            {
                ThisText.text = "ROOM: " + aa.Substring(6);
            }
            else
            {
                ThisText.text = "Random Room";
            }
        }
    }
}
