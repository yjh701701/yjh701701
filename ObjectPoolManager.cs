using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class ObjectPoolManager : MonoBehaviour
{

    //GM
    public static ObjectPoolManager objPoolManager;


    public Dictionary<int, PoolObjectData> poolDic;
    public List<GameObject> poolingPrefabList = new List<GameObject>();

    void Awake()
    {
        SingleTon();  //게임매니저로 싱글톤 구현시 주석처리  don't destroy는 안넣음
        CreateDictionaryData();  // 딕셔너리에 데이터 저장
        Initalize();  // 데이터 참조하여 풀링 생성
    }

    void Update()
    {
        TestFunc();
    }

    void SingleTon()
    {
        if (objPoolManager == null)
        {
            objPoolManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void Initalize()
    {
        //풀링 리스트에 있는 프리펩 딕셔너리 정보에 저장하기   =>  이 데이터는 따로 데이터 클래스 만들어서 그쪽에서 한꺼번에 캐싱헤주기
        foreach (int key in poolDic.Keys)
        {
            if (poolingPrefabList[key] != null)
            {
                int createNum = poolDic[key].initalObjectCount;
                for (int i = 0; i < createNum; i++)
                {
                    CreateNewObject(key);
                }
            }
        }
    }



    //초기 생성 및 큐 비어있는 상태에서 호출 받을 시
    void CreateNewObject(int dictionaryKey, GameObject parent = null)  //키와 생성후 들어갈 부모
    {

        GameObject tempObj = Instantiate(poolDic[dictionaryKey].prefab);
        poolDic[dictionaryKey].objQueue.Enqueue(tempObj);
        tempObj.SetActive(false);

        tempObj.transform.position = new Vector3(0, 0, 0);
        if(parent != null) tempObj.transform.SetParent(parent.transform);

    }

    // 큐에서 호출
    public GameObject UseObject(int dictionaryKey, Transform useParent = null)
    {
        if (poolDic[dictionaryKey].objQueue.Count <= 0)
        {
            CreateNewObject(dictionaryKey, this.gameObject);
        }
        GameObject temp = poolDic[dictionaryKey].objQueue.Dequeue();
        temp.transform.SetParent(useParent);
        temp.transform.position = Vector3.zero;
        temp.SetActive(true);

        return temp;
    }

    // 반환
    public void ReturnObject(int dictionaryKey, GameObject returnObj)
    {
        if (poolDic.ContainsKey(dictionaryKey))
        {
            poolDic[dictionaryKey].objQueue.Enqueue(returnObj);
            returnObj.transform.position = Vector3.zero;
            returnObj.SetActive(false);
        }
        else
        {
            Debug.LogWarning("ReturnObject : not found dictionaryKey");
            Destroy(returnObj);
        }
    }

    // 테스트용 임시 코드
    void TestFunc()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseObject(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            UseObject(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseObject(2);
        }
    }

    // 딕셔너리 데이터 생성 및 딕셔너리 개수만큼 리스트큐 생성
    void CreateDictionaryData()
    {
        //풀링객체 데이터 딕셔너리 생성
        poolDic = new Dictionary<int, PoolObjectData>();

        poolDic.Add(0, new PoolObjectData { prefab = poolingPrefabList[0], initalObjectCount = 10 });
        poolDic.Add(1, new PoolObjectData { prefab = poolingPrefabList[1], initalObjectCount = 50 });
        poolDic.Add(2, new PoolObjectData { prefab = poolingPrefabList[2], initalObjectCount = 30 });
    }
}

//
[Serializable]
public class PoolObjectData 
{
    //public int key; //key index
    public GameObject prefab;
    public int initalObjectCount = 0; //초기 생성 개수
    public Queue<GameObject> objQueue = new Queue<GameObject>();
    
}

