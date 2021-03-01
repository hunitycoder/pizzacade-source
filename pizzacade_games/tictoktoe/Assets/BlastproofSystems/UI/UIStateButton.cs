using Blastproof.Systems.UI;
using Blastproof.Tools.Elements;
using UnityEngine;

namespace Blastproof.Systems.UI
{ 
    public class UIStateButton : ButtonElement
    {
        [SerializeField] private UIState _state;

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            _state.Activate();
        }
    }
}