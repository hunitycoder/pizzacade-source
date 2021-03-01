using System.Collections.Generic;
using Blastproof.Systems.Core.Variables;
using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.Events;

namespace Blastproof.Systems.Core
{
    /*
		An event that may be fired inside the Blastproof.Systems namespace
	*/
    [CreateAssetMenu(menuName = "Blastproof/Events/StringEvent")]
    public class StringEvent : ScriptableObject
    {
        // The event 
        [BoxGroup("Event"), ShowInInspector, ReadOnly] private UnityEvent_String eventHappen = new UnityEvent_String();

        // What listeners are searching for it
        [BoxGroup("Listeners"), ShowInInspector, ReadOnly] private List<UnityAction<string>> listeners = new List<UnityAction<string>>();

        // The last value the event was invoked with
        [BoxGroup("Info"), SerializeField, ReadOnly] private string _lastValue;

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(1001)] private string _debugValue;
#endif

        // The event invocation method
        public void Invoke(string value)
        { _lastValue = value; eventHappen.Invoke(value); }

        // The event invocation method
        public void Invoke(StringVariable value)
        { _lastValue = value.Value; eventHappen.Invoke(value.Value); }

        // Ways to subscribe/unsubscribe to it
        public void Subscribe(UnityAction<string> action)
        { listeners.Add(action); eventHappen.AddListener(action); }

        public void Unsubscribe(UnityAction<string> action)
        { listeners.Remove(action); eventHappen.RemoveListener(action); }

#if UNITY_EDITOR
        // Fired in the editor, for whatever debugging reason
        [Button(ButtonSizes.Large), PropertyOrder(1002)]
        private void DebugInvoke() { eventHappen.Invoke(_debugValue); }
#endif
    }
}
