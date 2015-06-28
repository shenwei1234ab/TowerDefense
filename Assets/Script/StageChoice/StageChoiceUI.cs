using UnityEngine;
using System.Collections;

public class StageChoiceUI : MonoBehaviour 
{
    public StageChoiceUIButton[] m_choiceButtons;
    StageChoiceUIButton m_lastStageButton;
	// Use this for initialization
	void Start () 
    {
          int curStage = DataBase.GetInstance().GetStage();
        for(int i=0;i<m_choiceButtons.Length;++i)
        {
            if(m_choiceButtons[i].m_stageLevel <= curStage)
            {
                if(m_choiceButtons[i].m_stageLevel == curStage)
                {
                    m_lastStageButton = m_choiceButtons[i];
                }
                m_choiceButtons[i].UnLock();
                m_choiceButtons[i].Selected();
            }
            else
            {
                m_choiceButtons[i].Lock();
            }
        }
	}
	



	// Update is called once per frame
	void Update () 
    {
	
	}


    public void StageButtonClickOn(GameObject button)
    {
        StageChoiceUIButton stageButton = button.GetComponent<StageChoiceUIButton>();
        
        if(stageButton.GetStageStatus() == StageStatus.UnLock)
        {  
            m_lastStageButton.NotSelected();
            stageButton.Selected();
          
            m_lastStageButton = stageButton;
        }
    }

    public void StartGame()
    {
        if(!m_lastStageButton)
        {
            Debug.Log("请选择一关");
        }
        else
        {
            DataBase.GetInstance().SelectStage = m_lastStageButton.m_stageLevel;
            Application.LoadLevel("StagePrepare");
        }
    }




}
