using UnityEngine;

namespace Blastproof.Utility
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] private bool dontDestroyOnLoad;
        
        static T _instance;
        public static T Instance { get {
            if (_instance == null) _instance = FindObjectOfType<T>();
            return _instance;
        } }

        protected virtual void Awake()
        {
            if (Instance == null) _instance = this as T;
            if (Instance == null) _instance = FindObjectOfType<T>();

            if (Instance != this)
            {
                Log.Warning("Destroying this instance because I found another.");
                Destroy(gameObject);
            }
            if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        protected virtual void OnDestroy() { _instance = null; }
    }
}