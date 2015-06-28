using UnityEngine;
using System.Collections;

public enum StageStatus
{
    Lock,
    UnLock
}
public class StageChoiceUIButton : UiButton{
    StageStatus m_stageStatus;
    public int m_stageLevel;
    public UISprite m_stageSprite;
    void Awake()
    {
       
    }
    // Use this for initialization
	void Start () 
    {
        //int curStage = DataBase.GetInstance().GetStage();
        //if( m_stageLevel<=curStage )
        //{
        //    if (m_stageLevel == curStage)
        //    {

        //    }
        //    //当前是解锁的
        //    UnLock();
        //}
        //else
        //{
        //    Lock();
        //}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Lock()
    {
        m_stageSprite.spriteName = "back2";
        m_stageStatus = StageStatus.Lock;
    }

    public  void UnLock()
    {
        m_stageSprite.spriteName = "button_fight";
        m_stageStatus = StageStatus.UnLock;
    }


    public StageStatus GetStageStatus()
    {
        return m_stageStatus;
    }

    
}
