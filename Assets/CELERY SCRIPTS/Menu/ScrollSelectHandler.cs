using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSelectHandler : MonoBehaviour
{
    [Header("Receptari Scroll")]
    private ScrollRect scrollRect;
    private GameObject lastSelected;
    private RectTransform contentPanel;
    private Vector2 scrollDestiny;
    private Vector2 refVel;
    private void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentPanel = scrollRect.content;
        lastSelected = EventSystem.current.currentSelectedGameObject;
    }
    private void Update()
    {
        if (IsUsingGamepad())
        {
            scrollRect.scrollSensitivity = 0;
            ReceptariScrollHeight();
            ScrollReceptari();
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            scrollRect.scrollSensitivity = 15;
        }
    }
    private void ReceptariScrollHeight()
    {
        RectTransform selectedRectTransform = lastSelected.GetComponent<RectTransform>();
        float selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;
        float scrollViewMinY = contentPanel.anchoredPosition.y;
        float scrollViewMaxY = contentPanel.anchoredPosition.y + scrollRect.GetComponent<RectTransform>().rect.height;
        if (selectedPositionY > scrollViewMaxY) //Baixar
        {
            float newY = selectedPositionY + selectedRectTransform.rect.height / 1.5f - scrollRect.GetComponent<RectTransform>().rect.height;
            scrollDestiny = new Vector2(contentPanel.anchoredPosition.x, newY);
        }
        else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) < scrollViewMinY) //Pujar
        {
            scrollDestiny = new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y) - selectedRectTransform.rect.height *1.5f);
        }
    }
    private void ScrollReceptari()
    {
        contentPanel.anchoredPosition = Vector2.SmoothDamp(contentPanel.anchoredPosition, scrollDestiny, ref refVel, 0.2f, Mathf.Infinity, Time.unscaledDeltaTime);
    }
    private bool IsUsingGamepad()
    {
        return PlayerInputHandler.CurrentControlScheme != "Keyboard Mouse";
    }
}
