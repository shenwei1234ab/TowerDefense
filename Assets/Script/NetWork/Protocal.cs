using UnityEngine;
using System;
using System.Collections;
using LitJson;

public class Protocal : MonoBehaviour {

    public class HTTP_RequestLogin
    {
        public HTTP_RequestLogin(string username,string password)
        {
            Username = username;
            Password = password;
        }
        public string Username;
        public string Password;
    }


    public class HTTP_ResponseLogin
    {
        public uint StateCode;
        public String LogMessage;
        public String UserID;
    }





    //把某个object对象序列化为Json  string
    public static string ObjectToJson(object jsonObject)
    {
        return JsonMapper.ToJson(jsonObject);
    }



    //把Json string反序列化为C# 对象
    public static  T JsonToObject<T>(string jsonString)
    {
        return  JsonMapper.ToObject<T>(jsonString);
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
