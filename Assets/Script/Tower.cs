using UnityEngine;
using System.Collections;


public class Tower : MonoBehaviour 
{
   
    //要旋转的头部
    public Transform m_TopTower;
    //头部旋转的速度
    public float m_RotateSpeed = 5.0f;

    //要发射的位置
    public Transform m_ShootPos;
    private  int m_shotTime=0;
    private Transform m_transform;


    
    private Transform m_AttackEnemyTran=null;

    [HideInInspector]
    //攻击范围
    public float m_AttackArea=10;

    //攻击力
    public int m_AttackPower = 1;

    //攻击间隔
    public float m_AttackTime = 1.0f;
    //计时器
    private float m_Timer = 0.0f;

    //发射的子弹
    public GameObject m_Rockets;

	// Use this for initialization
	void Start () 
    {
        m_transform = gameObject.transform;
        m_Timer = m_AttackTime;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    //找到攻击返回内的敌人
        ScanForEnemy();
        RotateTo();
        Attack();
	}

    //在自己的攻击范围内找可以攻击的敌人
    void ScanForEnemy()
    {
        //默认之前有攻击敌人
        if (m_AttackEnemyTran)
        {
            //判断攻击的敌人是否出了自己的攻击范围
            float dis = Vector3.Distance(m_AttackEnemyTran.position, m_transform.position);
            if (dis > m_AttackArea)
            {
                m_AttackEnemyTran = null;
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
                        if (tmpLife == 0 || enemy.Life > tmpLife)
                        {
                            m_AttackEnemyTran = enemy.gameObject.transform;
                            tmpLife = enemy.Life;
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
         if(m_Rockets)
         {
             //Debug.Log("射击");
            GameObject rockObj =  (GameObject)Instantiate(m_Rockets, m_ShootPos.position, m_TopTower.rotation);
             //使子弹拥有和塔一样的攻击力
          Bullet bullet =   rockObj.GetComponent<Bullet>();
              bullet.m_AttackPower = m_AttackPower;
              bullet.Shoot(m_AttackEnemyTran);
             //如果是第一次的就销毁这次的子弹（为什么会发生？？？？？？？？？？？？？）
             //if(m_shotTime == 0)
             //{
             //    Destroy(rockObj);
             //    m_shotTime++;
             //}
            m_Timer = m_AttackTime;
         }
     }

  

  
}
