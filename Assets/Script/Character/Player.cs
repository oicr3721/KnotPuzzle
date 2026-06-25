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

    [SerializeField] private RuntimeAnimatorController normal;
    [SerializeField] private RuntimeAnimatorController noEye;
    [SerializeField] private RuntimeAnimatorController noArm;
    [SerializeField] private RuntimeAnimatorController noLeg;
    [SerializeField] private RuntimeAnimatorController noEye_Arm;
    [SerializeField] private RuntimeAnimatorController noEye_Leg;
    [SerializeField] private RuntimeAnimatorController noArm_Leg;
    [SerializeField] private RuntimeAnimatorController noEye_Arm_Leg;

    [Header("Component")]
    [SerializeField] private GameObject darknessMask;
    
    [SerializeField] private AudioClip knotSound;
    private AudioSource[] audioSource;
    [SerializeField] private AudioClip walkSound;

    [SerializeField] private float footstepInterval = 0.4f;

    private float moveTimer;


    public static event Action OnRopeReady;
    public static event Action OnRopeConnected;
    public static event Action OnRopeUsed;
    public static event Action OnKnotCollected;

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

    private void Start()
    {
        audioSource = GetComponents<AudioSource>();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    
    private void Update()
    {
        HandleFootstep();
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

    private void HandleFootstep()
    {
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 ||
                        Input.GetAxisRaw("Vertical") != 0;

        if (isMoving)
        {
            moveTimer += Time.deltaTime;

            if (!audioSource[1].isPlaying)
                audioSource[1].Play();

            if (moveTimer >= footstepInterval)
            {
                moveTimer = 0f;
                audioSource[1].PlayOneShot(walkSound);
            }
        }
        else
        {
            moveTimer = 0f;
            audioSource[1].Stop();
        }
    }

    public IEnumerator CompleteRopeConnect()
    {
        audioSource[0].PlayOneShot(knotSound, 4.0f);

        yield return StartCoroutine(knotSelector.SelectUseKnot());

        ropeHolder = null;
        SetPlayerState(PlayerState.None);

        OnRopeConnected?.Invoke();
        animator.SetTrigger("Tie");
    }

    public void CancleRopeConnect()
    {
        if (PlayerState != PlayerState.RopeReady) return;

        ropeHolder = null;
        SetPlayerState(PlayerState.None);
    }

    public void GetKnot()
    {
        audioSource[0].PlayOneShot(knotSound, 4.0f);
        StartCoroutine(knotSelector.SelectGetKnot());
    }

    public void EquipKnot(KnotType knot)
    {
        knotState[knot] = true;
        if (knot == KnotType.Eyes)
            darknessMask.SetActive(false);

        OnKnotCollected?.Invoke();
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

        switch(state)
        {
            case PlayerState.RopeWalk:
                OnRopeUsed?.Invoke();
                break;
            case PlayerState.RopeReady:
                OnRopeReady?.Invoke();
                break;
        }

        SetAnimator();
    }

    public void SetAnimator()
    {
        RuntimeAnimatorController rac;

        if (knotState[KnotType.Eyes])
        {
            if (knotState[KnotType.Arms])
            {
                if (knotState[KnotType.Legs])
                {
                    rac = normal;
                }
                else
                {
                    rac = noLeg;
                }
            }
            else
            {
                if (knotState[KnotType.Legs])
                {
                    rac = noArm;
                }
                else
                {
                    rac = noArm_Leg;
                }
            }
        }
        else
        {
            if (knotState[KnotType.Arms])
            {
                if (knotState[KnotType.Legs])
                {
                    rac = noEye;
                }
                else
                {
                    rac = noEye_Leg;
                }
            }
            else
            {
                if (knotState[KnotType.Legs])
                {
                    rac = noEye_Arm;
                }
                else
                {
                    rac = noEye_Arm_Leg;
                }
            }
        }

        animator.runtimeAnimatorController = rac;
    }
}