using UnityEngine;
using System.Collections;

public class PathNode : MonoBehaviour 
{
    //private PathNode m_Prev = null;
    public PathNode m_Next = null;

    public int m_Life = 15;
    public int m_MaxLife = 15;
    void Awake()
    {
        string strMainName = "PathNode";
        //获得本节点的名字
        string strMyName = gameObject.name;
        string strId = strMyName.Substring(strMainName.Length);
        int nNextId = System.Convert.ToInt32(strId) + 1;
        //获得下个节点的名字
        string strNextName = strMainName + nNextId.ToString();
        //获得下一个节点
       
        GameObject nextGameObj = GameObject.Find(strNextName);
        if(nextGameObj)
        {
            m_Next = nextGameObj.GetComponent<PathNode>();
        }
    }
	// Use this for initialization
	void Start () 
    {
      
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}


}
