using UnityEngine;
using System.Collections;

public class RocketTower : Tower 
{
    public Transform m_ShootPos2;

	// Update is called once per frame
    public override void Shoot()
    {
        CreateRocket(m_ShootPos.position);
        if(m_ShootPos2)
        {
            CreateRocket(m_ShootPos2.position);
        }
       
    }

    void CreateRocket(Vector3 pos)
    {
        GameObject rockObj = PoolManager.GetInstance().GetObject(m_Rockets.name, pos, m_TopTower.rotation);
       // GameObject rockObj = (GameObject)Instantiate(m_Rockets, pos, m_TopTower.rotation);
        //使子弹拥有和塔一样的攻击力
        Bullet bullet = rockObj.GetComponent<Bullet>();
        bullet.m_AttackPower = m_AttackPower;
        bullet.Shoot(m_AttackEnemyTran);
        m_Timer = m_AttackTime;
    }
}
