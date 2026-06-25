using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoBGMManager : MonoBehaviour
{
    private static TutoBGMManager instance;

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
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // 중복 생성 방지
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
