using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackFade : MonoBehaviour
{
    [SerializeField] private Image blackImage;
    [SerializeField] private AnimationCurve curveToBlack;
    [SerializeField] private AnimationCurve curveFromBlack;
    public IEnumerator FadeToBlack(float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float value = curveToBlack != null ? curveToBlack.Evaluate(t / time) : t / time;
            blackImage.color = new Color(0, 0, 0, value);
            yield return null;
        }
    }
    public IEnumerator FadeFromBlack(float time)
    {
        float t = 0;
        while (t < time)
        {
            t += Time.unscaledDeltaTime;
            float value = curveFromBlack != null ? curveFromBlack.Evaluate(t / time) : t / time;
            blackImage.color = new Color(0, 0, 0, 1 - value);
            yield return null;
        }
    }
}
