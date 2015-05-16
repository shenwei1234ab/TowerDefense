using UnityEngine;
using System.Collections;

public class FireTower : Tower 
{

    public GameObject m_fireObj;
    private ParticleSystem m_fireParticle;
    bool m_ifFiring = false;
	// Use this for initialization
	void Start () 
    {
        base.Start();
        m_fireParticle = m_fireObj.GetComponent<ParticleSystem>();
        m_fireObj.SetActive(false);
	}

    protected override void RotateTo()
    {
        if(m_ifFiring)
        {
            return;
        }
        base.RotateTo();
    }


    public override void ScanForEnemy()
    {
        //检查粒子是否到了生存时间
        if(m_ifFiring)
        {
            if(!m_fireParticle.IsAlive())
            {
                m_ifFiring = false;
                m_fireObj.SetActive(false);
            }
        }
        base.ScanForEnemy();
    }



    public override void Shoot()
    {
        if(m_ifFiring)
        {
            //正在发射火焰就返回
            return;
        }
        //发射火焰
        m_fireObj.transform.LookAt(m_AttackEnemyTran.position);
        m_fireObj.SetActive(true);
        m_fireObj.SendMessage("Shoot", m_AttackPower);
        m_ifFiring = true;
    }
}
