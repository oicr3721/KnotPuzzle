using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PlayerState
{
    None,
    RopeReady,
    Select,
    RopeWalk
}

public enum KnotType
{
    Eyes,
    Arms,
    Legs
}

public class Player : Character
{
    public static Player Instance { get; private set; }

    [Header("State")]
    [SerializeField] private PlayerState state = PlayerState.None;
    [SerializeField] private KnotSelector knotSelector;

    [Header("Component")]
    [SerializeField] private GameObject darknessMask;

    public PlayerState PlayerState => state;

    private RopeAttachableTile ropeHolder;
    public RopeAttachableTile RopeHolder => ropeHolder;
    public override bool CanMove => 
        (state == PlayerState.None)
        && (knotState[KnotType.Legs] == true);

    private Dictionary<KnotType, bool> knotState = new Dictionary<KnotType, bool>
    {
        {KnotType.Eyes, true},
        {KnotType.Arms, true},
        {KnotType.Legs, true}
    };

    public Dictionary<KnotType, bool> KnotState => knotState;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void ReadyRope(RopeAttachableTile ropeAttachable)
    {
        if (ropeAttachable == null) 
            return;
        if (!knotState.Any(x => x.Value))
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
        if (knot == KnotType.Eyes)
            darknessMask.SetActive(false);
    }

    public void UseKnot(KnotType knot)
    {
        knotState[knot] = false;
        if(knot == KnotType.Eyes)
            darknessMask.SetActive(true);

    }

    public void SetPlayerState(PlayerState state)
    {
        this.state = state;
    }
}