using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    public List<AudioSource> bgms;

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

    public void SetBgm(int index, bool setting)
    {
        bgms[index].volume = setting ? 1f : 0f;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);

        //    if (sceneName == "Main")
        //    {
        //        bgm1.volume = 0.0f;
        //        bgm4.volume = 0.0f;
        //        bgm3.volume = 0.0f;
        //        bgm1.Play();
        //        bgm2.Play();
        //        bgm3.Play();
        //        bgm4.Play();

        //    }

        //    else if(sceneName == "Stage1")
        //    {
        //        bgm2.Play();
        //        bgm3.Play();
        //        bgm4.Play();
        //    }

        //    else if(sceneName == "Chapter0")
        //    {
        //        bgm2.Play();
        //        bgm3.Play();
        //        bgm4.Play();
        //    }
    }



}