using UnityEngine;

namespace Common.UIScript
{
    public static class ResManager
    {
        // 加载gameobject 不实例化
        public static GameObject LoadGameObjectSync(string location)
        {
            GameObject prefab = Resources.Load<GameObject>(location);
            if (prefab == null)
            {
                Debug.LogError("Failed to load resource: " + location);
                return null;
            }

            return prefab;
        }
        
        // 加载gameobject 并且实例化
        public static GameObject InstantiateGameObjectSync(string location)
        {
            GameObject prefab = Resources.Load<GameObject>(location);
            if (prefab == null)
            {
                Debug.LogError("Failed to load resource: " + location);
                return null;
            }
            
            // 使用实例化方法创建预制体
            GameObject instance = GameObject.Instantiate(prefab);

            return instance;
        }

        /*
        public static GameObject InstantiateGameObjectSync(string location, Transform parent = null)
        {
            var p = YooAssets.LoadAssetSync<GameObject>(location);
            var ip = p.InstantiateSync(parent);
            return ip;
        }
        
        public static void LoadGameObjectAsync(string location, System.Action<GameObject> callback)
        {
            var p = YooAssets.LoadAssetAsync<GameObject>(location);
            p.Completed += handle =>
            {
                callback?.Invoke(handle.GetAssetObject<GameObject>());
            };
        }
        
        public static void InstantiateGameObjectAsync(string location, System.Action<GameObject> callback)
        {
            var p = YooAssets.LoadAssetAsync<GameObject>(location);
            p.Completed += handle =>
            {
                var ip = handle.InstantiateAsync();
                ip.Completed += handle2 =>
                {
                    callback?.Invoke(ip.Result);
                };
            };
        }
        
        public static async UniTask<GameObject> InstantiateGameObjectAsync(string location)
        {
            var p = YooAssets.LoadAssetAsync<GameObject>(location);
            await p;
            var ip = p.InstantiateAsync();
            await ip;
            return ip.Result;
        }

        public static T LoadAssetSync<T>(string location) where T : UnityEngine.Object
        {
            return YooAssets.LoadAssetSync<T>(location).GetAssetObject<T>();
        }

        public static void LoadAssetAsync<T>(string location, System.Action<T> callback) where T : UnityEngine.Object
        {
            var p = YooAssets.LoadAssetAsync<T>(location);
            p.Completed += handle =>
            {
                callback?.Invoke(handle.GetAssetObject<T>());
            };
        }
        */
    }
        
}