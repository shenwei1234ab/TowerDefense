using UnityEngine;
using System.Collections;
using System;
//发射子弹,导弹
public class Bullet : MonoBehaviour 
{
    //爆炸的粒子效果
    public GameObject m_explosionObj = null;
    //爆炸的位置
    public Transform m_explosionPos = null;

    //要射击的点的位置
    protected Vector3 m_AttackEnemy;

    protected Transform m_AttackEnemyTransform;
    public float m_movingSpeed;
    [HideInInspector]
    //子弹的威力
    public int m_AttackPower=0;

    protected Transform m_transform;
	// Use this for initialization
	public virtual void Start () 
    {
        m_transform = gameObject.transform;   
	}
	


    public virtual  void Shoot(Transform pos)
    {
        m_AttackEnemyTransform = pos;
        m_AttackEnemy = pos.position;
    }


    public  virtual void Update()
    {
        if(m_AttackEnemy == null)
        {
            return;
        }
        bool reach = ReachTarget(m_AttackEnemy);
        //到达目标地点就产生explosionObj
        if(reach)
        {
            if (m_explosionObj)
            {
               // GameObject explosionObj = (GameObject)GameObject.Instantiate(m_explosionObj, m_explosionPos.position, m_explosionPos.rotation);
                GameObject explosionObj = PoolManager.GetInstance().GetObject(m_explosionObj.name, m_explosionPos.position, m_explosionPos.rotation);
                //设置爆炸脚本的回掉函数
                explosionObj.GetComponent<ParticleSystemControl>().m_particleCompleteEvent += ParticleCompleteEvent;
            }
            //使用PoolObj的Recovery回收脚本
            gameObject.SendMessage("Recovery");
        }
    }

    //是否到达目标点
    public virtual bool ReachTarget(Vector3 targetPos)
    {
        float dis = Vector3.Distance(m_transform.position, targetPos);
        if(dis<0.1f)
        {
            //到达目标点
            return true;
        }
        //没有到达就继续走
        Vector3 dir = (targetPos - m_transform.position).normalized;
        m_transform.Translate(dir * m_movingSpeed * Time.deltaTime, Space.World);
        return false;
    }

    
   public  virtual void OnTriggerEnter(Collider collider)
   {
        if (collider.tag.CompareTo("Monster") == 0)
        {
            //调用怪物的getdamage方法
            collider.gameObject.SendMessage("GetDamage", m_AttackPower);
            //如果要爆炸效果
            if (m_explosionObj)
            {
                GameObject explosionObj = PoolManager.GetInstance().GetObject(m_explosionObj.name, m_explosionPos.position, m_explosionPos.rotation);
                //设置爆炸脚本的回掉函数
                explosionObj.GetComponent<ParticleSystemControl>().m_particleCompleteEvent += ParticleCompleteEvent;
            }
           // Destroy(gameObject);
            gameObject.SendMessage("Recovery");
        }
    }

    //默认是使particleSystem消失
    public virtual void ParticleCompleteEvent(GameObject obj)
   {
      // Destroy(obj);
       Debug.Log(obj.name);
      obj.SendMessage("Recovery");
   }

}
