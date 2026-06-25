using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    [SerializeField] private AudioClip[] clips;

    private AudioSource[] bgmLayers;

    private void Start()
    {
        bgmLayers = new AudioSource[clips.Length];

        /*
        for (int i = 0; i < clips.Length; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.clip = clips[i];
            source.loop = true;
            //source.Play();

            bgmLayers[i] = source;
        }*/

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        AudioSource source = gameObject.AddComponent<AudioSource>();
        bgmLayers = new AudioSource[clips.Length];

        if (sceneName == "Main")
        {
            source.clip = clips[0];
            source.loop = true;
            source.Play();

        }

        else if(sceneName == "Stage1")
        {
            source.clip = clips[1];
            source.loop = true;
            source.Play();
            source.clip = clips[2];
            source.loop = true;
            source.Play();
            source.clip = clips[3];
            source.loop = true;
            source.Play();
        }

        else if(sceneName == "Chapter0")
        {
            source.clip = clips[0];
            source.Stop();
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();

        if (source.clip == clip) return;

        source.clip = clip;
        source.Play();
    }

}