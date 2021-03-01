using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blastproof.Systems.Elements
{
    public class BetterToggle : SliderElement, IPointerDownHandler
    {
        [BoxGroup("References"), SerializeField] private Image background;

        [BoxGroup("Values"), SerializeField] private Color activeColor;
        [BoxGroup("Values"), SerializeField] private Color inactiveColor;

        private void OnEnable()
        {
            ThisSlider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            ThisSlider.onValueChanged.RemoveListener(OnValueChanged);
        }

        protected virtual void OnValueChanged(float value)
        {
            if (Mathf.RoundToInt(value) == 0)
            {
                OnTurnOff();
            }
            else OnTurnOn();
        }

        protected virtual void OnTurnOn()
        {
            background.color = activeColor;
        }

        protected virtual void OnTurnOff()
        {
            background.color = inactiveColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (Mathf.RoundToInt(ThisSlider.value) == 0)
            {
                SetValue(1);
            }
            else SetValue(0);
        }
    }
}