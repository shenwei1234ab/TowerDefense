using UnityEngine;
using System.Collections;

public class SingleTonTest : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        SingleTon1.Instance.test();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
