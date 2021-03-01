using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Blastproof.Systems.Core.Variables
{
    [CreateAssetMenu(menuName = "Blastproof/Variables/IntVariable")]
    public class IntVariable : ScriptableObject
    {
        public Action onValueChanged;
        
        [SerializeField] protected int val;
        [ShowInInspector, ReadOnly] public virtual int Value
        {
            get => val;
            set
            {
                UpdateBackingField(value);
                onValueChanged.Fire();
            }
        }
        public void Increment() { Value++; }
        public void Decrement() { Value--; }

        protected virtual void UpdateBackingField(int newValue)
        {
            val = newValue;
        }
    }
}