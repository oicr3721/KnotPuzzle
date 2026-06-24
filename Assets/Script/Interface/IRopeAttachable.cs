using UnityEngine;

public interface IRopeAttachable
{
    Transform AttachedPoint { get; }

    void OnRopeBridgeAttached(RopeBridge bridge);
    void OnRopeBridgeDetached(RopeBridge bridge);

    RopeBridge AttachedRopeBridge { get; }
}
