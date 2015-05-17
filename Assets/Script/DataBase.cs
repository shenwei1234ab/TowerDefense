using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Xml;
using System.IO;

using System.Security;

//记录怪物Monster.xml信息
public class Monster
{
    //怪物名字
    public string enemyName;
    //怪物等级
    public int enemyLevel;

    public GameObject enemyPrefab = null;
    //移动速度
    public float enemySpeed = 1.0f;


    public int enemyLife = 1;
    //防御
    public int enemyDefense = 1;

    //造成的伤害
    public int enemyDamage = 1;
    //击败后获得的金币
    public int enemyCoin = 1;

    public Monster(string name, int level, string enemyPre, float speed,int life, int defense,int damage, int coin)
    {
        enemyName = name;
        enemyLevel = level;
        enemySpeed = speed;
        enemyLife = life;
        enemyDefense = defense;
        enemyDamage = damage;
        enemyCoin = coin;
        enemyPrefab = Resources.LoadAssetAtPath<GameObject>(enemyPre);
    }
}

//记录Tower.xml信息
public enum AttackType
{
    All,
    Ground,
    Sky
}
public class TowerData
{
  
    public TowerData(string towerName, int towerLevel, float attackArea, int attackPower,float attackTime, AttackType type, int cost, string path)
    {
        m_TowerName=towerName;
        m_TowerLevel=towerLevel;
        m_AttackArea=attackArea;
        m_AttackPower = attackPower;
        m_AttackTime=attackTime;
        m_AttackType=type;
        m_Cost = cost;
        m_towerPrefab = Resources.LoadAssetAtPath<GameObject>(path);
    }
    //名字
    public string m_TowerName;
    public int m_TowerLevel;
    public GameObject m_towerPrefab = null;
    public float m_AttackArea;
    public int m_AttackPower;
    public float m_AttackTime;
    public AttackType m_AttackType;
    public int m_Cost;
}




//游戏开始从xml读取怪物和tower的信息
public class DataBase : ScriptableObject 
{
    private static DataBase m_Instance = null;
    public static DataBase GetInstance()
    {
        if(m_Instance ==null)
        {
            m_Instance = ScriptableObject.CreateInstance<DataBase>();
        }
        return m_Instance;
    }
    //读取Monster.xml的文件名字
    public string m_strMonsterFileName = "Monster";
    //读取Tower.xml的文件名字
    public string m_strTowerFileName = "Tower";
    
    //读取Monster.model的文件路径
    public string m_modelMonsterFileName = "Assets/Model/";
    //读取Tower.model的文件路径
    public string m_modelTowerFileName = "Assets/MOBA and Tower Defense/Prefabs_Turrets/";


    //以怪物名字和等级作为键创建map信息方便下次的读取
    public  Dictionary<string, Monster> m_MonsterDatas;
    public Dictionary<string, TowerData> m_TowerDatas;


    void Awake()
    {
        //读取Monster.xml文件
        m_MonsterDatas = new Dictionary<string, Monster>();
        SecurityParser SP = new SecurityParser();
        SP.LoadXml(Resources.Load(m_strMonsterFileName).ToString());
        SecurityElement SE = SP.ToXml();
        foreach (SecurityElement child in SE.Children)
        {
            //比对下是否使自己所需要得节点
            if (child.Tag == "table")
            {
                //获得读取的属性并构造Monster
                string enemyName = child.Attribute("Enemyname");
                int enemyLevel = int.Parse(child.Attribute("Level"));
                string prePath = m_modelMonsterFileName + child.Attribute("PrefabPath");
                float enemySpeed = float.Parse(child.Attribute("Speed"));
                int enemyLife = int.Parse(child.Attribute("Life"));
                int enemyDefense = int.Parse(child.Attribute("Defenese"));
                int enemyDamage = int.Parse(child.Attribute("Damage"));
                int getCoin = int.Parse(child.Attribute("GetCoin"));

                Monster monster = new Monster(enemyName, enemyLevel, prePath, enemySpeed, enemyLife, enemyDefense, enemyDamage, getCoin);
                //加入到字典容器中方便下次查询
                //键：enemyName+enemyLevel
                string key = enemyName + enemyLevel.ToString();
                m_MonsterDatas.Add(key, monster);
            }
        }


        //读取Tower.xml文件
        m_TowerDatas = new Dictionary<string, TowerData>();
        SecurityParser SP2 = new SecurityParser();
        SP2.LoadXml(Resources.Load(m_strTowerFileName).ToString());
        SecurityElement SE2 = SP2.ToXml();
        foreach (SecurityElement child in SE2.Children)
        {
            //比对下是否使自己所需要得节点
            if (child.Tag == "table")
            {
                //获得读取的属性并构造Monster
                string towerName = child.Attribute("TowerName");
                int towerLevel = int.Parse(child.Attribute("Level"));
                float attackArea = int.Parse(child.Attribute("AttackArea"));
                int attackPower = int.Parse(child.Attribute("AttackPower"));
                float attackTime = float.Parse(child.Attribute("AttackTime"));
                string attackType = child.Attribute("Attacktype");
                AttackType type;
                if(attackType.CompareTo("ALL")==0)
                {
                    type = AttackType.All;
                }
                else if (attackType.CompareTo("Ground") == 0)
                {
                    type = AttackType.Ground;
                }
                else
                {
                    type = AttackType.Sky;
                }
                int cost = int.Parse(child.Attribute("Cost"));
                string path = m_modelTowerFileName + child.Attribute("PrePath");

                //构造data对象
                TowerData toweData = new TowerData(towerName, towerLevel, attackArea, attackPower, attackTime, type, cost, path);
                //加入到字典容器中方便下次查询
                //键：enemyName+enemyLevel
                string key = towerName + towerLevel.ToString();
                m_TowerDatas.Add(key, toweData);
            }
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
