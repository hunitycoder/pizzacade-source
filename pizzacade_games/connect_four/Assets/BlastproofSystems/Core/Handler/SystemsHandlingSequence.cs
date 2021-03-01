using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Blastproof.Systems.Core.Handler
{
    [CreateAssetMenu(menuName = "Blastproof/Systems/Core/Handler/Sequence")]
    public class SystemsHandlingSequence : SerializedScriptableObject
    {
        [Serializable]
        public class SystemData
        {
            [AssetsOnly] public BlastproofSystem system;
            [AssetsOnly] public BlastproofSystem[] dependentSystems;
            [NonSerialized, ReadOnly, ShowInInspector] public bool handled;
        }
        
        [BoxGroup("Systems")] public List<SystemData> systemsToHandle = new List<SystemData>();

        public void Reset()
        {
            foreach (var systemData in systemsToHandle)
            {
                systemData.handled = false;
                systemData.system.Reset();
            }
        }
        
    }
}