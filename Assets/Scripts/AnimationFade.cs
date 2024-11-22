using UnityEngine;

public class AnimationFade : MonoBehaviour
{
    public static AnimationFade instance { get; private set; }
    public CanvasGroup canvasGroup;
    public bool fadeIn = false, fadeOut = false;

    public float timeToFade;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (fadeIn)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += timeToFade * Time.deltaTime;
                if (canvasGroup.alpha >= 1) { fadeIn = false; canvasGroup.alpha = 0; }
            }
        }
        if (fadeOut)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= timeToFade * Time.deltaTime;
                if (canvasGroup.alpha == 0) { fadeOut = false; }
            }
        }
    }
    public void FadeIn() { fadeIn = true; }
    public void FadeOut() { fadeOut = true; if (canvasGroup.alpha < 1) canvasGroup.alpha = 1; }
}
