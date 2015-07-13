using UnityEngine;
using System.Collections;

namespace MainScene
{
    public class RegisterModule : MonoBehaviour
    {

        public UILabel m_InputUsername;
        public UILabel m_InputPassword;
        public UILabel m_RepeatPassword;
        public UILabel m_Error;


        //判断输入的内容是否合法
        bool IfLegal()
        {
            if (m_InputUsername.text.Trim().Equals("") || m_InputPassword.text.Trim().Equals(""))
            {
                ShowError("用户名或密码不为空！！");
                return false;
            }
            if (m_InputPassword.text != m_RepeatPassword.text)
            {
                ShowError("两次输入的密码不一致！！");
                return false;
            }
            return true;
        }
        

        public void Post()
        {
            if (!IfLegal())
            {
                return; 
            }
            Protocal.HTTP_RequestRegister reqRegister = new Protocal.HTTP_RequestRegister(m_InputUsername.text, m_InputPassword.text);
            Protocal.HTTP_ResponseRegister resRegister = new Protocal.HTTP_ResponseRegister();
            //设置request对象和response对象和得到http请求的回调函数
            NetWorkManager.GetInstance().Register(reqRegister,resRegister, this.RegisterHandler);
        }

        void RegisterHandler(object getDate)
        {
            Protocal.HTTP_ResponseRegister resDate = (Protocal.HTTP_ResponseRegister)getDate;
            //注册成功
            if (resDate.StateCode == (uint)StateCode.RegisterSuccess)
            {
                UIManager.Instance.StartGame();
            }
            else
            {
                ShowError(resDate.LogMessage);
            }
        }


       

        public void ShowError(string errorMessage)
        {
            m_Error.gameObject.SetActive(true);
            m_Error.text = errorMessage;
        }
    }
}

