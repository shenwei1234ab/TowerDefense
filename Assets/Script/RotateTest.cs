using UnityEngine;
using System.Collections;

public class RotateTest : MonoBehaviour {
    public GameObject m_target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    //void Update () 
    //{
    //    Vector3 current = transform.eulerAngles;
    //    this.transform.LookAt(m_target.transform.position);
    //    Vector3 target = this.transform.eulerAngles;


    //    Debug.Log("current.y" + current.y);
    //    Debug.Log("target.y" + target.y);
   
    //    float next = Mathf.MoveTowardsAngle(current.y, target.y, 10 * Time.deltaTime);
    //    Debug.Log("next" + next);
    //    this.transform.eulerAngles = new Vector3(current.x, next, current.z);
    //}


    void Update()
    {
        Quaternion wantQuater = Quaternion.LookRotation(m_target.transform.position - transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, wantQuater, 1 * Time.deltaTime);
    }
}
