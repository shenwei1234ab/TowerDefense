using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
    //要射击的点
    protected Vector3 m_AttackEnemy;
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
        m_AttackEnemy = pos.position;
    }


    public  virtual void Update()
    {
        if(m_AttackEnemy == null)
        {
            return;
        }
        bool reach = ReachTarget(m_AttackEnemy);
        if(reach)
        {
            Destroy(this.gameObject);
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

    //撞到了某个东西
    void OnTriggerEnter(Collider collider)
    {
       //射击到了怪物
       if(collider.tag.CompareTo("Monster") == 0)
       {
          //调用怪物的getdamage方法
           collider.gameObject.SendMessage("GetDamage", m_AttackPower);
           Destroy(gameObject);
       }
    }

}
