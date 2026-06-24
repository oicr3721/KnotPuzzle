using UnityEngine;

public class RopeBridge : MonoBehaviour, IInteractable
{
    private IRopeAttachable holder;
    private IRopeAttachable target;

    [SerializeField] private LineRenderer line;

    public bool CanInteract => enabled;

    public Transform InteractionPoint => transform;

    public string InteractionText => "¡Ÿ ≈∏±‚";

    public void Init(IRopeAttachable holder, IRopeAttachable target)
    {
        this.holder = holder;
        this.target = target;
    }

    private void LateUpdate()
    {
        if (holder == null || target == null)
            return;

        line.positionCount = 2;
        line.SetPosition(0, holder.AttachedPoint.position);
        line.SetPosition(1, target.AttachedPoint.position);
    }

    public void DestroyBridge()
    {
        if (holder != null)
            holder.OnRopeDetached();

        if (target != null)
            target.OnRopeDetached();

        Destroy(gameObject);
    }

    public void Interact(Player player)
    {
        throw new System.NotImplementedException();
    }
}