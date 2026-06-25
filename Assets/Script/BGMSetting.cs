using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSetting : MonoBehaviour
{
    public int BGMIndex;
    public bool setting;

    // Start is called before the first frame update
    void Start()
    {
        BGMManager.instance.SetBgm(BGMIndex, setting);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
