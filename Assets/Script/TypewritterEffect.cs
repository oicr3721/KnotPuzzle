using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text textUI;
    [SerializeField] private float typingSpeed = 0.05f;

    public void StartTyping(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(message));
    }

    private bool isTyping;
    private IEnumerator TypeText(string message)
    {
        isTyping = true;

        textUI.text = message;
        textUI.ForceMeshUpdate();

        textUI.maxVisibleCharacters = 0;

        int total = message.Length;

        for (int i = 0; i <= total; i++)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                textUI.maxVisibleCharacters = total;
                break;
            }

            textUI.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}