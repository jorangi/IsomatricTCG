using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerAction
{
    useCard,
    move
}
public class Player : Character
{
    public RectTransform CardsUI;
    private PlayerAction act;
    public PlayerAction Act
    {
        get => act;
        set
        {
            if(value == PlayerAction.useCard)
            {
                StartCoroutine(CardsUIShow(true));
            }
            if(act == PlayerAction.useCard && value != PlayerAction.useCard)
            {
                StartCoroutine(CardsUIShow(false));
            }
            act = value;
        }
    } 
    private void Start()
    {
        Pos = new(0, 0, 0);
        SetPosition(Pos);

        GameManager.Inst.battleManager.curCha = this;
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        CardArrange();
    }
    protected override void Update()
    {
        base.Update();

        if (Act == PlayerAction.move && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.forward);
            if (hit)
            {
                string[] a = hit.collider.name.Split('|');
                Vector3Int des = new(Convert.ToInt32(a[0]), Convert.ToInt32(a[1]), Convert.ToInt32(a[2]));
                MovePosition(des);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Act = PlayerAction.useCard;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Act = PlayerAction.move;
        }
    }
    private IEnumerator CardsUIShow(bool show)
    {
        foreach (RectTransform rect in CardsUI)
        {
            rect.GetChild(0).GetComponent<Image>().raycastTarget = false;
        }
        if (!show)
        {
            foreach(RectTransform rect in CardsUI)
            {
                StartCoroutine(CardsShow(show, rect));
                while(rect.anchoredPosition.y > - 150.0f)
                {
                    yield return null;
                }
            }
        }
        else
        {
            foreach (RectTransform rect in CardsUI)
            {
                StartCoroutine(CardsShow(show, rect));
                while (rect.anchoredPosition.y <= -150f)
                {
                    yield return null;
                }
            }
        }
        foreach (RectTransform rect in CardsUI)
        {
            rect.GetChild(0).GetComponent<Image>().raycastTarget = true;
        }
    }
    private IEnumerator CardsShow(bool show, RectTransform rect)
    {
        if(!show)
        {
            while (rect.anchoredPosition.y > -300.0f)
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(rect.anchoredPosition.y, -320.0f, Time.deltaTime * 10.0f));
                yield return null;
            }
            rect.anchoredPosition = new(rect.anchoredPosition.x, -300.0f);
        }
        else
        {
            while (rect.anchoredPosition.y < 0)
            {
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Lerp(rect.anchoredPosition.y, 10f, Time.deltaTime * 10.0f));
                yield return null;
            }
            rect.anchoredPosition = new(rect.anchoredPosition.x, 0f);
        }
    }
    public void CardArrange()
    {
        const float cardWidth = 165.0f;
        const float spacing = 30.0f;
        float boxWidth = (CardsUI.childCount - 1) * (spacing + cardWidth);
        for(int i = 0; i<CardsUI.childCount; i++)
        {
            RectTransform rect = CardsUI.GetChild(i).GetComponent<RectTransform>();
            rect.anchoredPosition = new(-boxWidth / 2 + spacing * i + cardWidth * i, 0f);
        }
    }
}
