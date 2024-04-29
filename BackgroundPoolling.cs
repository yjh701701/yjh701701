using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPoolling : MonoBehaviour
{
    List<RectTransform> background = new List<RectTransform>();

    float obj_width = 4500f;
    float backgroundMoveSpeed = 5f;

    private void Awake()
    {
        int childCount = transform.childCount;
        for(int i=0; i<childCount; i++)
        {
            background.Add(transform.GetChild(i).GetComponent<RectTransform>());
            background[i].localPosition = new Vector3( i * obj_width, 0, 0);

        }
    }

    void FixedUpdate()
    {
        foreach(RectTransform t in background)
        {
            t.localPosition -= new Vector3(backgroundMoveSpeed,0,0);

            if(t.localPosition.x <= -obj_width ) 
            {
                float xPos = ((background.Count-1) * obj_width);
                t.localPosition = new Vector3 (xPos,0,0);
            }
        }

        
    }
}
