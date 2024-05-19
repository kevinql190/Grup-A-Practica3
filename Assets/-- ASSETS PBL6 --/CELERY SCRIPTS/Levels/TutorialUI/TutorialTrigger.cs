using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private int tutorialId;
    private bool isPlayerInside;
    private TutorialController tutorial;
    private void Awake()
    {
        tutorial = GameObject.FindGameObjectWithTag("Player").GetComponent<TutorialController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<TutorialController>().ShowTutorial(tutorialId);
            isPlayerInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<TutorialController>().HideTutorial();
            isPlayerInside = false;
        }
    }
    private void OnDestroy()
    {
        if (isPlayerInside && tutorial!=null)
            tutorial.HideTutorial();
    }
}
