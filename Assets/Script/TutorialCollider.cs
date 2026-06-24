using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class TutorialCollider : MonoBehaviour
{
    public event Action OnPlayerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
            OnPlayerEnter?.Invoke();
    }
}