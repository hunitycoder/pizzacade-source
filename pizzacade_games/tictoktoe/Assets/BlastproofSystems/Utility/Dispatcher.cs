using System.Collections;
using System.Collections.Generic;
using System;
using Blastproof.Systems.Core;
using Blastproof.Utility;
using UnityEngine;

// ---- This class is used to ensure calls on the main thread.
// ---- At the moment there are issues with Firebase Transactions which are called on separate threads.
namespace Blastproof.Tools
{
    public class Dispatcher : Singleton<Dispatcher>
    {
        // ---- actions queued for execution, on the main thread
        Queue<Action> q = new Queue<Action>();

        // ---- Fire actions to execute, while present
        private void Update() { lock (q) { while (q.Count > 0) { q.Dequeue().Fire(); } } }

        // ---- Enqueues an IEnumerator
        private void Enqueue(IEnumerator coroutime) { lock (q) { q.Enqueue(() => { StartCoroutine(coroutime); }); } }
        public void RunOnMain(Action action) { Instance.Enqueue(ActionWrapper(action)); }

        // ---- Wraps an action into an IEnumerator to be executed with StartCoroutine
        private IEnumerator ActionWrapper(Action a) { a.Fire(); yield return null; }
    }
}