using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Blastproof.Systems.UI
{
    [CreateAssetMenu(menuName = "Blastproof/Systems/UI/UISubState")]
    public class UISubState : SerializedScriptableObject
    {
        [BoxGroup("System"), SerializeField] private UISystem _system;

        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, IsReadOnly = false, KeyLabel = "behaviour", ValueLabel = "child_index")]
        public Dictionary<UISubBehaviour, int> subBehaviours = new Dictionary<UISubBehaviour, int>();

        public bool Active => subBehaviours.Count > 0;
        public int FirstActiveIndex => SmallestIndexSubBehaviour();

        public void Reset() { subBehaviours.Clear(); }

        public void Register(UISubBehaviour subBehaviour)
        {
            if(!subBehaviours.ContainsKey(subBehaviour))
                subBehaviours.Add(subBehaviour, subBehaviour.transform.GetSiblingIndex());
        }

        public void UnRegister(UISubBehaviour subBehaviour)
        {
            if (subBehaviours.ContainsKey(subBehaviour))
                subBehaviours.Remove(subBehaviour);
        }

        public void Activate()
        {
            //_system.ChangeSubstate(this);
        }

        private int SmallestIndexSubBehaviour()
        {
            int smallestValue = int.MaxValue;
            foreach (var kvp in subBehaviours)
            {
                if(kvp.Value < smallestValue)
                {
                    smallestValue = kvp.Value;
                }
            }
            return smallestValue;
        }
    }
}