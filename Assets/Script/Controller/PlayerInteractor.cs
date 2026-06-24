using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("Mouse Point")]
    [SerializeField] private Transform mousePoint;
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private float detectRadius = 0.3f;

    [SerializeField] private LayerMask interactableMask;

    [Header("UI")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TextMeshProUGUI interactionText;

    private bool canInteract =>
        (player.PlayerState != PlayerState.None)
        && (player.KnotState[KnotType.Arms] == true);

    private Camera cam;
    private IInteractable target;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (!canInteract)
        {
            HideUI();
            return;
        }

        UpdateMousePoint();
        DetectInteractable();

        if (target != null &&
            target.CanInteract &&
            Input.GetKeyDown(KeyCode.F))
        {
            target.Interact(player);
        }
    }

    private void UpdateMousePoint()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(cam.transform.position.z);

        Vector3 mouseWorldPos =
            cam.ScreenToWorldPoint(mouseScreenPos);

        mouseWorldPos.z = 0f;

        Vector2 playerPos = player.transform.position;
        Vector2 dir = mouseWorldPos - (Vector3)playerPos;

        if (dir.magnitude > interactRange)
        {
            mouseWorldPos =
                playerPos +
                dir.normalized * interactRange;
        }

        mousePoint.position = mouseWorldPos;
    }

    private void DetectInteractable()
    {
        target = null;

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                mousePoint.position,
                detectRadius,
                interactableMask);

        float closestDist = float.MaxValue;

        foreach (var hit in hits)
        {
            if (!hit.TryGetComponent<IInteractable>(out var interactable))
                continue;

            if (!interactable.CanInteract)
                continue;

            float dist = Vector2.Distance(
                mousePoint.position,
                hit.transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                target = interactable;
            }
        }

        if (target != null)
            UpdateUI();
        else
            HideUI();
    }

    private void UpdateUI()
    {
        interactionText.text = target.InteractionText;

        if (!interactionPrompt.activeSelf)
            interactionPrompt.SetActive(true);

        interactionPrompt.transform.position =
            mousePoint.position + Vector3.up * 0.5f;
    }

    private void HideUI()
    {
        if (interactionPrompt.activeSelf)
            interactionPrompt.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (mousePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                mousePoint.position,
                detectRadius);
        }

        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(
                player.transform.position,
                interactRange);
        }
    }
}