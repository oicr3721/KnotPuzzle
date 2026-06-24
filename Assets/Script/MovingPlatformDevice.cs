using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformDevice : MonoBehaviour, IInteractable
{
    [SerializeField] private MovingPlatform movingPlatform;

    public bool CanInteract => Player.Instance.KnotState[KnotType.Arms];

    public Transform InteractionPoint => transform;

    public string InteractionText => "장치 건드리기";

    public void Interact(Player player)
    {
        movingPlatform.ToggleMoving();
    }
}
