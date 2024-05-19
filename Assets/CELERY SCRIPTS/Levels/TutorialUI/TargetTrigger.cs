using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetTrigger : MonoBehaviour
{
    [SerializeField] private GameObject targetCanvas;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float scaleSpeed;
    [SerializeField] private float scaleAmount;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float dampSmoothTime;
    private Vector3 dampRelVel;
    private bool isAnyTutorialShown;
    private bool isPlayerInside;
    private Vector3 targetScale;
    [SerializeField] bool showOnlyWithSkill;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (showOnlyWithSkill && !other.GetComponent<SkillAbilities>().isSkillActive)
            {
                HideTarget();
                return;
            }
            ShowTarget();
            isPlayerInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideTarget();
            isPlayerInside = false;
        }
    }
    private void OnDestroy()
    {
        if (isPlayerInside) GameObject.FindGameObjectWithTag("Player").GetComponent<TutorialController>().HideTutorial();
    }
    public void ShowTarget()
    {
        targetScale = new Vector3(1, 1, 1);
        if (!isAnyTutorialShown)
            StartCoroutine(ShowingTutorial());
    }
    private IEnumerator ShowingTutorial()
    {
        targetObject.SetActive(true);
        targetCanvas.transform.localScale = new Vector3(0, 0, 0);
        isAnyTutorialShown = true;
        float t = 0;
        while (isAnyTutorialShown)
        {
            t += Time.deltaTime;
            float scaleFactor = Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;
            targetCanvas.transform.localScale = Vector3.SmoothDamp(targetCanvas.transform.localScale, targetScale + Vector3.one * scaleFactor, ref dampRelVel, dampSmoothTime);
            targetObject.transform.Rotate(transform.right, Time.deltaTime * rotationSpeed);
            targetCanvas.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            yield return null;
        }
    }
    public void HideTarget()
    {
        targetScale = new Vector3(0, 0, 0);
        StartCoroutine(HidingTutorial());
    }
    private IEnumerator HidingTutorial()
    {
        while (targetScale.sqrMagnitude == 0)
        {
            if (targetCanvas.transform.localScale.sqrMagnitude < 0.1)
            {
                targetObject.SetActive(false);
                isAnyTutorialShown = false;
            }
            yield return null;
        }
    }
}
