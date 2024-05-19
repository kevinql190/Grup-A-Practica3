using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoSettingFullScreen : MonoBehaviour
{
    private int CurrentPage => GetComponent<SwipeController>().currentPage;
    private void Start()
    {
        if (!Screen.fullScreen) GetComponent<SwipeController>().GoToPage(1);
    }
    public void ChangeFullScreenSetting()
    {
        Screen.fullScreen = CurrentPage == 1;
    }
}