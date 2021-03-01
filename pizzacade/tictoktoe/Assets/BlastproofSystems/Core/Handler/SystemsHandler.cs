using System.Collections.Generic;
using System.Linq;
using MEC;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.Core.Handler
{
	/*
		A class that handles the handling (initialization / update) of systems in an orderly fashion
	*/
	[CreateAssetMenu(menuName = "Blastproof/Systems/Core/Handler/SystemsHandler")]

	public class SystemsHandler : SerializedScriptableObject
	{
		// The amount of time to wait between initialization phases
		private const float INITIALIZER_DELAY = .1f;

		// References to the systems to be handled. 
		// To be added in the desired handling order
		[BoxGroup("Data"), SerializeField] private SystemsHandlingSequence _sequence;

		// Fired after all systems have been handled
		[BoxGroup("Data"), SerializeField] private SimpleEvent _whenToHandle;

		// Fired after all systems have been handled
		[BoxGroup("Data"), SerializeField] private SimpleEvent _onAllSystemsHandled;

		// Are all systems handled?
		[BoxGroup("Systems info"), ShowInInspector, ReadOnly]
		private bool AllSystemsHandled => (_sequence != null) && _sequence.systemsToHandle.All(x => x.system.IsInitialized());

		// The systems that have been handled
		[BoxGroup("Systems info"), ShowInInspector, ReadOnly]
		private List<BlastproofSystem> _systemsHandled = new List<BlastproofSystem>();

		// The systems that are still to be handled
		[BoxGroup("Systems info"), ShowInInspector, ReadOnly]
		private List<BlastproofSystem> _systemsLeftToHandle = new List<BlastproofSystem>();

		// The coroutine handle
		private CoroutineHandle _handle;

		// The systems handling for initialization is done in Awake()
		private void OnEnable() { _whenToHandle.Subscribe(Handle); }
		private void OnDisable() { _whenToHandle.Unsubscribe(Handle); }

		// Handles the system at the desired time.
		[Button(ButtonSizes.Large)]
		private void Handle()
		{
			// Reset the sequence
			_sequence.Reset();

			// And the container lists
			_systemsHandled.Clear();
			_systemsLeftToHandle.Clear();

			// Add all the systems as left to handle
			_systemsLeftToHandle.AddRange(_sequence.systemsToHandle.Select(x => x.system));

			// Start handling them
			_handle = Timing.RunCoroutine(HandleSystems(), Segment.SlowUpdate);
		}

		// A coroutine which handles the systems
		private IEnumerator<float> HandleSystems()
		{
			// Did we progress handling anything new?
			var progress = false;

			// Loop through each individual system data
			foreach (var systemData in _sequence.systemsToHandle)
			{
				// Ignore systems already handled
				if (systemData.handled) continue;

				// If it has no dependent systems or all of its dependent systems are initialized 
				if (systemData.dependentSystems.Length == 0 || DependenciesInitialized(systemData))
				{
					// Handle it!
					systemData.system.Initialize();

					// Mark as handled
					systemData.handled = true;

					// Put it in the appropriate container
					_systemsHandled.Add(systemData.system);
					_systemsLeftToHandle.Remove(systemData.system);

					// We handled a new system!
					progress = true;
				}
			}

			// Log current status
			if (progress)
				Log.Message("<color=#00FF00>" + GetType() + ": " + _systemsHandled.Count + " systems.</color>\n" +
				            "<color=#FF0000>Left to handle: (" + _systemsLeftToHandle.Count + ")=[" + string.Join(", ", _systemsLeftToHandle.Select(x => x.name).ToArray()) + "]</color>");

			// If not all systems have been handled
			if (AllSystemsHandled)
			{
				Log.Message("<color=#FFFF00>ALL systems handled!</color>");
				_onAllSystemsHandled.Invoke();
			}
			else
			{
				Log.Message("Rerun loop because systems uninitialized");
				foreach(var system in _sequence.systemsToHandle)
				{
					if(!system.system.IsInitialized())
					{
					    Log.Message($"{system.system.name} - is NOT Initialized");
						foreach(var dependent in system.dependentSystems)
							Log.Message($" Dependency{dependent.name} - {dependent.IsInitialized()}");
					}
				}
				// Wait a little
				yield return Timing.WaitForSeconds(INITIALIZER_DELAY);

				// Do this again
				_handle = Timing.RunCoroutine(HandleSystems(), Segment.SlowUpdate);
			}
		}

		// Returns true if the system has been handled, false otherwise.
		// By handling we understand initialization / synchronization
		private bool DependenciesInitialized(SystemsHandlingSequence.SystemData systemData)
		{
			foreach (var system in systemData.dependentSystems)
			{
				if (!system.IsInitialized()) 
				{ 
					Log.Message("System not initialized: " + system.name);
					return false;
				}
			}
			return true;
		}
	}
}
