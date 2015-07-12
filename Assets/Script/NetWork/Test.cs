using UnityEngine;
using System.Collections;
using LitJson;

public class Person
{
    public string name;
    public int age;
}
public class Test : MonoBehaviour
{
    //public void PostData()
    //{
    //    Protocal.HTTP_RequestLogin reqLogin = new Protocal.HTTP_RequestLogin("shenwei","645127861");
    //    Protocal.HTTP_ResponseLogin resLogin = new Protocal.HTTP_ResponseLogin();
    //    //设置request对象和response对象和得到http请求的回调函数
    //    NetWorkManager.GetInstance().Login(reqLogin, resLogin, this.LoginHandler);
    //}

    
    //void LoginHandler(object getDate)
    //{
    //    Protocal.HTTP_ResponseLogin resDate = (Protocal.HTTP_ResponseLogin)getDate;
    //    Debug.Log(resDate.StateCode);
    //    Debug.Log(resDate.LogMessage);
    //    Debug.Log(resDate.UserID);
    //}






    // Use this for initialization
    void Start()
    {
        //PersonToJson();
        //JsonToPerson();
    }

    // Update is called once per frame
    void Update()
    {

    }


}


