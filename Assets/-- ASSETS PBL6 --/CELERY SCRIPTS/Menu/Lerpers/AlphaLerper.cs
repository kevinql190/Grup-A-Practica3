using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AlphaLerper : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    public IEnumerator LerpAlpha(float time, bool doFadeIn)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float value = t / time;
            canvasGroup.alpha = doFadeIn ? value : 1 - value;
            yield return null;
        }
    }
}
