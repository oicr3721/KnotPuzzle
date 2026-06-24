using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RopeBridge : MonoBehaviour, IInteractable
{
    private IRopeAttachable holderA;
    private IRopeAttachable holderB;

    [SerializeField] private LineRenderer line;

    public bool CanInteract => enabled;

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

        // 컨트롤 복구
        player.enabled = true;
    }
}