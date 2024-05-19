using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapyrusScroll : MonoBehaviour
{
    [Header("Page Scroll")]
    [SerializeField] private GameObject paperGameobject;
    [SerializeField] private float paperMaxScroll;
    [SerializeField] private GameObject scrollViewport;
    private Vector3 scrollStartPosition;
    private Vector3 paperStartPosition;
    private void Start()
    {
        scrollStartPosition = scrollViewport.transform.position;
        paperStartPosition = paperGameobject.transform.position;
    }
    public void ScrollPage(Vector2 scrollVector)
    {
        paperGameobject.transform.localPosition = Vector3.Lerp(paperStartPosition - paperGameobject.transform.up * paperMaxScroll, paperStartPosition, scrollVector.y);
    }
    private void OnDisable()
    {
        scrollViewport.transform.position = scrollStartPosition;
    }
}
