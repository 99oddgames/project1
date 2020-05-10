using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoPoolable : MonoBehaviour
{
   private MonoPoolable m_prefab = null;
   private bool m_isSpawned = false;
   private bool m_isDesignTimeObject = true;

   private float m_spawnTimestamp;

   public bool IsSpawned
   {
      get
      {
         return m_isSpawned;
      }
   }

   public bool IsDesignTimeObject
   {
      get
      {
         return m_isDesignTimeObject;
      }
      set
      {
         m_isDesignTimeObject = value;
      }
   }

   public float ElapsedSinceSpawn
   {
      get
      {
         if(!IsSpawned)
         {
            Debug.LogError($"Pooled instance ({name}) is being polled for time ElapsedSinceSpawn while not spawned. This is not supported.");
            return -1;
         }

         return Time.time - m_spawnTimestamp;
      }
   }

   public MonoPoolable Prefab
   {
      get
      {
         return m_prefab;
      }
      set
      {
         m_prefab = value;
      }
   }

   public void OnSpawned()
   {
      m_spawnTimestamp = Time.time;
      m_isSpawned = true;
      OnSpawn();
   }

   public void OnDespawned()
   {
      m_isSpawned = false;
   }

   private void Awake()
   {
      if(m_isDesignTimeObject)
      {
         InitDesignTimeObject();
      }

      OnAwake();
   }

   public virtual void OnAwake() { }
   public virtual void Reinitialize() { }
   public virtual void OnDespawn() { }
   public virtual void OnSpawn() { }

   public void InitDesignTimeObject()
   {
      Reinitialize();
      OnSpawned();
   }

   public void Despawn(float delay = 0)
   {
      if (!m_isSpawned)
         return;

      if (delay == 0)
      {
         StopAllCoroutines();
         PoolService.Despawn(this);
         OnDespawn();

         return;
      }

      StartCoroutine(DespawnRoutine(delay));
   }

   private IEnumerator DespawnRoutine(float delay)
   {
      float elapsed = 0.0f;

      while (elapsed < delay)
      {
         yield return null;
         elapsed += Time.deltaTime;
      }

      StopAllCoroutines();
      PoolService.Despawn(this);
      OnDespawn();
   }
}
