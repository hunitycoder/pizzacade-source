using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Blastproof.Systems.Elements
{
    [RequireComponent(typeof(Slider))]
    public abstract class SliderElement : MonoBehaviour
    {
        // ---- A reference to the attached slider component
        private Slider _slider;
        [BoxGroup("References"), ReadOnly, ShowInInspector] public Slider ThisSlider => _slider ?? (_slider = GetComponent<Slider>());

        // Method that sets the value
        protected void SetValue(float value)
        {
            ThisSlider.value = value;
        }
    }
}