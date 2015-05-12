﻿using UnityEngine;
using System.Collections;

public class RocketTower : Tower 
{
    public Transform m_ShootPos2;

	// Update is called once per frame
    public override void Shoot()
    {
        CreateRocket(m_ShootPos.position);
        CreateRocket(m_ShootPos2.position);
    }

    void CreateRocket(Vector3 pos)
    {
        GameObject rockObj = (GameObject)Instantiate(m_Rockets, pos, m_TopTower.rotation);
        //使子弹拥有和塔一样的攻击力
        Bullet bullet = rockObj.GetComponent<Bullet>();
        bullet.m_AttackPower = m_AttackPower;
        bullet.Shoot(m_AttackEnemyTran);
        m_Timer = m_AttackTime;
    }
}
