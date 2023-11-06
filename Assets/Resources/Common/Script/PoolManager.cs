using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Common.UIScript
{
    /// <summary>
    /// GameObject对象池，注意必须回收，否则会内存泄漏
    /// </summary>
    public class PoolManager
    {
        /// <summary>
        /// 对GameObject和其对应的资源名的封装，方便进行释放；无其它用途
        /// </summary>
        public class PoolItem
        {
            public GameObject Obj { get; set; }
            public string Name { get; set; }
        }
        
        static Dictionary<string, ObjectPool<GameObject>> _pools = new Dictionary<string, ObjectPool<GameObject>>();

        /// <summary>
        /// 从对象池中获取GameObject；如果你有一个变量保存了name，那就使用这个接口，参考Lod.cs中的用法。Release的时候把你保存的name传入；
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject Get(string name,  Transform parent = null)
        {
            if (!_pools.TryGetValue(name, out var pool))
            {
                pool = new ObjectPool<GameObject>(() =>
                {
                    return CreateGameObject(name);
                }, OnGet, OnRelease, OnDestroy, true);
                _pools.Add(name, pool);
            }

            GameObject obj = pool.Get();
            obj.transform.SetParent(parent);
            
            return obj;
        }
        
        public static void Release(string name, GameObject obj)
        {
            if (_pools.TryGetValue(name, out var pool))
            {
                pool.Release(obj);
            }
        }

        /// <summary>
        /// 从对象池中获取PoolItem；PoolItem是对GameObject和其对应的资源名的封装，方便进行释放；一般动态可配置的资源用该接口；
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static PoolItem GetPoolItem(string name, Transform parent = null)
        {
            var go = Get(name, parent);
            var item = GenericPool<PoolItem>.Get();
            item.Obj = go;
            item.Name = name;
            return item;
        }
        
        public static void Release(PoolItem item)
        {
            Release(item.Name, item.Obj);
            GenericPool<PoolItem>.Release(item);
        }

        /// <summary>
        /// Clean all pools
        /// </summary>
        public static void Clear()
        {
            _pools.Clear();
        }

        private static GameObject CreateGameObject(string resName)
        {
            return ResManager.InstantiateGameObjectSync(resName);
        }
        
        private static void OnGet(GameObject obj)
        {
            obj.SetActive(true);
        }
        
        private static void OnRelease(GameObject obj)
        {
            obj.transform.SetParent(null);
            obj.SetActive(false);
        }
        
        private static void OnDestroy(GameObject obj)
        {
            Object.Destroy(obj);
        }
    }
}