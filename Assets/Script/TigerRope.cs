using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerRope : MonoBehaviour, IInteractable
{
    [SerializeField] Enemy Tiger;
    [SerializeField] SpriteRenderer screwRenderer;
    [SerializeField] SpriteRenderer ropeRenderer;
    [SerializeField] Sprite screwKnot;

    public bool CanInteract => Player.Instance.KnotState[KnotType.Arms];

    public Transform InteractionPoint => transform;

    public string InteractionText => "팔을 사용해 동앗줄 만들기";

    public void Interact(Player player)
    {
        screwRenderer.sprite = screwKnot;
        ropeRenderer.enabled = true;

        player.UseKnot(KnotType.Arms);
        Tiger.Interact(player);
    }
}
