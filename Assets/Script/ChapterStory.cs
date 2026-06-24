using System.Collections;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;

public class ChapterStory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI storyTMP;
    [SerializeField] private float textDelay = 0.04f;
    [SerializeField] private GameObject enemy =null;

    [TextArea]
    [SerializeField] private string storyText;
    [SerializeField] private string stageSceneName;

    private string buffer = "";
    private int cursor = 0;

    private void Start()
    {
        enemy?.SetActive(false);
        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        yield return Process(storyText);
        FadeManager.Instance.LoadSceneWithFade(stageSceneName);
    }

    private IEnumerator Process(string input)
    {
        var regex = new Regex(@"\[(.*?)\]");

        foreach (Match match in regex.Matches(input))
        {
            string normal = input.Substring(cursor, match.Index - cursor);

            if (!string.IsNullOrEmpty(normal))
            {
                yield return TypeAdd(normal);
            }

            cursor = match.Index + match.Length;

            yield return Execute(match.Groups[1].Value);
        }

        if (cursor < input.Length)
        {
            yield return TypeAdd(input.Substring(cursor));
        }
    }

    private IEnumerator Execute(string command)
    {
        string[] parts = command.Split(' ');

        switch (parts[0])
        {
            case "remove":
                yield return TypeAdd(parts[1]);
                yield return RemoveWord(parts[1], float.Parse(parts[2]));
                break;

            case "wait":
                yield return new WaitForSeconds(float.Parse(parts[1]));
                break;

            case "spawn":
                yield return SpawnEnemy();
                break;
        }
    }

    // =========================
    // 타자기
    // =========================
    private IEnumerator TypeAdd(string text)
    {
        foreach (char c in text)
        {
            buffer += c;
            storyTMP.text = buffer;
            yield return new WaitForSeconds(textDelay);
        }
    }

    // =========================
    // remove
    // =========================
    private IEnumerator RemoveWord(string word, float delay)
    {
        int index = buffer.IndexOf(word);
        if (index < 0)
            yield break;

        yield return new WaitForSeconds(delay);

        buffer = buffer.Remove(index, word.Length);
        storyTMP.text = buffer;
    }

    // =========================
    // spawn (콰과광 등장)
    // =========================
    private IEnumerator SpawnEnemy()
    {
        enemy?.SetActive(true);

        Transform t = enemy.transform;

        Vector3 endPos = t.position;
        Vector3 startPos = endPos + Vector3.up * 12f;

        t.position = startPos;
        t.localScale = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        // 급강하
        seq.Append(t.DOMove(endPos, 0.25f).SetEase(Ease.InQuad));

        // 충격 압축
        seq.Append(t.DOScale(new Vector3(1.8f, 0.6f, 1f), 0.05f));

        // 튀어오름 (임팩트)
        seq.Append(t.DOScale(2.5f, 0.22f).SetEase(Ease.OutBack));

        // 안정화
        seq.Append(t.DOScale(1f, 0.08f));

        // 카메라 흔들림
        seq.JoinCallback(() =>
        {
            Camera.main.transform.DOShakePosition(0.4f, 0.6f);
        });

        yield return seq.WaitForCompletion();
    }
}