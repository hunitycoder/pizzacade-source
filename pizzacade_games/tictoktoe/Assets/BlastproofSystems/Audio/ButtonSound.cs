using System.Collections;
using System.Collections.Generic;
using Blastproof.Tools.Elements;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.Audio
{
    public class ButtonSound : ButtonElement
    {
        [BoxGroup("References"), SerializeField] private AudioSystem audioSystem;

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            audioSystem.PlaySoundData(audioSystem.audioData.ButtonClick);
        }
    }
}
