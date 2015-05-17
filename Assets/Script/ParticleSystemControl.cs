using UnityEngine;
using System.Collections;

//粒子系统控制,过了粒子的播放完成就通知注册者  
public class ParticleSystemControl : MonoBehaviour
{
    public delegate void ParticleComplete(GameObject sender);
    public event ParticleComplete m_particleCompleteEvent;
    public GameObject m_particleObj;
    ParticleSystem m_particleSystem;
    // Use this for initialization
    void Start()
    {
        m_particleSystem = m_particleObj.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (m_particleSystem.IsAlive())
        {
            return;
        }
        else
        {
            m_particleCompleteEvent(this.gameObject);
        }
    }
}
