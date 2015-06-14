using UnityEngine;
using System.Collections;


//泛型单例
public class CSingleTon <T>:MonoBehaviour where T:new() 
{
    class SingleTonCreator
    {
        static SingleTonCreator()
        {

        }
        internal static readonly T m_instance = new T();  
    }
  
    public static T Instance
    {
        get
        {
            return SingleTonCreator.m_instance;
        }
    }

    void Awake()
    {

    }
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
