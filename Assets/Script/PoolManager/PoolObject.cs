using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//被对象池管理的对象
public class PoolObject : MonoBehaviour 
{
    //public void SetPool( List<GameObject> poolList)
    //{
    //    m_pools = poolList;
    //}

    public void SetPool(Stack<GameObject > poolList)
    {
        m_pools = poolList;
    }
    //PoolMangager中装有本对象类型的缓存池
   // List<GameObject> m_pools;
    Stack<GameObject> m_pools;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    //销毁:重新回到对象池中
    public void Recovery()
    {
        //m_pools.Add(this.gameObject);
        m_pools.Push(this.gameObject);
        gameObject.SetActive(false);
    }
}
