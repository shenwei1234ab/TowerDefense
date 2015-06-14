﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//对象池
public class PoolManager : MonoBehaviour 
{
    //要缓存的不同类型的基本信息
    [System.Serializable]
    public class PoolObj
    {
        public GameObject m_objPreb;
        //初始的数量
        public int m_initCount;
        //缓存池中数量不够时重行生成的数量
        public int m_rebuildCount;
        [HideInInspector]
        //存放相应类型的缓存池
        //public List<GameObject> m_pool;
        //使用stack管理可用的游戏对象
        public Stack<GameObject> m_pool;
    }

    public PoolObj[] m_poolObjArray;



    static PoolManager m_instance;
    public static PoolManager GetInstance()
    {
        return m_instance;
    }

    //装有可用的GameOject
    private static Dictionary<string, PoolObj> m_poolObjDictionary;

    void Awake()
    {
        m_instance = this;
    }
	// Use this for initialization
	void Start () 
    {
        m_poolObjDictionary = new Dictionary<string, PoolObj>();
	    //遍历preb依次存入map集合中以便于之后取出 
        foreach (PoolObj obj in m_poolObjArray)
        {
            //obj.m_pool = new List<GameObject>();
            obj.m_pool = new Stack<GameObject>();
            CreateNewPoolObject(obj.m_objPreb, obj.m_initCount, obj.m_pool);
            m_poolObjDictionary.Add(obj.m_objPreb.name, obj);
        }
       
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    /// <summary>
    /// 从缓冲池中取出指定类型的对象，如果prefab不存在就返回null
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <param name="quat"></param>
    /// <returns></returns>
    public GameObject GetObject(string type,Vector3 pos,Quaternion quat)
    {
        GameObject retObj=null;
        //在m_pool中查找,如果存在对应的Prefab
        if(m_poolObjDictionary.ContainsKey(type))
        {
            //查找是否有空余可用的GameObj
            //List<GameObject> objList = m_poolObjDictionary[type].m_pool;
            Stack<GameObject> objList = m_poolObjDictionary[type].m_pool;
            //如果有可用的GameObject
            if(objList.Count > 0)
            {
                //取出第一个 
                //retObj = objList[0];
                //objList[0].SetActive(true); 
                ////重新执行start函数
                //objList[0].BroadcastMessage("Start");
                //objList.RemoveAt(0); 
                retObj = objList.Pop();
                retObj.SetActive(true);
                retObj.BroadcastMessage("Start");
            }
            //没有可用的就重新在生成
            else
            {
                int rebulidCount = m_poolObjDictionary[type].m_rebuildCount;
                GameObject preb = m_poolObjDictionary[type].m_objPreb;
                //List<GameObject> pool = m_poolObjDictionary[type].m_pool;
                Stack<GameObject> pool = m_poolObjDictionary[type].m_pool;
                CreateNewPoolObject(preb, rebulidCount, pool);
                retObj = Instantiate(preb) as GameObject;
                retObj.AddComponent<PoolObject>();
                retObj.SendMessage("SetPool", pool);
                retObj.SetActive(true);
            }
            retObj.transform.position = pos;
            retObj.transform.rotation = quat;
        }
        return retObj;

    }

    //void CreateNewPoolObject(GameObject preb,int num,List<GameObject> pool)
    void CreateNewPoolObject(GameObject preb,int num,Stack<GameObject> pool)
    {
        for(int i=0;i<num;++i)
        {
            GameObject newObj = Instantiate(preb) as GameObject;
            newObj.AddComponent<PoolObject>();
            newObj.SendMessage("SetPool", pool);
            newObj.SetActive(false);
            //pool.Add(newObj);
            pool.Push(newObj);
        }
    }




    //对象销毁重新回收对象
    
    //存入新的到对象池
    public void SaveObject(string type,int num)
    {
        
        
    }
}
