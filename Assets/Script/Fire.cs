using UnityEngine;
using System.Collections;

//控制火焰
public class Fire : MonoBehaviour
{   
    //伤害
    int m_attackPower;
	public void Shoot(int power )
    {
        m_attackPower = power;
    }

	// Update is called once per frame

    void OnParticleCollision(GameObject other)
    {
        //射击到了怪物
        if (other.tag.CompareTo("Monster") == 0)
        {
            //调用怪物的getdamage方法
            other.SendMessage("GetDamage", m_attackPower);
        }
    }

    void Update()
    {
        
    }
}
