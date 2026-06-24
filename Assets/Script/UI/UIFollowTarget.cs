using UnityEngine;

public class UIFollowTarget : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Offset")]
    [SerializeField] private Vector3 worldOffset;

    [Header("Camera")]
    [SerializeField] private Camera targetCamera;

    private RectTransform rectTransform;
    private Canvas parentCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    private void LateUpdate()
    {

        if (target == null || targetCamera == null)
            return;

        Vector3 worldPos =
            target.position + worldOffset;

        Vector3 screenPos =
            targetCamera.WorldToScreenPoint(worldPos);

        // Ä«¸Þ¶ó µÚ¿¡ ÀÖÀ¸¸é UI ¼û±è
        if (screenPos.z < 0f)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            rectTransform.position = screenPos;
        }
        else
        {
            RectTransform canvasRect =
                parentCanvas.transform as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPos,
                parentCanvas.worldCamera,
                out Vector2 localPos
            );

            rectTransform.localPosition = localPos;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}