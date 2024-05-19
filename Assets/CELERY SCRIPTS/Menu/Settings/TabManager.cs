using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    public void SwitchToTab(int TabID)
    {
        foreach(GameObject tab in tabs)
        {
            tab.SetActive(false);
        }
        tabs[TabID].SetActive(true);
    }
}
