using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EmojiPlayer : MonoBehaviour
{

    [SerializeField] DOTweenAnimation[] Emojis;
    int emojiNum;
    int prevDialogue = -1;

    bool emojiIsActive;
    private void OnCollisionEnter(Collision collision)
    {
        if (!emojiIsActive)
            StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        emojiIsActive = true;
        emojiNum = Random.Range(0, Emojis.Length);
        do
        {
            emojiNum = Random.Range(0, Emojis.Length);
            print(emojiNum);
        } while (emojiNum == prevDialogue);

        prevDialogue = emojiNum;
        Emojis[emojiNum].gameObject.SetActive(true);
        Emojis[emojiNum].DORestart();
        yield return new WaitForSeconds(2f);
        Emojis[emojiNum].gameObject.SetActive(false);
        emojiIsActive = false;
    }
}
