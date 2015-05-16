using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour 
{

    public string m_DieAnimationName = "dragon_die";
    public string m_WalkAnimationName="dragon_idle";


    //存放血条预设的文件位置
    public string m_lifePrebFilePath = "Assets/Prefab/LifeBar.prefab";
    public PathNode m_TargetNode=null;
    private Transform m_Transform;
    public  Transform m_enemyPrefab;

    [HideInInspector]
    //生命
 
    public bool m_ifDead = false;

    public int m_maxLife = 50;
    private int m_curLife = 50;
    private UISlider m_lifeSlider;

    private float m_uiLife;

    //血条要更随的life_point
    public GameObject m_LifeBarPoint;
    //血条的预设
    private GameObject m_LifeBarPreb;
    private Camera m_uiCamera;
    private Camera m_mainCamera;
    public int m_enemyDamage=1;
    public float m_enemySpeed=1;
    public int m_enemyDefense=0;
    public int m_enemyCoin = 0;


    public EnemyFactory m_enemyFactory;
	// Use this for initialization

    void Awake()
    {
        //从Prefab文件中载入血条的预设
        m_LifeBarPreb = Resources.LoadAssetAtPath<GameObject>(m_lifePrebFilePath);
        m_uiCamera = GameObject.Find("Camera").GetComponent<Camera>();
        m_mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //在uicamera中生成
        GameObject newLifeBar = (GameObject)GameObject.Instantiate(m_LifeBarPreb);
        newLifeBar.transform.parent = m_uiCamera.transform;
        newLifeBar.transform.localScale = new Vector3(1, 1, 1);


        MyUIFollowTarget uiTarget = newLifeBar.GetComponent<MyUIFollowTarget>();
        uiTarget.m_targetTransform = m_LifeBarPoint.transform;
        
        
        //获取血量
        m_lifeSlider = newLifeBar.GetComponent<UISlider>();
    }



	void Start () 
    {
        m_TargetNode = GameObject.Find("PathNode1").GetComponent<PathNode>();
        m_Transform = gameObject.transform;
        //m_curLife = m_maxLife;


        //设置最大血量
        MaxLife = m_maxLife;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(m_ifDead)
        {
            return;
        }
        MoveTo();
	}


    public void RotateTo()
    {
        m_Transform.LookAt(m_TargetNode.transform);
    }


    void PlayAnimation(string strAnimation,float deltaTime)
    {
        animation.CrossFade(strAnimation, deltaTime);
    }

    public void MoveTo()
    {
        //向PathNode移动 
        Vector3 pos1 = m_Transform.position;
        Vector3 posTarget = m_TargetNode.transform.position;

        float dist = Vector2.Distance(new Vector2(pos1.x, pos1.z), new Vector2(posTarget.x, posTarget.z));
        if(dist < 1.0f)
        {
            //到了基地
            if (m_TargetNode.m_Next == null)
            {
                GameManager.GetInstance().ReduceLife(m_enemyDamage);
                DestoryEnemy();
            }
            //到达了PathNode
            else
            {
                m_TargetNode = m_TargetNode.m_Next;
                RotateTo();
            }
        }
        //播放动画
        PlayAnimation(m_WalkAnimationName, 0.2f);
        m_Transform.Translate(m_Transform.forward * Time.deltaTime * m_enemySpeed, Space.World);
    }
       
    //销毁了
    void DestoryEnemy()
    {
        m_ifDead = true;
        animation.Play(m_DieAnimationName);
        StartCoroutine("dieComplete");
        //销毁ui
        Destroy(m_lifeSlider.gameObject);

        //从GameManager的容器中移出
        GameManager.GetInstance().m_EnemyList.Remove(this);
    }

    //设置当前的生命值
    public int Life
    {
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            m_curLife = value;
            //设置UI
            m_lifeSlider.value = (float)m_curLife / m_maxLife;
        }
        get
        {
            return m_curLife;
        }
    }


    //设置最大血量
    public int MaxLife
    {
        set
        {
            m_curLife = m_maxLife;
            //设置ui
           UISprite lifeFrontSprite= m_lifeSlider.gameObject.GetComponentInChildren<UISprite>();
            //设置血量与血条长度的对应关系
           lifeFrontSprite.width = m_maxLife/10;
        }
    }

    //收到伤害
    public void GetDamage(int attackPower)
    {
        if(m_ifDead)
        {
            return;
        }
        //实际的伤害 = 攻击力 - 防守力
        int damage = attackPower - m_enemyDefense;
        if(damage <=0)
        {
            damage = 1;
        }
        Life = m_curLife - damage;
        if(Life <= 0)
        {
            DestoryEnemy();
            //添加金币
            GameManager.GetInstance().AddCoin(m_enemyCoin);
            
        }
    }



    IEnumerator dieComplete()
    {
        yield return new WaitForSeconds(3);
        //工厂中现在的敌人数
        m_enemyFactory.m_totalEnemies -= 1;
        //如果当前已经是最后一个敌人
        if(m_enemyFactory.m_totalEnemies == 0)
        {
            GameManager.GetInstance().GameComplete();
        }
        Destroy(this.gameObject);
    }
}
