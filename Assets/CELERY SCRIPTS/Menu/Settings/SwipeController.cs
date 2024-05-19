using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    private int maxPage;
    public int currentPage;
    private Vector3 targetPos;
    private Vector3 startRectPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform contentRect;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] float swipeTime;
    private bool isSwiping;
    private void Awake()
    {
        maxPage = contentRect.transform.childCount - 1;
        Instantiate(contentRect.transform.GetChild(0), contentRect).SetAsLastSibling();
        Instantiate(contentRect.transform.GetChild(contentRect.transform.childCount - 1), contentRect).SetAsFirstSibling();
        startRectPos = contentRect.anchoredPosition3D + pageStep;
        currentPage = 0;
    }
    public void Next()
    {
        targetPos += pageStep;
        if (currentPage < maxPage && !isSwiping)
        {
            currentPage++;
            StartCoroutine(MovePage());
        }
        else
        {
            currentPage = 0;
            StartCoroutine(MoveAndLoop(startRectPos));
        }
    }
    public void Previous()
    {
        targetPos -= pageStep;
        if (currentPage > 0 && !isSwiping)
        {
            currentPage--;
            StartCoroutine(MovePage());
        }
        else
        {
            currentPage = maxPage;
            StartCoroutine(MoveAndLoop(startRectPos + pageStep * maxPage));
        }
    }
    public void GoToPage(int page)
    {
        currentPage = page;
        contentRect.anchoredPosition3D = startRectPos + pageStep * page;
        targetPos = contentRect.anchoredPosition3D;
    }
    private IEnumerator MovePage()
    {
        float t = 0;
        isSwiping = true;
        Vector3 startPos = contentRect.anchoredPosition3D;
        while (t < swipeTime)
        {
            t += Time.unscaledDeltaTime;
            float value = t / swipeTime;
            contentRect.anchoredPosition3D = Vector3.Lerp(startPos, targetPos, value);
            yield return null;
        }
        isSwiping = false;
    }
    private IEnumerator MoveAndLoop(Vector2 destination)
    {
        yield return MovePage();
        contentRect.anchoredPosition3D = destination;
        targetPos = destination;
    }
}
