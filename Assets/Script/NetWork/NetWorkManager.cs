using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;

public class NetWorkManager  :MonoBehaviour
{
  
    void Awake()
    {
        m_instance = this;
    }
    static NetWorkManager m_instance=null;
    
    public static NetWorkManager GetInstance()
    {
        return m_instance;
    }
    
    IEnumerator IPostData<T>(object postObj,string strUrl,T resObj,GetDateHandle getDataFun)
    {
        System.Collections.Hashtable headers = new System.Collections.Hashtable();
        headers.Add("Content-Type", "multipart/form-data");
        //headers.Add("Content-Type", "application/x-www-form-urlencoded");
        //
        string postData = Protocal.ObjectToJson(postObj);
        Debug.Log("postData is " + postData);
        byte[] bs = System.Text.UTF8Encoding.UTF8.GetBytes(postData);

        WWW www = new WWW(strUrl, bs, headers);
        yield return www;
        if (www.error != null)
        {
           // m_info = www.error;
            Debug.Log("Connect Error");
            //resObj.StateCode = 1003;
           // yield return null;
        }
        else
        {
           // Debug.Log(i);
             //反序列化返回的json字符
              resObj = (T)Protocal.JsonToObject<T>(www.text);
              getDataFun(resObj);
        }
        
    }
    StateCode m_StateCode;


    public delegate void GetDateHandle(object  getDate);


    public void Login(Protocal.HTTP_RequestLogin reqLogin, Protocal.HTTP_ResponseLogin resLogin, GetDateHandle getDataFun)
    {
        //Debug.Log("Login");
        StartCoroutine(IPostData(reqLogin, "127.0.0.1:3000/login", resLogin, getDataFun));
      
    }


    public void Register(Protocal.HTTP_RequestRegister reqRegister,Protocal.HTTP_ResponseRegister resRegister,GetDateHandle getDataFun)
    {
        StartCoroutine(IPostData(reqRegister, "127.0.0.1:3000/register", resRegister, getDataFun));
    }

    void Update()
    {
       
    }
}
