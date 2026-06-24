using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;

public class RopeBridge : MonoBehaviour, IInteractable
{
    private IRopeAttachable holderA;
    private IRopeAttachable holderB;

    [SerializeField] private CapsuleCollider2D ropeCollider;
    [SerializeField] private float ropeWidth = 0.5f;

    [SerializeField] private LineRenderer line;

    public bool CanInteract
    {
        get
        {
            if (ropeDir == RopeDir.Vertical)
            {
                if (Player.Instance.KnotState[KnotType.Arms])
                    return true;
                else
                    return false;
            }
            else if (ropeDir == RopeDir.Horizontal)
            {
                if (Player.Instance.KnotState[KnotType.Legs])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }

    public Transform InteractionPoint => transform;

    public string InteractionText => "줄 타기";
    private RopeDir ropeDir;

    public void Init(IRopeAttachable holder, IRopeAttachable target)
    {
        holderA = holder;
        holderB = target;

        if (holderA != null)
            holderA.OnRopeBridgeAttached(this);

        if (holderB != null)
            holderB.OnRopeBridgeAttached(this);

        if (holder.AttachedPoint.TryGetComponent<RopeAttachableTile>(out var attachable))
            ropeDir = attachable.RopeDir;

        SetupCollider();
        UpdateCollider(holderA.AttachedPoint.position, holderB.AttachedPoint.position);
    }
    private void SetupCollider()
    {
        if (ropeCollider == null)
            ropeCollider = gameObject.AddComponent<CapsuleCollider2D>();

        ropeCollider.isTrigger = true;
    }

    private void UpdateCollider(Vector2 a, Vector2 b)
    {
        Vector2 mid = (a + b) * 0.5f;
        Vector2 dir = b - a;

        float distance = dir.magnitude;

        ropeCollider.transform.position = mid;

        ropeCollider.size = new Vector2(distance, ropeWidth);
        ropeCollider.direction = CapsuleDirection2D.Horizontal;

        ropeCollider.transform.rotation =
            Quaternion.FromToRotation(Vector3.right, dir);
    }

    private void LateUpdate()
    {
        if (holderA == null || holderB == null)
            return;

        line.positionCount = 2;
        line.SetPosition(0, holderA.AttachedPoint.position);
        line.SetPosition(1, holderB.AttachedPoint.position);
    }

    public void DestroyBridge()
    {
        if (holderA != null)
            holderA.OnRopeBridgeDetached(this);

        if (holderB != null)
            holderB.OnRopeBridgeDetached(this);

        Destroy(gameObject);
    }

    public void Interact(Player player)
    {
        if (holderA == null || holderB == null)
            return;

        Transform start;
        Transform end;

        float distA = Vector3.Distance(player.transform.position, holderA.AttachedPoint.position);
        float distB = Vector3.Distance(player.transform.position, holderB.AttachedPoint.position);

        if (distA < distB)
        {
            start = holderA.AttachedPoint;
            end = holderB.AttachedPoint;
        }
        else
        {
            start = holderB.AttachedPoint;
            end = holderA.AttachedPoint;
        }

        StartCoroutine(MoveAlongRope(player, start, end));
    }

    private IEnumerator MoveAlongRope(Player player, Transform start, Transform end)
    {
        player.SetPlayerState(PlayerState.RopeWalk);

        string animationBoolText = "";
        switch(ropeDir)
        {
            case RopeDir.Horizontal:
                animationBoolText = "Moving";
                break;
            case RopeDir.Vertical:
                animationBoolText = "Climbing";
                break;
        }

        player.Animator.SetBool(animationBoolText, true);

        // 필요하면 플레이어 컨트롤 잠금
        float t = 0f;
        float duration = 1.5f; // 이동 속도

        Vector3 startPos = start.position;
        Vector3 endPos = end.position;

        // 시작 위치 보정
        player.transform.position = startPos;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            Vector3 pos = Vector3.Lerp(startPos, endPos, t);

            player.transform.position = pos;

            yield return null;
        }

        player.transform.position = endPos;

        SnapToEndTop(player, end);

        // 컨트롤 복구
        player.enabled = true;

        player.SetPlayerState(PlayerState.None);

        player.Animator.SetBool(animationBoolText, false);
    }
    private void SnapToEndTop(Player player, Transform end)
    {
        Collider2D col = end.GetComponent<Collider2D>();
        if (col == null) return;

        Bounds b = col.bounds;

        Vector2 targetPos = new Vector2(
            b.center.x,
            b.max.y
        );

        // 플레이어가 collider 위에 "서게" 살짝 올림
        float playerOffset = 0.1f;
        targetPos.y += playerOffset;

        player.transform.position = targetPos;
    }
}