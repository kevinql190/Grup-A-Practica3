using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UILerper : MonoBehaviour
{
    #region Panel Lerp
    public IEnumerator LerpPanel(GameObject fadeout, GameObject fadein, float fadeoutTime, float fadeinTime, float inbetweenTime, GameObject firstSelected = null)
    {
        float timer = 0f;
        while (timer < fadeoutTime) //Fade out
        {
            timer += Time.deltaTime;
            float progress = timer / fadeoutTime;
            fadeout.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(1, 0, progress);
            yield return null;
        }
        fadeout.SetActive(false);
        yield return new WaitForSeconds(inbetweenTime);
        fadein.SetActive(true);
        if(firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
        timer = 0f;
        while (timer < fadeinTime) //Fade out
        {
            timer += Time.deltaTime;
            float progress = timer / fadeinTime;
            fadein.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, progress);
            yield return null;
        }
    }
    #endregion
}
