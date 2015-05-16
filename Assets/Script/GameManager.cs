using UnityEngine;
using System.Collections;


//游戏逻辑
public class GameManager : MonoBehaviour 
{
    
    //particle system
    public GameObject m_BuildingEffect;
    public GameObject m_BuildingDoneEffect;
    //UI管理
    //当前的金币
    public int m_curCoins = 10;




    //与地面的碰撞层
    public LayerMask m_groundLayer;
    //基地的生命
    int m_Life = 10;
    //当前所有的敌人容器
    public ArrayList m_EnemyList = new ArrayList();
	
   //当前所有的tower容器
    public ArrayList m_TowerList = new ArrayList();


    public GameObject m_processBarPreb;


    EnemyFactory m_enemyFactory;
    //当前选中的塔(升级或者销毁)
   // private Tower m_SelectTower= null;

    //当前要选中的图标
    //private MyButton m_SelectButton = null;

    void Awake()
    {  ////注册所有ui事件
        m_Instance = this;
    }

	void Start () 
    {
        //开始产生怪物
         GameObject enemyFactory = GameObject.Find("EnemyFactory");
        m_enemyFactory =  enemyFactory.GetComponent<EnemyFactory>();
        ProductEnemy(true);
	}


    public void ProductEnemy(bool control)
    {
        m_enemyFactory.ifProductMonster = control;
    }
	
    //当前的进攻波数
   private  int m_Wave=1;
   public int CurrentWave
    {
        set
        {
            m_Wave = value;
            //在ui上显示
            UIManager.Instance().CurWave = m_Wave;

        }
        get
        {
            return m_Wave;
        }
    }

   //总共的波数
   private int m_TotalWaves = 1;
    public int nTotalWaves
   {
       set
       {
           m_TotalWaves = value;
           UIManager.Instance().TotalWaves = m_TotalWaves;
       }
        get
       {
           return m_TotalWaves;
       }
   }


   

	// Update is called once per frame
	void Update () 
    {
        
	}

    //在场景中点击到了塔的模型
    public void HitTower(Tower hitTower,Vector3 hitPoint)
    {
        //正在建
        if(hitTower.m_towerState == TowerState.Building)
        {
            return;
        }
        //否则面板显示 
        else
        {
            UIManager.Instance().ShowPanel(hitPoint);  
        }
    }
    
    public void HitPlane()
    {
        
    }


    public bool CreateTower(Vector3 hitPos,TowerType towerType)
    {
        string strKey = towerType.ToString() + "1";
          //查找要创建的塔的价钱
          TowerData newTowerDate = TowerFactory.GetInstance().FindTowerData(strKey);
          int needCost = newTowerDate.m_Cost;
          //金钱不够
          if (m_curCoins < needCost)
          {
              //产生提示
              UIManager.Instance().CreateWarning(hitPos, "金钱不够");
              return false;
          }
          //在指定的位置创建tower;
          Tower newTower = TowerFactory.GetInstance().ProduceTower(towerType, 1, hitPos);
          m_TowerList.Add(newTower);
          //减少钱
          Coins -= needCost;
          return true;
    }

   
    //在指定位置创建粒子动画
    GameObject CreateParticleSystem(GameObject particlePreb,Vector3 crePos)
    {
        GameObject particleSystem = (GameObject)GameObject.Instantiate(particlePreb, crePos,Quaternion.identity);
        return particleSystem;
    }


    private static GameManager m_Instance;
    public static GameManager GetInstance()
    {
        return m_Instance;
    }


    public void AddCoin(int coins)
    {
        m_curCoins += coins;
        //ui
        //m_uiManger.Coins = m_curCoins;
        UIManager.Instance().Coins = m_curCoins;
    }



    public int Coins
    {
        get
        {
            return m_curCoins;
        }
        set
        {
            if(value < 0)
            {
                value = 0;
            }
            m_curCoins = value;
            //m_uiManger.Coins = m_curCoins;
            UIManager.Instance().Coins = m_curCoins;
        }
    }


    //基地受到伤害
    public void ReduceLife(int reduceLife)
    {
        m_Life -= reduceLife;
        if(m_Life <=0)
        {
            m_Life = 0;
        }
       // m_uiManger.Life = m_Life;
        UIManager.Instance().Life = m_Life;
        if(m_Life == 0)
        {
            //GameOver;
        }
    }



    public bool UpdateTower(Tower selectTower)
    {
        Vector3 towerPos = selectTower.gameObject.transform.position;
        //当前是最大等级
        int curLevel = selectTower.m_curLevel;
        if (curLevel >= selectTower.m_maxLevel)
        {
            UIManager.Instance().CreateWarning(towerPos, "当前是最大等级");
            //ui显示
            return false;
        }
        int nextLevel = selectTower.m_curLevel + 1;
        string key = selectTower.m_towerType + nextLevel.ToString();
        //查找要创建的塔的价钱
        TowerData newTowerDate = TowerFactory.GetInstance().FindTowerData(key);
        int needCost = newTowerDate.m_Cost;
        //金钱不够
        if (m_curCoins < needCost)
        {
            //产生提示
            UIManager.Instance().CreateWarning(towerPos, "金钱不够");
            UIManager.Instance().HidPanel();
            return false;
        }
        //在指定的位置创建tower;
        Tower newTower = TowerFactory.GetInstance().ProduceTower(selectTower.m_towerType, nextLevel, towerPos);
        m_TowerList.Add(newTower);
        //减少钱
        Coins -= needCost;
        //销毁原来的塔
        Destroy(selectTower.gameObject);
        //隐藏uipanel
        UIManager.Instance().HidPanel();
        return true;
    }


    public void DestoryTower(Tower selectTower)
    {
        //销毁塔
        Destroy(selectTower.gameObject);
        UIManager.Instance().HidPanel();
    }



    //建造粒子动画完成后执行的动作 
    //void BuildingEffectCompleteEvent(GameObject obj)
    //{
    //    //销毁自己并创建完成完成动画和注册事件
    //    Destroy(obj);
    //    GameObject particleSystem = (GameObject)GameObject.Instantiate(m_BuildingDoneEffect, m_bulidPos, Quaternion.identity);
    //    particleSystem.GetComponent<ParticleSystemControl>().m_particleCompleteEvent += BuildingDoneEffectCompleteEvent;
    //}
    //void BuildingDoneEffectCompleteEvent(GameObject obj)
    //{
    //    Destroy(obj);
    //    //在指定的位置创建tower;
    //    string strKey = m_SelectTowerName + "1";
    //    Tower newTower = TowerFactory.GetInstance().ProduceTower(strKey, m_bulidPos);
    //    m_TowerList.Add(newTower);
    //    m_ifSelected = false;
    //}



    //基地生命为0
    public void GameOver()
    {

    }


    //所有的敌人都消灭了
    public void GameEnd()
    {

    }
   
}
