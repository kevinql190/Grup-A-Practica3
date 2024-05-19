using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ReceptariButtonHandler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        eventData.selectedObject = gameObject;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        eventData.selectedObject = transform.parent.transform.parent.GetChild(0).gameObject;
    }

}
