using UnityEngine;
using System.Collections;

namespace MainScene
{
    public class LoginModule : MonoBehaviour
    {

        public UILabel m_InputUsername;
        public UILabel m_InputPassword;
        public UILabel m_Error;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Post()
        {
            //判断判断用户名和密码是否正确
            Protocal.HTTP_RequestLogin reqLogin = new Protocal.HTTP_RequestLogin(m_InputUsername.text, m_InputPassword.text);
            Protocal.HTTP_ResponseLogin resLogin = new Protocal.HTTP_ResponseLogin();
            //设置request对象和response对象和得到http请求的回调函数
            NetWorkManager.GetInstance().Login(reqLogin, resLogin, this.LoginHandler);
        }

        void LoginHandler(object getDate)
        {
            Protocal.HTTP_ResponseLogin resDate = (Protocal.HTTP_ResponseLogin)getDate;
            //登陆成功 
            if (resDate.StateCode == (uint)StateCode.LoginSuccess)
            {
                UIManager.Instance.StartGame();
            }
            else
            {
                ShowError(resDate.LogMessage);
            }
            Debug.Log(resDate.StateCode);
            Debug.Log(resDate.LogMessage);
            Debug.Log(resDate.UserID);
        }


        public void ShowError(string errorMessage)
        {
            m_Error.gameObject.SetActive(true);
            m_Error.text = errorMessage;
        }

    }

}
