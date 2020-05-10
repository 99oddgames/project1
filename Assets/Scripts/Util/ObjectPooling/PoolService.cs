using System.Collections.Generic;
using UnityEngine;

public class PoolService
{
   private static Dictionary<MonoPoolable, MonoPool<MonoPoolable>> pools = new Dictionary<MonoPoolable, MonoPool<MonoPoolable>>(25);
   private static Transform root;

   private static void LazyInit()
   {
      if (root == null)
      {
         root = new GameObject().transform;
         root.name = "MonoPoolRoot";

         GameObject.DontDestroyOnLoad(root);
      }
   }

   public static T SafeSpawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool activate = true) where T : MonoPoolable
   {
      if (prefab == null)
         return null;

      return Spawn(prefab, position, rotation, parent, activate);
   }

   public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool activate = true) where T : MonoPoolable
   {
      if (prefab == null)
      {
         throw new MissingReferenceException("[MonoPool] Failed to spawn an object. Prefab is null.");
      }

      LazyInit();

      var pool = GetPool(prefab);

      T result = (T) pool.Spawn(position, rotation, activate);
      SetParent(result.transform, parent);

      return result;
   }

   public static void Despawn(MonoPoolable instance)
   {
      if(instance.IsDesignTimeObject)
      {
         instance.gameObject.SetActive(false);
         return;
      }

      var pool = GetPool(instance.Prefab);
      pool.Despawn(instance);
      SetParent(instance.transform, root);
   }

   public static void DespawnAll()
   {
      foreach(var prefabPoolPair in pools)
      {
         prefabPoolPair.Value.DespawnAll();
      }
   }

   private static MonoPool<MonoPoolable> GetPool(MonoPoolable prefab)
   {
      if (pools.ContainsKey(prefab))
      {
         return pools[prefab];
      }
      else
      {
         MonoPool<MonoPoolable> newPool = new MonoPool<MonoPoolable>(prefab);
         pools.Add(prefab, newPool);

         return newPool;
      }
   }

   private static void SetParent(Transform poolable, Transform parent)
   {
      poolable.SetParent(parent == null ? root : parent);
   }
}
