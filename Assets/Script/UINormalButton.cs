using UnityEngine;
using System.Collections;
public enum ButtonType
{
    //CreateTower,
    UpdateTower,
    DestoryTower,
    Exit,
    NextScene,
    BackToChoice,
    BackToPrepare,
    PauseGame,
    ResumeGame,
}
public class UINormalButton : UiButton 
{
    public ButtonType m_buttonType;
	// Use this for initialization
	void Start () 
    {
        InputSystem.Instance().RegistUIEvent(this);
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
