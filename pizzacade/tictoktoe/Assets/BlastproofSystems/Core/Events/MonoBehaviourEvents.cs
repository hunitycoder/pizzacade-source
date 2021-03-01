using Blastproof.Utility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.Core
{
	/*
		This class instantiates automatically at the start of the game.
		It handles MonoBehaviour events that might be required by scriptable objects.
		Necessary because Scriptable Objects don't receive them, and perhaps we don't want to have this limitation
		The alternative, having separate monobehaviours for every scriptable object is cumbersome
	*/
	public class MonoBehaviourEvents : Singleton<MonoBehaviourEvents>
	{
		[BoxGroup("EventsData")] [SerializeField, AssetsOnly] private BoolEvent _pauseEvent;
        [BoxGroup("EventsData")] [SerializeField, AssetsOnly] private BoolEvent _focustEvent;
        [BoxGroup("EventsData")] [SerializeField, AssetsOnly] private SimpleEvent _quitEvent;
		[BoxGroup("EventsData")] [SerializeField, AssetsOnly] private SimpleEvent _awakeEvent;
		[BoxGroup("EventsData")] [SerializeField, AssetsOnly] private SimpleEvent _startEvent;

		protected override void Awake() { base.Awake(); _awakeEvent.Invoke(); Log.Message($"Awake event invoked"); }
		protected void Start() { _startEvent.Invoke(); Log.Message($"Start event invoked"); }
		protected void OnApplicationPause(bool pause) { _pauseEvent.Invoke(pause); Log.Message($"Pause event invoked with value: {pause}"); }
        protected void OnApplicationFocus(bool focused) { _focustEvent.Invoke(focused); Log.Message($"Focus event invoked with value: {focused}"); }
        protected void OnApplicationQuit() { _quitEvent.Invoke(); Log.Message("Quit event invoked"); }
	}
}
