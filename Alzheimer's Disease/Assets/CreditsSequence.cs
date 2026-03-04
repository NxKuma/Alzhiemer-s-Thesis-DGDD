using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CinematicCredits : MonoBehaviour
{
    [Header("1. Title Settings")]
    public Image titleImage;
    public float titleFadeInTime = 2f;
    public float titleStayTime = 3f;
    public float titleFadeOutTime = 1.5f;

    [Header("2. Scroll Settings")]
    public RectTransform creditsTextRect;
    public float scrollSpeed = 100f;
    public float startYPos = -800f;
    public float endYPos = 1200f;

    [Header("3. Final Scene Fade")]
    public CanvasGroup finalSceneBlackout; 
    public float finalFadeTime = 2.5f;

    void Start()
    {
        if (titleImage != null) SetAlpha(titleImage, 0);
        if (finalSceneBlackout != null) finalSceneBlackout.alpha = 0;

        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        yield return StartCoroutine(FadeImage(titleImage, 0, 1, titleFadeInTime));
        yield return new WaitForSeconds(titleStayTime);
        yield return StartCoroutine(FadeImage(titleImage, 1, 0, titleFadeOutTime));

        creditsTextRect.anchoredPosition = new Vector2(creditsTextRect.anchoredPosition.x, startYPos);
        while (creditsTextRect.anchoredPosition.y < endYPos)
        {
            creditsTextRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }

        yield return StartCoroutine(FadeImage(titleImage, 0, 1, titleFadeInTime));
        yield return new WaitForSeconds(titleStayTime);

        float elapsed = 0;
        while (elapsed < finalFadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / finalFadeTime;
            SetAlpha(titleImage, Mathf.Lerp(1, 0, t));
            if (finalSceneBlackout != null) finalSceneBlackout.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }
    }

    IEnumerator FadeImage(Image img, float start, float end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            SetAlpha(img, Mathf.Lerp(start, end, elapsed / duration));
            yield return null;
        }
        SetAlpha(img, end);
    }

    void SetAlpha(Image img, float alpha)
    {
        if (img == null) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}