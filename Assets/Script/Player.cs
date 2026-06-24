using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    None,
    RopeReady,
    Select
}

public enum KnotType
{
    Eyes,
    Arms,
    Legs
}

public class Player : Character
{
    [Header("State")]
    [SerializeField] private PlayerState state = PlayerState.None;
    [Header("State")]
    [SerializeField] private KnotSelector knotSelector;

    private int knotCount = 3;
    public PlayerState PlayerState => state;

    private RopeAttachableTile ropeHolder;
    public RopeAttachableTile RopeHolder => ropeHolder;
    public override bool CanMove => 
        (state == PlayerState.None)
        && knotState[KnotType.Legs] == true;

    private Dictionary<KnotType, bool> knotState = new Dictionary<KnotType, bool>
    {
        {KnotType.Eyes, true},
        {KnotType.Arms, true},
        {KnotType.Legs, true}
    };

    public Dictionary<KnotType, bool> KnotState => knotState;

    public void ReadyRope(RopeAttachableTile ropeAttachable)
    {
        if (ropeAttachable == null) 
            return;
        if (knotCount <= 0)
            return;

        ropeHolder = ropeAttachable;
        SetPlayerState(PlayerState.RopeReady);
    }

    public IEnumerator CompleteRopeConnect()
    {
        yield return StartCoroutine(knotSelector.SelectUseKnot());

        ropeHolder = null;
        SetPlayerState(PlayerState.None);

    }

    public void CancleRopeConnect()
    {
        if (PlayerState != PlayerState.RopeReady) return;

        ropeHolder = null;
        SetPlayerState(PlayerState.None);
    }

    public void GetKnot()
    {
        StartCoroutine(knotSelector.SelectGetKnot());
    }

    public void EquipKnot(KnotType knot)
    {
        knotState[knot] = true;
    }

    public void UseKnot(KnotType knot)
    {
        knotState[knot] = false;
    }

    public void SetPlayerState(PlayerState state)
    {
        this.state = state;
    }
}