using System;
using UnityEngine;
using UnityEngine.Events;

namespace Blastproof.Systems.Core
{
	// Event Initializers
	[Serializable] public class UnityEvent_Bool : UnityEvent<bool> { }
	[Serializable] public class UnityEvent_Int : UnityEvent<int> { }
	[Serializable] public class UnityEvent_String : UnityEvent<string> { }
}
