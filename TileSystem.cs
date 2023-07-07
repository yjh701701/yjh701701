using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class TileSystem : MonoBehaviour
{
    List<GameObject> tileObjList = new List<GameObject>();
    List<TileState> tileStateList = new List<TileState>();
    public GameObject player;
   
    float tileSize_x = 50f; //가로 타일 사이즈
    float tileSize_z = 50f; //세로 타일 사이즈
    int tileNum_x = 3; //가로 타일 개수 (고정
    int tileNum_z = 3; //세로 타일 개수 (고정

    Vector3 playerPos;
    float moveMin_x;
    float moveMax_x;
    float moveMin_z;
    float moveMax_z;

    void Start()
    {
        Init();
    }
    private void Update()
    {
        playerMoveChk();
    }

    void Init()
    {
        TileSet();
        Vector3 startPosition = new Vector3(tileNum_x * (tileSize_x / 2) , 0 , tileNum_z * (tileSize_z / 2));
        player.transform.position = startPosition;
        moveMin_x = startPosition.x - (tileSize_x / 2);
        moveMax_x = startPosition.x + (tileSize_x / 2);
        moveMin_z = startPosition.z - (tileSize_z / 2);
        moveMax_z = startPosition.z + (tileSize_z / 2);
    }

    void TileSet()
    {
        GameObject[,] tile= new GameObject[tileNum_x, tileNum_z];
        
        int count = 0;
        for(int j=0; j<tileNum_x; j++)
        {
            for(int k = 0; k < tileNum_z; k++)
            {
                tile[j, k] = transform.GetChild(count).gameObject;
                tile[j, k].AddComponent<TileState>();
                count++;
                tile[j, k].transform.position = new Vector3(k * tileSize_x + tileSize_x/2, 0, j * tileSize_z + tileSize_z/2);
                if(k == 0) { tile[j, k].GetComponent<TileState>().isLeft = true; }
                else if(k == tileNum_z - 1) { tile[j, k].GetComponent<TileState>().isRight = true; }
                if(j == 0) { tile[j, k].GetComponent<TileState>().isDown = true; }
                else if(j == tileNum_x - 1) { tile[j, k].GetComponent<TileState>().isUp = true; }

                tileObjList.Add(tile[j, k]);
                tileStateList.Add(tile[j, k].GetComponent<TileState>());
            } 
        }
        //tile[i].transform.position = new Vector3(-tileSize_x, 0, tileNum_z);


    }

    void playerMoveChk()
    {
        playerPos = player.transform.position;
        if(playerPos.x > moveMax_x) //R
        {
            TileMoveChk(0);
            moveMin_x += tileSize_x;
            moveMax_x += tileSize_x;
        }
        if(playerPos.x <= moveMin_x) //L
        {
            TileMoveChk(1);
            moveMin_x -= tileSize_x;
            moveMax_x -= tileSize_x;
        }
        if(playerPos.z > moveMax_z) //U
        {
            TileMoveChk(2);
            moveMin_z += tileSize_z;
            moveMax_z += tileSize_z;
        }
        if(playerPos.z <= moveMin_z) //D
        {
            TileMoveChk(3);
            moveMin_z -= tileSize_z;
            moveMax_z -= tileSize_z;
        }
    }

    void TileMoveChk(int direction)  // 일단 3x3 기준    // 0 R   1 L    2 U   3 D
    {
        switch (direction)
        {
            case 0:
                //R
                for (int i = 0; i < tileObjList.Count; i++)
                {
                    if (tileStateList[i].isLeft)
                    {
                        TileMove_R(tileObjList[i]);
                        tileStateList[i].isLeft = false;
                        tileStateList[i].isRight = true;
                    }
                    else if (!tileStateList[i].isLeft && !tileStateList[i].isRight)
                    {
                        tileStateList[i].isLeft = true;
                    }

                    else
                    {
                        tileStateList[i].isRight = false;
                    }
                }
                break;
            case 1:
                for (int i = 0; i < tileObjList.Count; i++)
                {
                    if (tileStateList[i].isRight)
                    {
                        TileMove_L(tileObjList[i]);
                        tileStateList[i].isRight = false;
                        tileStateList[i].isLeft = true;
                    }
                    else if (!tileStateList[i].isLeft && !tileStateList[i].isRight)
                    {
                        tileStateList[i].isRight = true;
                    }

                    else
                    {
                        tileStateList[i].isLeft = false;
                    }
                }
                break;
            case 2:
                for (int i = 0; i < tileObjList.Count; i++)
                {
                    if (tileStateList[i].isDown)
                    {
                        TileMove_U(tileObjList[i]);
                        tileStateList[i].isDown = false;
                        tileStateList[i].isUp = true;
                    }
                    else if (!tileStateList[i].isDown && !tileStateList[i].isUp)
                    {
                        tileStateList[i].isDown = true;
                    }

                    else
                    {
                        tileStateList[i].isUp = false;
                    }
                }
                break;

            case 3:
                for (int i = 0; i < tileObjList.Count; i++)
                {
                    if (tileStateList[i].isUp)
                    {
                        TileMove_D(tileObjList[i]);
                        tileStateList[i].isUp = false;
                        tileStateList[i].isDown = true;
                    }
                    else if (!tileStateList[i].isDown && !tileStateList[i].isUp)
                    {
                        tileStateList[i].isUp = true;
                    }

                    else
                    {
                        tileStateList[i].isDown = false;
                    }
                }
                break;
        }
       
    }

    void TileMove_R(GameObject tile)
    {
        tile.transform.position += new Vector3(tileSize_x * tileNum_x, 0, 0);
    }

    void TileMove_L(GameObject tile)
    {
        tile.transform.position -= new Vector3(tileSize_x * tileNum_x, 0, 0);
    }

    void TileMove_U(GameObject tile)
    {
        tile.transform.position += new Vector3(0, 0, tileSize_z * tileNum_z);
    }

    void TileMove_D(GameObject tile)
    {
        tile.transform.position -= new Vector3(0, 0, tileSize_z * tileNum_z);
    }

    
}
