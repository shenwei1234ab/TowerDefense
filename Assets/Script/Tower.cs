using UnityEngine;
using System.Collections;



public enum TowerState
{
    None,
    Building,
}

//对应Tower.xml文件 
public enum TowerType
{
    Acid,
    Cannon,
    Cross,
    Crystal,
    Fire,
    Gatlin,
    Gauss,
    Laser,
    Mortar,
    Plasma,
    Rocket,
    Tesla
}


public class Tower : MonoBehaviour 
{
    //存放进度条预设的文件位置
    public string m_processPrebFilePath = "Assets/Prefab/ProcessBar.prefab";

    //建造的时间
    public float m_buildTime = 3.0f;
    //要发射的位置
    public Transform m_ShootPos;
    //要旋转的头部
    public Transform m_TopTower;
    //头部旋转的速度
    public float m_RotateSpeed = 5.0f;
    protected Enemy m_AttackEnemy = null;
    protected Transform m_transform;
    protected Transform m_AttackEnemyTran=null;

    [HideInInspector]
    public string m_towerName;
    //最大等级
    public int m_maxLevel = 3;
    //当前等级
    public int m_curLevel = 1;
    //攻击范围
    public float m_AttackArea=10;

    //攻击力
    public int m_AttackPower = 1;

    //攻击间隔
    public float m_AttackTime = 1.0f;
    //计时器
    protected float m_Timer = 0.0f;

    //发射的子弹
    public GameObject m_Rockets;


    //进度条
    public GameObject m_processBar;
    public Transform m_processBarPos;
    
   
    //当前的状态 
    public TowerState m_towerState;
	// Use this for initialization
	protected void Start () 
    {
        m_transform = gameObject.transform;
        m_Timer = m_AttackTime;
        m_towerState = TowerState.Building;
        //创建ProcessBar并注册事件
        GameObject newBar= Resources.LoadAssetAtPath<GameObject>(m_processPrebFilePath);
        GameObject processBar = (GameObject)GameObject.Instantiate(newBar);
        //添加到ui 中
        processBar.transform.parent = UICamera.currentCamera.transform;
        processBar.transform.localScale = new Vector3(1, 1, 1);
        if (!m_processBarPos)
        {
            m_processBarPos = this.transform;
        }
        processBar.transform.position = UIManager.WorldPosToUI(m_processBarPos.position);
        //注册事件
        processBar.GetComponent<ProcessBar>().m_timeOverEvent += BuildComplete;
	}


    void BuildComplete(GameObject sender)
    {
        Destroy(sender);
        m_towerState = TowerState.None;
    }

	// Update is called once per frame
	protected void Update () 
    {
        if (m_towerState == TowerState.Building )
        {
            return;
        }
        //建造或者升级完成
        ScanForEnemy();
        RotateTo();
        Attack();
	}

    

    //在自己的攻击范围内找可以攻击的敌人
   public virtual void ScanForEnemy()
    {
        if (m_AttackEnemyTran)
        {
            //判断攻击的敌人是否出了自己的攻击范围
            float dis = Vector3.Distance(m_AttackEnemyTran.position, m_transform.position);
            if (dis > m_AttackArea)
            {
                m_AttackEnemyTran = null;
                m_AttackEnemy = null;
                //重新去找
            }
        }
        //失去了攻击敌人就重新去找
        else
        {
            Collider[] cols = Physics.OverlapSphere(m_transform.position, m_AttackArea);
            if (cols.Length == 0)
            {
                return;
            }
            foreach (Collider col in cols)
            {
                int tmpLife = 0;
                //找生命值最小的
                if (col.gameObject.tag == "Monster")
                {
                    Enemy enemy = col.gameObject.GetComponent<Enemy>();
                    if (enemy.m_ifDead == true)
                    {
                        continue;
                    }
                    else
                    {
                        //找生命值最小的
                        if (tmpLife == 0 || enemy.Life < tmpLife)
                        {
                            m_AttackEnemyTran = enemy.gameObject.transform;
                            tmpLife = enemy.Life;
                            m_AttackEnemy = enemy;
                        }
                    }
                }
            }
        }
    }


    void RotateTo()
    {
        //如果能攻击敌人就转向他
        if (m_AttackEnemyTran == null)
        {
            return;
        }
        Vector3 current = m_TopTower.eulerAngles;
        //转向下一帧物体的位置
        m_TopTower.LookAt(m_AttackEnemyTran.transform.position+m_AttackEnemyTran.transform.forward);
        Vector3 target = m_TopTower.eulerAngles;
        float next = Mathf.MoveTowardsAngle(current.y, target.y, 120 * Time.deltaTime);
        m_TopTower.eulerAngles = new Vector3(current.x, next, current.z);
    }

      void Attack()
     {
         if (m_AttackEnemyTran == null || m_AttackEnemyTran.gameObject.GetComponent<Enemy>().m_ifDead)
         {
             return;
         }
         m_Timer -= Time.deltaTime;
         //还没有到攻击的时间
         if(m_Timer >0)
         {
             return;
         }
         //到了时间间隔
        //发射子弹
           Shoot();
     }

   
    public virtual void Shoot()
      {
          //Debug.Log("射击");
          GameObject rockObj = (GameObject)Instantiate(m_Rockets, m_ShootPos.position, m_TopTower.rotation);
          //使子弹拥有和塔一样的攻击力
          Bullet bullet = rockObj.GetComponent<Bullet>();
          bullet.m_AttackPower = m_AttackPower;
          bullet.Shoot(m_AttackEnemyTran);
          m_Timer = m_AttackTime;
      }

}
