using System;
using UnityEngine;

public enum PlayerState
{
    None,
    RopeReady
}

public class Player : Character
{
    [Header("State")]
    private PlayerState state = PlayerState.None;

    private int knotCount;
    public event Action<PlayerState> OnPlayerStateChanged;
    public PlayerState PlayerState => state;

    private IRopeAttachable ropeHolder;
    public IRopeAttachable RopeHolder => ropeHolder;

    public void ReadyRope(IRopeAttachable ropeAttachable)
    {
        if (ropeAttachable == null) 
            return;
        if (knotCount <= 0)
            return;

        knotCount--;
        SetMovementLock(true);
        ropeHolder = ropeAttachable;
        SetPlayerState(PlayerState.RopeReady);
    }

    public void GetRope()
    {
        knotCount++;
    }

    public void SetPlayerState(PlayerState state)
    {
        this.state = state;
        OnPlayerStateChanged?.Invoke(state);
    }
}