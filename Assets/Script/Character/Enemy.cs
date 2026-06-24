using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Enemy : Character, IInteractable
{
    [SerializeField] private PlayableDirector director;
    public override bool CanMove => true;

    public bool CanInteract => true;

    public Transform InteractionPoint => transform;

    [SerializeField] private string interactionText = "Á×¿³!";
    public string InteractionText => interactionText;

    public event Action OnDead;

    [SerializeField] private string nextChapterName = "Chapter";

    public void Interact(Player player)
    {
        OnDead?.Invoke();
        director?.Play();
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
        Debug.Log("ÄÆ½Å ³¡");
        FadeManager.Instance.LoadSceneWithFade(nextChapterName);
    }
}
