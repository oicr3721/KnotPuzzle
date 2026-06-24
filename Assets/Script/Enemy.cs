using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IInteractable
{
    public override bool CanMove => true;

    public bool CanInteract => true;

    public Transform InteractionPoint => transform;

    public string InteractionText => "ав©Ё!";

    public void Interact(Player player)
    {
        Debug.Log($"{name} ав╢ы");
        Destroy(gameObject);
    }
}
