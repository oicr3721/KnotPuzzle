using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RopeActionController : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private RopeBridge ropeBridgePrefab;
    [SerializeField] private float ropeRange;
    [SerializeField] private Transform mousePoint;
    [SerializeField] private float targetDetectRadius = 0.3f;

    [SerializeField] private Sprite enableSprite;
    [SerializeField] private Sprite disableSprite;

    private SpriteRenderer mouseRenderer;
    private Camera cam;

    private IRopeAttachable target;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        mouseRenderer = mousePoint.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player.PlayerState != PlayerState.RopeReady) return;

        UpdateUI();
        UpdateTarget();

        if(target != null)
            mouseRenderer.sprite = enableSprite;
        else
            mouseRenderer.sprite = disableSprite;
    }

    private void UpdateUI()
    {

        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = Mathf.Abs(cam.transform.position.z);

        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;

        Vector2 holderPos = player.RopeHolder.AttachedPoint.position;
        Vector2 dir = (mouseWorldPos - (Vector3)holderPos);

        if (dir.magnitude > ropeRange)
        {
            mouseWorldPos = holderPos + dir.normalized * ropeRange;
        }

        mousePoint.position = mouseWorldPos;
    }

    private void UpdateTarget()
    {
        target = null;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                mousePoint.position,
                targetDetectRadius);

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<RopeAttachableTile>(out var attachable))
                continue;

            if (attachable == player.RopeHolder)
                continue;

            if (attachable.RopeDir != player.RopeHolder.RopeDir)
                continue;

            target = attachable;
            break;
        }
    }

    public void RopeConnectInput(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        ConnectRope();
    }

    private void ConnectRope()
    {
        if (cam == null || player.PlayerState != PlayerState.RopeReady)
            return;

        if(target == null)
        {
            //타깃 없으면 취소처리
            player.CancleRopeConnect();
            return;
        }

        RopeBridge ropeBridge = Instantiate(ropeBridgePrefab);
        ropeBridge.Init(target, player.RopeHolder);

        StartCoroutine(player.CompleteRopeConnect());
    }

    private void OnDrawGizmos()
    {
        // 마우스 포인트 탐지 범위
        if (mousePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                mousePoint.position,
                targetDetectRadius);
        }

        // 로프 최대 거리
        if (player != null && player.RopeHolder != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(
                player.RopeHolder.AttachedPoint.position,
                ropeRange);
        }
    }
}