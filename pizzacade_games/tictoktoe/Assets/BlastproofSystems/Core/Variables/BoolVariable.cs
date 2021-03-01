using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Blastproof.Systems.Core.Variables
{
    [CreateAssetMenu(menuName = "Blastproof/Variables/BoolVariable")]
    public class BoolVariable : ScriptableObject
    {
        public Action<bool> onValueChanged;

        protected bool val;
        [BoxGroup("Properties"), ShowInInspector]
        public virtual bool Value
        {
            get => val;
            set
            {
                if(val != value)
                {
                    val = value;
                    onValueChanged.Fire(val);
                }
            }
        }
    }
}