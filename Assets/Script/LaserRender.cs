using UnityEngine;
using System.Collections;

public class LaserRender : MonoBehaviour 
{
    public GameObject m_Laser;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider collider)
    {
       
        m_Laser.SendMessage("OnTriggerEnter", collider);
    }
}
