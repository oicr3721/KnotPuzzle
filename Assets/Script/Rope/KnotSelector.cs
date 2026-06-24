using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class KnotSelector : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject selectUI;
    [SerializeField] private List<Button> knotButtons;
    [SerializeField] private TextMeshProUGUI selectInfoText;
    private KnotType? selectedKnot = null;

    public void SelectKnot(int knot)
    {
        selectedKnot = (KnotType)knot;
    }

    public IEnumerator SelectUseKnot()
    {
        player.SetPlayerState(PlayerState.Select);

        selectedKnot = null;

        List<KnotType> selectableKnots = new();

        foreach (var pair in player.KnotState)
        {
            knotButtons[(int)pair.Key].interactable = pair.Value;
        }

        selectInfoText.text = "»ç¿ëÇÒ ¸ÅµìÀ» °ñ¶óÁÖ¼¼¿ä";
        OpenKnotSelectUI();

        yield return new WaitUntil(() => selectedKnot.HasValue);

        player.UseKnot(selectedKnot.Value);

        CloseKnotSelectUI();

        player.SetPlayerState(PlayerState.None);
    }

    public IEnumerator SelectGetKnot()
    {
        player.SetPlayerState(PlayerState.Select);

        selectedKnot = null;

        foreach (var pair in player.KnotState)
        {
            knotButtons[(int)pair.Key].interactable = !pair.Value;
        }

        selectInfoText.text = "¸ÅµìÀ» ¾îµð¿¡ ÀåÂøÇÒÁö °ñ¶óÁÖ¼¼¿ä";
        OpenKnotSelectUI();

        yield return new WaitUntil(() => selectedKnot.HasValue);

        player.EquipKnot(selectedKnot.Value);

        CloseKnotSelectUI();

        player.SetPlayerState(PlayerState.None);
    }

    private void OpenKnotSelectUI()
    {
        selectUI.SetActive(true);
    }

    private void CloseKnotSelectUI()
    {
        selectUI.SetActive(false);
    }
}
