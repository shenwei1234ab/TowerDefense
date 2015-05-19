using UnityEngine;
using System.Collections;

//一般的游戏UIButton
public enum NormalUIButtonType
{
    OK,
    BackToMain,
}
public class NormalUIButton :UiButton
{
    public NormalUIButtonType m_uiButtonType;
	// Use this for initialization
	protected void Start () 
    {
        //向ChoiceScene注册事件
        ChoiceSceneInputSystem.Instance().RegistUIEvent(this);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
