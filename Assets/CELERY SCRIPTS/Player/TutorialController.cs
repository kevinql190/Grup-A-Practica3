using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private List<GameObject> tutorials;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float scaleAmount;
    [SerializeField] private float dampSmoothTime;
    private Vector3 dampRelVel;
    private bool isAnyTutorialShown;
    private int currentId = -1;
    private Vector3 targetScale;
    public void ShowTutorial(int tutorialid)
    {
        if(currentId == -1)
            targetScale = new Vector3(1, 1, 1);
        if(!isAnyTutorialShown)
            StartCoroutine(ShowingTutorial(tutorialid));
    }
    private IEnumerator ShowingTutorial(int id)
    {
        currentId = id;
        tutorials[id].SetActive(true);
        tutorialCanvas.transform.localScale = new Vector3(0, 0, 0);
        isAnyTutorialShown = true;
        float t = 0;
        while (isAnyTutorialShown)
        {
            t += Time.deltaTime;
            float scaleFactor = Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
            tutorialCanvas.transform.localScale = Vector3.SmoothDamp(tutorialCanvas.transform.localScale ,targetScale + Vector3.one * scaleFactor, ref dampRelVel, dampSmoothTime);
            tutorialCanvas.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            yield return null;
        }
        currentId = -1;
    }
    public void HideTutorial()
    {
        targetScale = new Vector3(0, 0, 0);
        StartCoroutine(HidingTutorial());
    }
    private IEnumerator HidingTutorial()
    {
        while(targetScale.sqrMagnitude == 0 && currentId != -1)
        {
            if (tutorialCanvas.transform.localScale.sqrMagnitude < 0.1)
            {
                tutorials[currentId].SetActive(false);
                isAnyTutorialShown = false;
            }
            yield return null;
        }
    }
}
