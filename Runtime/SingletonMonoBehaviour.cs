using System;
using UnityEngine;

namespace ShalicoUtils
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
        where T : SingletonMonoBehaviour<T>
    {
        private static T s_instance;

        public static bool IsValid => s_instance != null;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindFirstObjectByType<T>();

                    if (s_instance == null)
                        throw new Exception($"No instance of {typeof(T)} found in the scene");

                    s_instance.Initialize();
                }

                return s_instance;
            }
        }

        private void Awake()
        {
            if (s_instance == null)
            {
                s_instance = (T)this;
                s_instance.Initialize();
            }
            else if (s_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (s_instance == this)
            {
                Cleanup();
                s_instance = null;
            }
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void Cleanup()
        {
        }
    }
}