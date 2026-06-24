using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : Character, IInteractable
{
    public override bool CanMove => true;

    public bool CanInteract => true;

    public Transform InteractionPoint => transform;

    public string InteractionText => "Á×¿³!";

    public event Action OnDead;

    public void Interact(Player player)
    {
        OnDead?.Invoke();
        Destroy(gameObject);
    }
}
