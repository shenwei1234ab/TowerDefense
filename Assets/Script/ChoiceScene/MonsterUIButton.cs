using UnityEngine;
using System.Collections;



public class MonsterUIButton :UiButton 
{
    //存储代表的monster的信息
    public Monster m_monster;
    public GameObject m_textObj;
    public GameObject m_spriteObj;
    UILabel m_label;
    UISprite m_sprite;
    void Awake()
    {
        m_label = m_textObj.GetComponent<UILabel>();
        m_sprite = m_spriteObj.GetComponent<UISprite>();
    }
    // Use this for initialization
    protected void Start()
    {
        //向ChoiceScene注册事件
        ChoiceSceneInputSystem.Instance().RegistUIEvent(this);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void SetMonster(Monster monster)
    {
        m_monster = monster;
    }


    public void SetText(string text)
    {
        m_label.text = text;
    }

    public void SetSprite(string spriteName)
    {
        m_sprite.spriteName = spriteName;
    }


}
