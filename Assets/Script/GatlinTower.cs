using UnityEngine;
using System.Collections;

public class GatlinTower : Tower 
{
    public GameObject m_fireObj;
    private ParticleSystem m_fireParticle;
    bool m_ifFiring = false;
    // Use this for initialization
    void Start()
    {
        base.Start();
        m_fireParticle = m_fireObj.GetComponent<ParticleSystem>();
        m_fireObj.SetActive(false);
    }

    // Update is called once per frame


    public override void ScanForEnemy()
    {
        base.ScanForEnemy();
        if(m_ifFiring)
        {
            if(m_AttackEnemy == null)
            {
                m_ifFiring = false;
                m_fireObj.SetActive(false);
            }
            else
            {
                m_AttackEnemy.GetDamage(m_AttackPower);
            }
        }
    }
    
    public override void Shoot()
    {
        if (m_ifFiring)
        {
            return;
        }
        //发射火焰
       // m_fireObj.transform.LookAt(m_AttackEnemyTran.position);
        m_fireObj.SetActive(true);
       // m_fireObj.SendMessage("Shoot", m_AttackPower);
        m_ifFiring = true;
    }
}
