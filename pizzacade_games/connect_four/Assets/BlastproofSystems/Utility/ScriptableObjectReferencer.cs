using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Linq;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Blastproof
{
    public class ScriptableObjectReferencer : MonoBehaviour
    {
        [BoxGroup("Load References"), SerializeField] private string _folder;
        
        [BoxGroup("Load References"), SerializeField] public List<ScriptableObject> scriptableObjects;

#if UNITY_EDITOR
        [Button]
        public void AddAllRefs()
        {
            scriptableObjects = new List<ScriptableObject>(GetAllInstances<ScriptableObject>());
        }

        public ScriptableObject GetRef(Type t)
        {
            foreach(var r in scriptableObjects)
            {
                if (r.GetType() == t)
                    return r;
            }
            return null;
        }

        private List<T> GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets("t:" + typeof(T).Name, new[] { $"Assets/{_folder}" });
            var list = new List<T>();
            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                list.Add(asset);
                EditorUtility.SetDirty(asset);
            }
            return list;
        }

        [Button]
        void LogMyPath()
        {
            var prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
            Log.Message(AssetDatabase.GetAssetPath(prefab));
        }
#endif
    }
}