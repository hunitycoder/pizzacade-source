using System;
using Blastproof.Tools;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Blastproof.Systems.Core
{
    public abstract class BlastproofSystem : SerializedScriptableObject
    {
        //Fired when the system was successfully initialized
        [BoxGroup("Initialization"), ReadOnly, NonSerialized, ShowInInspector]
        public Action onSystemInitialized;

        // Is the system initialized
        [BoxGroup("Initialization"), ReadOnly, NonSerialized, ShowInInspector]
        private bool _initialized;

        protected virtual void OnEnable()
        {
            onSystemInitialized += MarkInitialized;
        }

        protected virtual void OnDisable()
        {
            onSystemInitialized -= MarkInitialized;
        }

        public virtual void Initialize()
        {
            Dispatcher.Instance.RunOnMain(() => { Log.Message($"<color=#0000FF> {name} </color> Initialization BEGIN!"); });
        }

        public bool IsInitialized() => _initialized;

        public virtual void Reset()
        {
            _initialized = false;
        }

        private void MarkInitialized()
        {
            onSystemInitialized -= MarkInitialized;
            _initialized = true;
            if(Dispatcher.Instance) Dispatcher.Instance.RunOnMain(() => { Log.Message($"<color=#00FF00> {name}</color> Initialization END!"); });
        }
    }
}
