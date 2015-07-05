using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TowerFactory : MonoBehaviour 
{
    //当前所有的tower容器
    public Dictionary<string, GameObject> m_TowerList = new Dictionary<string, GameObject>();


    uint m_towerId=0;
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


    //更据towername+"level" 返回 TowerData数据
    public TowerData FindTowerData(string strKey)
    {
        //从m_TowerDatas中找出对应的TowerData并复制和初始化位置
        TowerData newData = DataBase.GetInstance().m_TowerDatas[strKey];
        return newData;
    }


    //更据towername+"level"和坐标产生不同的对象在指定的地方
    //public Tower ProduceTower(string strKey,Vector3 initPos)
    public Tower ProduceTower(TowerType type,int level, Vector3 initPos)
    {
        string strKey = type.ToString() + level.ToString();
        //从m_TowerDatas中找出对应的TowerData并复制和初始化位置
        TowerData newData = DataBase.GetInstance().m_TowerDatas[strKey];
        GameObject newObj = (GameObject)GameObject.Instantiate(newData.m_towerPrefab,initPos,Quaternion.identity);
        Tower newTower = newObj.GetComponent<Tower>();
        newTower.m_towerType = type;
        newTower.m_towerName = newData.m_TowerName;
        newTower.m_AttackArea = newData.m_AttackArea;
        newTower.m_AttackPower = newData.m_AttackPower;
        newTower.m_AttackTime = newData.m_AttackTime;
        newObj.SetActive(true);

        newTower.name = newTower.name + m_towerId.ToString();
        m_towerId++;
        m_TowerList.Add(newObj.name, newObj);
        return newTower;
    }

    public void DestoryTower(GameObject towerObj)
    {
        m_TowerList.Remove(towerObj.name);
        Destroy(towerObj);
    }

    public void DestoryAllTower()
    {
        foreach (KeyValuePair<string, GameObject> tower in m_TowerList)
        {
            Destroy(tower.Value);
        }
        m_TowerList.Clear();
    }
}
