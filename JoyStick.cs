using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public GameObject player;
    private RectTransform rectTransform;
    private RectTransform lever;
    

    Vector2 inputPos;
    Vector2 joyrect_Devision2;   //조이스틱 넓이 나누기2(중앙값 구하기 위해)

    bool isInput = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        lever = transform.GetChild(0).GetComponent<RectTransform>();
        joyrect_Devision2 = new Vector2(rectTransform.rect.width / 2, rectTransform.rect.height / 2);
    }

    void Update()
    {
        if (isInput)
        {
            lever.anchoredPosition = inputPos * joyrect_Devision2; //레버 위치값
            player.transform.position += new Vector3(inputPos.x, 0, inputPos.y) * Time.deltaTime;  //캐릭터 움직임
            player.transform.rotation = Quaternion.LookRotation(new Vector3(inputPos.x, 0, inputPos.y)); //캐릭터 방향
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isInput = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        SetInputPos(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isInput = false;
        lever.anchoredPosition = Vector2.zero;
    }

    void SetInputPos(PointerEventData eventData)
    {
        inputPos = (eventData.position - rectTransform.anchoredPosition - joyrect_Devision2) / joyrect_Devision2;

        //조작범위 벗어날때 값 제한
        if (inputPos.x >= 1)
            inputPos.x = 1;
        else if (inputPos.x <= -1)
            inputPos.x = -1;

        if (inputPos.y >= 1)
            inputPos.y = 1;
        else if (inputPos.y <= -1)
            inputPos.y = -1;
    }
}
