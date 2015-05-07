using UnityEngine;
using System.Collections;

//技能响应
public class Skill : MonoBehaviour 
{
    private bool m_ifColding = false;
    private UISprite m_SkillSprite;
    public float m_ColdingTime = 2;
	void Start () 
    {
        m_SkillSprite = transform.Find("SkillMask").GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKey(KeyCode.A)&& !m_ifColding)
        {
            //释放技能
            //todo.....................
            //开始冷却
            m_SkillSprite.fillAmount = 1;
            m_ifColding = true;
        }
        //技能正在冷却
        if(m_ifColding)
        {
            m_SkillSprite.fillAmount -= 1 / m_ColdingTime * Time.deltaTime;
            if(m_SkillSprite.fillAmount < 0.05)
            {
                m_SkillSprite.fillAmount = 0;
                m_ifColding = false;
            }
        }
	}
}
