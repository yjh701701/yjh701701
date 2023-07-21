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
    Vector2 joyrect_Devision2;   //���̽�ƽ ���� ������2(�߾Ӱ� ���ϱ� ����)

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
            lever.anchoredPosition = inputPos * joyrect_Devision2; //���� ��ġ��
            player.transform.position += new Vector3(inputPos.x, 0, inputPos.y) * Time.deltaTime;  //ĳ���� ������
            player.transform.rotation = Quaternion.LookRotation(new Vector3(inputPos.x, 0, inputPos.y)); //ĳ���� ����
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

        //���۹��� ����� �� ����
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
