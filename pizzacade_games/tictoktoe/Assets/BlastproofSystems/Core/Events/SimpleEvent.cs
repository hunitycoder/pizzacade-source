using System.Collections.Generic;
using Sirenix.OdinInspector;

using UnityEngine;
using UnityEngine.Events;

namespace Blastproof.Systems.Core
{
	/*
		An event that may be fired inside the Blastproof.Systems namespace
	*/
	[CreateAssetMenu(menuName = "Blastproof/Events/SimpleEvent")]
	public class SimpleEvent : ScriptableObject
	{
		[BoxGroup("Events debugging"), ShowInInspector, ReadOnly]
		public List<string> EnterMethods
		{
			get
			{
				var methods = new List<string>();
				if (listeners != null)
				{
					foreach(var listener in listeners)
						methods.Add(listener.Method.Name);
				}
				return methods;
			}
		}
		 
		// The event 
        [BoxGroup("Event"), ShowInInspector, ReadOnly] private UnityEvent eventHappen = new UnityEvent();

		// What listeners are searching for it
		[BoxGroup("Listeners"), ShowInInspector, ReadOnly] private List<UnityAction> listeners = new List<UnityAction>();

		// The event invocation method
		public void Invoke()
		{ eventHappen.Invoke(); }

		// Ways to subscribe/unsubscribe to it
		public void Subscribe(UnityAction action)
		{ listeners.Add(action); eventHappen.AddListener(action); }

		public void Unsubscribe(UnityAction action)
		{ listeners.Remove(action); eventHappen.RemoveListener(action); }

#if UNITY_EDITOR
        // Fired in the editor, for whatever debugging reason
        [Button(ButtonSizes.Large), PropertyOrder(1002)]
        private void DebugInvoke() { eventHappen.Invoke(); }
#endif
	}
}
