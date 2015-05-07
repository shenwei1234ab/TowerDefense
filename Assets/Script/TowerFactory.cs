using UnityEngine;
using System.Collections;

public class TowerFactory : MonoBehaviour 
{
    public static TowerFactory GetInstance()
    {
        return m_Instance;
    }

    private static TowerFactory m_Instance;
	// Use this for initialization
	void Start () 
    {
        m_Instance = this;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}


    //更据towername+"level"和坐标产生不同的对象在指定的地方
    public Tower ProduceTower(string strKey,Vector3 initPos)
    {
        //从m_TowerDatas中找出对应的Tower并复制和初始化位置
        TowerData newData = DataBase.GetInstance().m_TowerDatas[strKey];
        GameObject copyObj = (GameObject)GameObject.Instantiate(newData.m_towerPrefab,initPos,Quaternion.identity);
        //copyObj.AddComponent<Tower>();
        Tower newTower = copyObj.GetComponent<Tower>();
        newTower.m_AttackArea = newData.m_AttackArea;
        newTower.m_AttackPower = newData.m_AttackPower;
        newTower.m_AttackTime = newData.m_AttackTime;
        copyObj.SetActive(true);
        return newTower;
    }




}
