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
        SingleTon();  //���ӸŴ����� �̱��� ������ �ּ�ó��  don't destroy�� �ȳ���
        CreateDictionaryData();  // ��ųʸ��� ������ ����
        Initalize();  // ������ �����Ͽ� Ǯ�� ����
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
        //Ǯ�� ����Ʈ�� �ִ� ������ ��ųʸ� ������ �����ϱ�   =>  �� �����ʹ� ���� ������ Ŭ���� ���� ���ʿ��� �Ѳ����� ĳ�����ֱ�
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



    //�ʱ� ���� �� ť ����ִ� ���¿��� ȣ�� ���� ��
    void CreateNewObject(int dictionaryKey, GameObject parent = null)  //Ű�� ������ �� �θ�
    {

        GameObject tempObj = Instantiate(poolDic[dictionaryKey].prefab);
        poolDic[dictionaryKey].objQueue.Enqueue(tempObj);
        tempObj.SetActive(false);

        tempObj.transform.position = new Vector3(0, 0, 0);
        if(parent != null) tempObj.transform.SetParent(parent.transform);

    }

    // ť���� ȣ��
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

    // ��ȯ
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

    // �׽�Ʈ�� �ӽ� �ڵ�
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

    // ��ųʸ� ������ ���� �� ��ųʸ� ������ŭ ����Ʈť ����
    void CreateDictionaryData()
    {
        //Ǯ����ü ������ ��ųʸ� ����
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
    public int initalObjectCount = 0; //�ʱ� ���� ����
    public Queue<GameObject> objQueue = new Queue<GameObject>();
    
}

