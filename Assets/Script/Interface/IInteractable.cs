using UnityEngine;

public interface IInteractable
{
    bool CanInteract { get; }
    Transform InteractionPoint { get; }
    string InteractionText { get; }
    void Interact(Player player);
}
