using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    private int siblingNum;
    private Vector2 startPoint, moveOffest, moveBegin;
    public RectTransform rect;
    public TextMeshProUGUI cardName, cardDesc;
    public Image rayTarget, cardBase, cardIllust;

    public IEnumerator ReturnPos()
    {
        rayTarget.raycastTarget = false;
        while((rect.anchoredPosition - startPoint).sqrMagnitude > 0.01f)
        {
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, startPoint, Time.deltaTime * 10f);
            yield return null;
        }
        rect.anchoredPosition = startPoint;
        rayTarget.raycastTarget = true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        siblingNum = transform.parent.GetSiblingIndex();
        transform.SetAsLastSibling();
        transform.GetChild(0).localScale = new(1.5f, 1.5f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(siblingNum);
        transform.GetChild(0).localScale = new(1f, 1f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        foreach(RectTransform sibling in rect.parent)
        {
            if (sibling != rect)
            {
                sibling.GetChild(0).GetComponent<Image>().raycastTarget = false;
            }
        }
        startPoint = rect.anchoredPosition;
        moveBegin = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (RectTransform sibling in rect.parent)
        {
            if (sibling != rect)
            { 
                sibling.GetChild(0).GetComponent<Image>().raycastTarget = true;
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        moveOffest = (eventData.position - moveBegin) * new Vector2(1920.0f / Screen.width, 1080.0f / Screen.height);
        rect.anchoredPosition = startPoint + moveOffest;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(ReturnPos());
    }
}
