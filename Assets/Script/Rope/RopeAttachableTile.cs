using UnityEngine;

public enum RopeDir
{
    Vertical,
    Horizontal
}

public class RopeAttachableTile : MonoBehaviour, IRopeAttachable, IInteractable
{
    public Transform AttachedPoint => transform;
    public string InteractionText {
        get
        {
            if (attachedRopeBridge == null) return "¸Åµì ¹­±â";
            else return "¸Åµì ¼ö°Å";
        }
    }
    public bool CanInteract => true;
    public Transform InteractionPoint => this.transform;

    private RopeBridge attachedRopeBridge;
    public RopeBridge AttachedRopeBridge => attachedRopeBridge;

    [SerializeField] private RopeDir ropeDir;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite attachedSprite;
    [SerializeField] private Sprite normalSprite;
    public RopeDir RopeDir => ropeDir;

    public void Interact(Player player)
    {
        if (attachedRopeBridge == null)
        {
            player.ReadyRope(this);
        }
        else
        {
            attachedRopeBridge.DestroyBridge();
            player.GetKnot();
        }
    }

    public void OnRopeBridgeAttached(RopeBridge bridge)
    {
        spriteRenderer.sprite = attachedSprite;
        attachedRopeBridge = bridge;
    }

    public void OnRopeBridgeDetached(RopeBridge bridge)
    {
        if (attachedRopeBridge != bridge) return;

        spriteRenderer.sprite = normalSprite;
        attachedRopeBridge = null;
    }
}
