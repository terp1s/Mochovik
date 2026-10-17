using System.Collections;
using UnityEngine;

public class SnailEmotes : MonoBehaviour
{
    [SerializeField] private GameObject smallBubble;
    [SerializeField] private GameObject mediumBubble;
    [SerializeField] private GameObject bigBubble;

    [SerializeField] private GameObject lightBulb;

    [SerializeField] private GameObject hunger;

    [SerializeField] float bubbleSpacing = 0.5f;
    [SerializeField] float bubbleDuration = 2f;
    [SerializeField] float hungerDuration = 2f;

    public void EmoteHunger()
    {
        if (hunger != null)
        {
            hunger.SetActive(true);
            StartCoroutine(HandleDuration(hunger, hungerDuration));
        }
    }

    public void EmoteBubbles()
    {
        GameObject[] bubbles = new GameObject[] { bigBubble , mediumBubble, smallBubble };
        StartCoroutine(HandleBubbleSequence(bubbles, bubbleSpacing, bubbleDuration));
    }

    private IEnumerator HandleBubbleSequence(GameObject[] bubbles, float spacing, float duration)
    {
        for (int i = bubbles.Length - 1; i >= 0; i--)
        {
            if (bubbles[i] != null)
            {
                bubbles[i].SetActive(true);
                StartCoroutine(HandleDuration(bubbles[i], duration));
                yield return new WaitForSeconds(spacing);
            }
        }
    }

    private IEnumerator HandleDuration(GameObject emote, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (emote != null)
        {
            emote.SetActive(false);
        }

        if(emote == hunger)
            EmoteBubbles();
    }
}
