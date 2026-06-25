using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Enemy : Character, IInteractable
{
    [SerializeField] private PlayableDirector director;
    public override bool CanMove => true;

    public bool CanInteract => canInteract;

    [SerializeField] private bool canInteract = true;
    public Transform InteractionPoint => transform;

    [SerializeField] private string interactionText = "Á×¿³!";
    public string InteractionText => interactionText;

    public event Action OnDead;
    [SerializeField] private bool deadDestroy = false;

    [SerializeField] private string nextChapterName = "Chapter";

    public void Interact(Player player)
    {
        OnDead?.Invoke();
        if(director != null)
            director?.Play();

        if (deadDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (director == null) return;
        director.stopped += OnCutsceneEnd;
    }

    private void OnDisable()
    {
        if (director == null) return;
        director.stopped -= OnCutsceneEnd;
    }

    private void OnCutsceneEnd(PlayableDirector d)
    {
        FadeManager.Instance.LoadSceneWithFade(nextChapterName);
    }
}
