using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Xml;
using System.IO;

using System.Security;


//ChoiceScene的场景控制脚本
public class ChoiceScene : MonoBehaviour 
{
    //要出现的敌人
    Dictionary<string, Monster> m_monsters;
    // Use this for initialization
    void Start()
    {
        m_monsters = new Dictionary<string, Monster>();
        //读取XML数据
        ReadXML();
        AddEnemyIconsToScrollView();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //读取xml文件获取敌人类型
    void ReadXML()
    {
        //读取Monster.xml文件
        SecurityParser SP = new SecurityParser();
        SP.LoadXml(Resources.Load("Scene1").ToString());
        SecurityElement SE = SP.ToXml();
        foreach (SecurityElement child in SE.Children)
        {
            //比对下是否使自己所需要得节点
            if (child.Tag == "table")
            {
                //依次读取
                string enemyName = child.Attribute("enemyname");
                string enemyLevel = child.Attribute("level");
                string key = enemyName + enemyLevel;
                //查询是否有了
                if (m_monsters.ContainsKey(key))
                {
                    continue;
                }
                //查询对应的具体信息
                Monster newMonster = DataBase.GetInstance().m_MonsterDatas[key];
                m_monsters.Add(key, newMonster);
            }
        }
    }

    //添加敌人的图标
    void AddEnemyIconsToScrollView()
    {
        int index = 1;
        foreach (Monster monster in m_monsters.Values)
        {
            Vector3 pos = ChoiceSceneUI.Instance().IconInitPos + new Vector3((index - 1) * ChoiceSceneUI.Instance().m_gridInterval, 0, 0);
            ChoiceSceneUI.Instance().AddEnemyIconToScrollView(monster, pos);
            ++index;
        }
    }


    //保存玩家的信息
    void Save()
    {

    }


}
