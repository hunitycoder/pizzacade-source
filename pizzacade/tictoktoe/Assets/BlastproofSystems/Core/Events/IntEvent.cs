using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Blastproof.Systems.Core
{
	/*
		An event that may be fired inside the Blastproof.Systems namespace
	*/
	[CreateAssetMenu(menuName = "Blastproof/Events/IntEvent")]
	public class IntEvent : ScriptableObject
	{
		// The event 
        [BoxGroup("Event"), ShowInInspector, ReadOnly] private UnityEvent_Int eventHappen = new UnityEvent_Int();

        // What listeners are searching for it
        [BoxGroup("Listeners"), ShowInInspector, ReadOnly] private List<UnityAction<int>> listeners = new List<UnityAction<int>>();

        // The last value the event was invoked with
        [BoxGroup("Info"), SerializeField, ReadOnly] private int _lastValue;

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(1001)] private int _debugValue;
#endif

		// The event invocation method
		public void Invoke(int value)
		{ eventHappen.Invoke(value); }

		// Ways to subscribe/unsubscribe to it
		public void Subscribe(UnityAction<int> action)
		{ listeners.Add(action); eventHappen.AddListener(action); }

		public void Unsubscribe(UnityAction<int> action)
		{ listeners.Remove(action); eventHappen.RemoveListener(action); }

#if UNITY_EDITOR
        // Fired in the editor, for whatever debugging reason
        [Button(ButtonSizes.Large), PropertyOrder(1002)]
        private void DebugInvoke() { eventHappen.Invoke(_debugValue); }
#endif
	}
}
