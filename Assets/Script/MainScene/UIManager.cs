using UnityEngine;
using System.Collections;
namespace MainScene
{
    public class UIManager : MonoBehaviour
    {
        //记录当前操作的模块
        GameObject m_CurrenModules;

        public GameObject m_Buttons;

        //输入模块
        public GameObject m_LoginModule;

        //注册模块
        public GameObject m_RegisterModule;



        void Start()
        {
            m_instance = this;
            m_CurrenModules = m_Buttons;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private static UIManager m_instance;
        public static UIManager Instance
        {
            get
            {
                return m_instance;
            }
        }


        public void OnVolumnChanged()
        {
          //  m_fVolumn = UISlider.current.value;
        }

        public void OnDifficultyChanged()
        {
            //switch (UIPopupList.current.value.Trim())
            //{
            //    case "简单":
            //        m_Difficulty = GameDifficulty.EASY;
            //        break;
            //    case "困难":
            //        m_Difficulty = GameDifficulty.HARD;
            //        break;
            //    case "地狱":
            //        m_Difficulty = GameDifficulty.HELL;
            //        break;
            //}
        }

        public void OnWindowsChanged()
        {
          //  m_IfWindows = UIToggle.current.value;
        }



        public void OnLoginClick()
        {
            m_CurrenModules.SetActive(false);
            //出现输入用户名和密码的输入框
            m_LoginModule.SetActive(true);
            m_CurrenModules = m_LoginModule;
        }


        //提交登陆信息
        public void PostLogin()
        {
            m_LoginModule.SendMessage("Post");
        }



        public void LoginCancel()
        {
            m_CurrenModules.SetActive(false);
            m_Buttons.SetActive(true);
            m_CurrenModules = m_Buttons;
        }


        //start界面单击了选项
        public void OnOptionClick()
        {
           // m_StartTween.PlayForward();
           // m_OptionTween.PlayForward();
        }

        /// <summary>
        /// 点击注册用户
        /// </summary>
        public void OnRegisterClick()
        {
            m_CurrenModules.SetActive(false);
            m_RegisterModule.SetActive(true);
            m_CurrenModules = m_RegisterModule;
        }

        /// <summary>
        /// 提交注册用户
        /// </summary>
        public void PostRegister()
        {
            //两次输入是否一致


        }

        public void RegisterCancel()
        {
            m_CurrenModules.SetActive(false);
            m_LoginModule.SetActive(true);
            m_CurrenModules = m_LoginModule;
        }





        public void OnExitClick()
        {
            Debug.Log("离开游戏");
        }



        //option界面完成了选项的调整
        public void OnFinishOption()
        {
           // m_OptionTween.PlayReverse();
           // m_StartTween.PlayReverse();
        }



        //开始游戏
        public void StartGame()
        {
            Application.LoadLevel("StageChoice");
        }
    }
}

