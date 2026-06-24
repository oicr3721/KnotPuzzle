using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChapterStory : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private TypewriterEffect writer;
    [SerializeField] private GameObject enemy;

    [Header("Story")]
    [SerializeField] private List<Storyline> lines = new List<Storyline>();

    private void Start()
    {
        enemy.SetActive(false);
        StartCoroutine(PlayStory());
    }

    private IEnumerator PlayStory()
    {
        foreach (var line in lines)
        {
            writer.StartTyping(line.text);

            yield return new WaitUntil(() => !writer.IsTyping);

            yield return new WaitForSeconds(line.delayAfterText);

            if (line.spawnEnemy)
            {
                ThunderSpawn();
            }
        }
    }
    private void ThunderSpawn()
    {
        enemy.SetActive(true);

        Transform t = enemy.transform;

        Vector3 endPos = t.position;
        Vector3 startPos = endPos + Vector3.up * 8f;

        t.position = startPos;
        t.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        seq.Append(t.DOMove(endPos, 0.25f).SetEase(Ease.InQuad));

        seq.Append(t.DOScaleY(0.6f, 0.05f));
        seq.Join(t.DOScaleX(1.4f, 0.05f));

        seq.Append(t.DOScale(1.6f, 0.12f).SetEase(Ease.OutBack));

        seq.Append(t.DOScale(1f, 0.08f));

        seq.JoinCallback(() =>
        {
            Camera.main.transform.DOShakePosition(0.35f, 0.6f);
            Camera.main.DOFieldOfView(70, 0.15f).From(60);
        });
    }
}