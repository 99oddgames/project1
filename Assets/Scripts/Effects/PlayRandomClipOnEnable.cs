using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomClipOnEnable : MonoBehaviour
{
    public float Delay = 0.0f;
    public AudioClip[] Clips;

    private AudioSource source;

    public bool IsWaiting
    {
        get;
        set;
    }

    private void Awake()
    {
        source = GetComponentInChildren<AudioSource>();
    }

    private void OnEnable()
    {
        if(Delay > 0)
        {
            StartCoroutine(PlayDeleayed());
            return;
        }

        Play();
    }

    private void OnDisable()
    {
        IsWaiting = false;
    }

    private void Play()
    {
        if (Clips.Length == 0)
            return;

        int randomIndex = Random.Range(0, Clips.Length);
        source.clip = Clips[randomIndex];
        source.Play();
    }

    private IEnumerator PlayDeleayed()
    {
        Delay delay = new Delay(Delay, Delay);
        delay.Next();
        
        while (!delay.IsUp)
            yield return null;

        Play();
    }
}
