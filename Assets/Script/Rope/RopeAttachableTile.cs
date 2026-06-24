using UnityEngine;

public enum RopeDir
{
    Vertical,
    Horizontal
}

public class RopeAttachableTile : MonoBehaviour, IRopeAttachable, IInteractable
{
    private bool attached = false;

    public Transform AttachedPoint => transform;
    public string InteractionText {
        get
        {
            if (attached) return "¸Åµì ¼ö°Å";
            else return "¸Åµì ¹­±â";
        }
    }
    public bool CanInteract => attached == false;
    public Transform InteractionPoint => this.transform;
    public bool Attached => attached;

    private RopeBridge attachedRopeBridge;
    public RopeBridge AttachedRopeBridge => attachedRopeBridge;

    private RopeDir ropeDir;
    public RopeDir RopeDir => ropeDir;

    public void Interact(Player player)
    {
        if (attachedRopeBridge == null)
        {
            player.SetRopeHoldTarget(this);
        }
        else
        {
            player.GetRope();
        }
    }

    public void OnRopeAttached(RopeBridge bridge)
    {
        attached = true;
    }

    public void OnRopeDetached(RopeBridge bridge)
    {
        attached = false;

        attachedRopeBridge.DestroyBridge();
    }

    public void OnRopeBridgeAttached(RopeBridge bridge)
    {
        throw new System.NotImplementedException();
    }

    public void OnRopeBridgeDetached(RopeBridge bridge)
    {
        throw new System.NotImplementedException();
    }
}
