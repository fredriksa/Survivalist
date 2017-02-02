using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

    private float timeTilFadeOut = 0;
    private float fadeOutDuration = 0;

    private void Awake()
    {
        foreach (CanvasRenderer renderer in getCanvasRenderers())
            renderer.SetAlpha(0);

        foreach (Text text in GetComponentsInChildren<Text>())
            text.CrossFadeAlpha(0, 1 / 100000, false);

        foreach (Image image in getImages())
            image.CrossFadeAlpha(0, 1 / 100000, false);
    }

    private void Update()
    {
        if (timeTilFadeOut >= 0)
        {
            timeTilFadeOut -= Time.deltaTime;
        }

        if (timeTilFadeOut < 0 && fadeOutDuration != 0)
        {
            fadeOut(fadeOutDuration);
            fadeOutDuration = 0;
        }
    }

    public void fade(float fadeInTime, float fadeOutTime, float fadeBreak = 0)
    {
        fadeIn(fadeInTime);
        timeTilFadeOut = fadeOutTime;
        fadeOutDuration = fadeInTime + fadeBreak;
    }

    public void fadeIn(float time)
    {
        foreach (Image image in getImages())
            image.CrossFadeAlpha(1, time, false);

        foreach (Text text in GetComponentsInChildren<Text>())
            text.CrossFadeAlpha(1, time, false);
    }

    public void fadeOut(float time)
    {
        foreach (Image image in getImages())
            image.CrossFadeAlpha(0, time, false);

        foreach (Text text in GetComponentsInChildren<Text>())
            text.CrossFadeAlpha(0, time, false);
    }

    private CanvasRenderer[] getCanvasRenderers()
    {
        CanvasRenderer[] renderers = GetComponentsInChildren<CanvasRenderer>();
        renderers[renderers.Length - 1] =  GetComponent<CanvasRenderer>();
        return renderers;
    }

    private Image[] getImages()
    {
        Image[] images = GetComponentsInChildren<Image>();
        images[images.Length - 1] = GetComponent<Image>();
        return images;
    }
}
