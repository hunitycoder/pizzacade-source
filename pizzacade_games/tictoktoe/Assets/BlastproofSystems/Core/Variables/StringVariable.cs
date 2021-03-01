using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Blastproof.Systems.Core.Variables
{
    [CreateAssetMenu(menuName = "Blastproof/Variables/StringVariable")]
    public class StringVariable : ScriptableObject
    {
        public Action onValueChanged;
        
        protected string val;
        [ShowInInspector] public virtual string Value
        {
            get => val;
            set
            {
                this.val = value;
                onValueChanged.Fire();
            }
        }
    }
}