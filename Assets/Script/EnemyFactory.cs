using UnityEngine;
using System.Collections;

using Mono.Xml;
using System.IO;

using System.Security;

public class EnemyFactory : MonoBehaviour
{

    enum WaveState
    {
        NormalWave,
        LastWave,
        NoWave,
    }
     WaveState m_waveState;
    //序列号
    int m_Index = 0;

    //计时器
    float m_Timer = 0;

    //是否在等待
    bool m_ifWaiting = true;

    //是否要开始生产怪物
    bool m_ifProductMonster = false;
    public bool ifProductMonster
    {
        set
        {
            m_ifProductMonster = value;
        }
    }
    //敌人的数据
    public class EnemyData
    {
        public EnemyData(int wave,string name,int level,float waitTime)
        {
            m_Wave = wave;
            m_Name = name;
            m_Level = level;
            m_Wait = waitTime;
        }
        //数据
        public int m_Wave = 1;
        public string m_Name = "";
        public int m_Level =1;
        public float m_Wait=1.0f;
    }


    //起始位置
    public GameObject m_StartNode;
    //保存敌人的数据
    ArrayList m_EnemyDataList;
	
    //要生产的敌人总数
    public int m_totalEnemies;
    void Awake()
    {
        
    }

	void Start () 
    {
        //使用mono.xml解析xml文件
        ReadXML();
        //获得第一个敌人
        EnemyData data = (EnemyData)m_EnemyDataList[m_Index];
        m_Timer = data.m_Wait;
        m_waveState = WaveState.NormalWave;
	}


    
    //读入XML数据并存入到m_EnemyDataList容器中
    void ReadXML()
    {
        m_EnemyDataList = new ArrayList();
        SecurityParser SP = new SecurityParser();
        string xmlPath = "Scene1";
        SP.LoadXml(Resources.Load(xmlPath).ToString());

        SecurityElement SE = SP.ToXml();
        foreach (SecurityElement child in SE.Children)
        {
            //比对下是否使自己所需要得节点
            if (child.Tag == "table")
            {
                //获得节点得属性
                string wave = child.Attribute("wave");
                GameManager.GetInstance().nTotalWaves = int.Parse(wave);
                string enemyname = child.Attribute("enemyname");
                string level = child.Attribute("level");
                string wait = child.Attribute("wait");
                EnemyData data = new EnemyData(int.Parse(wave),enemyname,int.Parse(level),float.Parse(wait));
                m_EnemyDataList.Add(data);
            }
        }
        m_totalEnemies = m_EnemyDataList.Count;
    }

    
	
	// Update is called once per frame
	void Update () 
    {
     
        if (!m_ifProductMonster)
        {
            return;
        }
        //不在等待就产生
        if (!m_ifWaiting)
        {
            //根据当前读取到的内容生成怪物，并读取吓一条消息
            ProduceEnemy();
        }
        else
        {
            //在等待
            if(m_Timer >0)
            {
                m_Timer -= Time.deltaTime;
            }
            //等待完成了，准备生成怪物
            else
            {
                //如果上一波还没有完全销毁。。。。。。return


                // 获取当前波数
                int currWave = ((EnemyData)m_EnemyDataList[m_Index]).m_Wave;
                GameManager.GetInstance().CurrentWave = currWave;
                //最后一波开始了
                if(currWave == GameManager.GetInstance().nTotalWaves && m_waveState==WaveState.NormalWave)
                {
                    ///////////////////////////播放boss来了的动画
                    UIManager.Instance().ShowLastWave();
                    //停止出兵直到动画播放完成
                    m_ifProductMonster = false;
                    m_waveState = WaveState.LastWave;
                }
                m_ifWaiting = false;
            }
        }
	}
    //产生敌人
    void ProduceEnemy()
    {
        //获取敌人
        EnemyData data = (EnemyData)m_EnemyDataList[m_Index];
        //查找敌人的prefab
        Monster  enemyData = FindEnemy(data.m_Name,data.m_Level);
        //生成敌人
            GameObject newEnemy = (GameObject)Instantiate(enemyData.enemyPrefab, m_StartNode.transform.position, m_StartNode.transform.rotation);  
            //添加脚本
            //newEnemy.AddComponent<Enemy>();
            newEnemy.SetActive(true);
            //设置脚本的参数
            Enemy enemyComponet = newEnemy.GetComponent<Enemy>();
            
            enemyComponet.m_enemySpeed = enemyData.enemySpeed;
            enemyComponet.m_maxLife = enemyData.enemyLife;
            enemyComponet.m_enemyDefense = enemyData.enemyDefense;
            enemyComponet.m_enemyDamage = enemyData.enemyDamage;
            enemyComponet.m_enemyCoin = enemyData.enemyCoin;
            enemyComponet.m_enemyFactory = this;
            GameManager.GetInstance().m_EnemyList.Add(enemyComponet);
        //准备读取下一条记录
        //如果当前是最后一条记录
        if (m_Index >= m_EnemyDataList.Count-1)
        {
            //最后一个敌人
            m_waveState = WaveState.NoWave;

            //不再产生怪物
            m_ifProductMonster = false;
            Debug.Log("Last Enemy");
            return;
        }
        ++m_Index;
        m_ifWaiting = true;
        m_Timer = ((EnemyData)m_EnemyDataList[m_Index]).m_Wait;
    }


    //从database中找到key对应的Monster信息
    Monster FindEnemy(string name, int level)
    {
        string key = name + level.ToString();
       return DataBase.GetInstance().m_MonsterDatas[key];
    }
}
