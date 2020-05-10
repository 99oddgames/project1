using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShakeInfo
{
    public bool Play;
    public float Amp;
    public float Duration;
}

public class SpecialEffect : MonoPoolable
{
    public ShakeInfo Screenshake;

    private AudioSource[] audioSources = null;
    private ParticleSystem[] particleSystems = null;
    private PlayRandomClipOnEnable[] clipPlayers = null;

    public static SpecialEffect SafeSpawn(SpecialEffect prefab, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (prefab == null)
            return null;

        var instance = PoolService.Spawn(prefab, position, rotation);

        if (instance == null)
            return null;

        if(parent != null)
        {
            instance.transform.SetParent(parent);
        }

        if (instance.Screenshake.Play)
        {
            Debug.Log("implement screenshakes");
        }

        return instance;
    }

    private void Awake()
    {
        audioSources = GetComponentsInChildren<AudioSource>();
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        clipPlayers = GetComponentsInChildren<PlayRandomClipOnEnable>();
    }

    private void Update()
    {
        foreach (var source in audioSources)
        {
            if (source.isPlaying)
                return;
        }

        foreach (var system in particleSystems)
        {
            if (system.isPlaying)
                return;
        }

        foreach(var clipPlayer in clipPlayers)
        {
            if (clipPlayer.IsWaiting)
                return;
        }

        Despawn();
    }
}
