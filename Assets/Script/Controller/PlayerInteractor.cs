using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private float interactRange = 1f;
    [SerializeField] private LayerMask interactableMask;

    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TextMeshProUGUI interactionText;

    private IInteractable target;

    // Update is called once per frame
    void Update()
    {
        DetectInteractable();

        if (target != null && target.CanInteract && Input.GetKeyDown(KeyCode.F))
        {
            target.Interact(player);
        }
    }

    void DetectInteractable()
    {
        target = null;
        float closestDist = float.MaxValue;

        var hits = Physics2D.OverlapCircleAll(player.transform.position, interactRange, interactableMask);

        foreach (var hit in hits)
        {
            if (hit.gameObject == this) continue;
            var interactable = hit.GetComponent<IInteractable>();
            if (interactable != null && interactable.CanInteract)
            {
                float dist = Vector2.Distance(player.transform.position, hit.transform.position);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    target = interactable;
                }
            }
        }

        if (target != null)
            UpdateUI();
        else if (interactionPrompt.activeSelf)
            interactionPrompt.SetActive(false);
    }

    private void UpdateUI()
    {
        string text = target.InteractionText;

        interactionText.text = text;

        if (!interactionPrompt.activeSelf)
            interactionPrompt.SetActive(true);
    }


}