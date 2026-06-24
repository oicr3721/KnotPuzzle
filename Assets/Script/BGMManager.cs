using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    private AudioSource[] bgmLayers;

    private void Start()
    {
        bgmLayers = new AudioSource[clips.Length];

        for (int i = 0; i < clips.Length; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.clip = clips[i];
            source.loop = true;
            source.Play();

            bgmLayers[i] = source;
        }

    }
  
}