using UnityEngine;
using System.Collections;



public class Rocket : Bullet 
{
    public override void Update()
    {
        if (m_AttackEnemyTransform == null)
        {
            return;
        }
        Vector3 current = m_transform.eulerAngles;
        //转向物体的位置
        m_transform.LookAt(m_AttackEnemyTransform.position);
        Vector3 target = m_AttackEnemyTransform.eulerAngles;


        //移动
        m_transform.Translate(m_transform.forward * m_movingSpeed * Time.deltaTime, Space.World);
    }
}
