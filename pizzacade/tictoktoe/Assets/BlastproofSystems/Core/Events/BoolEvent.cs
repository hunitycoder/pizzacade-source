using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Blastproof.Systems.Core
{
	/*
		An event that may be fired inside the Blastproof.Systems namespace
	*/
	[CreateAssetMenu(menuName = "Blastproof/Events/BoolEvent")]
	public class BoolEvent : ScriptableObject
	{
		// The event 
        [BoxGroup("Event"), ShowInInspector, ReadOnly] private UnityEvent_Bool eventHappen = new UnityEvent_Bool();

		// What listeners are searching for it
		[BoxGroup("Listeners"), ShowInInspector, ReadOnly] private List<UnityAction<bool>> listeners = new List<UnityAction<bool>>();

        // The last value the event was invoked with
        [BoxGroup("Info"), SerializeField, ReadOnly] private bool _lastValue;

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(1001)] private bool _debugValue;
#endif

		// The event invocation method
		public void Invoke(bool value)
		{ eventHappen.Invoke(value); }

		// Ways to subscribe/unsubscribe to it
		public void Subscribe(UnityAction<bool> action)
		{ listeners.Add(action); eventHappen.AddListener(action); }

		public void Unsubscribe(UnityAction<bool> action)
		{ listeners.Remove(action); eventHappen.RemoveListener(action); }

		public void UnsubscribeAll()
		{ listeners.Clear(); eventHappen.RemoveAllListeners();}

#if UNITY_EDITOR
		// Fired in the editor, for whatever debugging reason
		[Button(ButtonSizes.Large), PropertyOrder(1002)]
        private void DebugInvoke() { eventHappen.Invoke(_debugValue); }
#endif
	}
}
