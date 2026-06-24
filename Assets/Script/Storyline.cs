using System;
using UnityEngine;

[Serializable]
public class Storyline
{
    [TextArea]
    public string text;

    public bool spawnEnemy;

    public float delayAfterText = 1f;
}