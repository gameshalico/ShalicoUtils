using UnityEngine;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif

namespace ShalicoUtils
{
    public abstract class ScriptableSettings<T> : ScriptableObject
        where T : ScriptableSettings<T>
    {
        private static T _instance;

        public static bool HasInstance => _instance != null;

        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {
                    var asset = PlayerSettings.GetPreloadedAssets().OfType<T>().FirstOrDefault();
                    _instance = asset != null ? asset : CreateInstance<T>();
                }

                return _instance;

#else
                if (_instance == null) _instance = CreateInstance<T>();

                return _instance;
#endif
            }
        }

        protected void OnEnable()
        {
            _instance = (T)this;
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }

#if UNITY_EDITOR
        protected static void Create()
        {
            var typeName = typeof(T).Name;
            var assetPath =
                EditorUtility.SaveFilePanelInProject($"Save {typeName}", nameof(T),
                    "asset", "", "Assets");

            if (string.IsNullOrEmpty(assetPath))
                return;

            var instance = CreateInstance<T>();
            AssetDatabase.CreateAsset(instance, assetPath);
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.RemoveAll(x => x is T);
            preloadedAssets.Add(instance);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            AssetDatabase.SaveAssets();
        }
#endif
    }
}