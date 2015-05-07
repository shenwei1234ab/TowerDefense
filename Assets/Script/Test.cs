using UnityEngine;
using System.Collections;

//测试鼠标事件
public class Test : MonoBehaviour
{



    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("撞到了什么");
        Debug.Log(collider.tag);
    }


	// Update is called once per frame
	void Update () 
    {
      if(Input.GetKeyDown(KeyCode.A))
      {
          gameObject.transform.Translate(-gameObject.transform.right* 2);
      }
      else  if(Input.GetKeyDown(KeyCode.D))
      {
          gameObject.transform.Translate(gameObject.transform.right * 2);
      }
      else if (Input.GetKeyDown(KeyCode.W))
      {
          gameObject.transform.Translate(gameObject.transform.forward * 2);
      }
      else if (Input.GetKeyDown(KeyCode.S))
      {
          gameObject.transform.Translate(-gameObject.transform.forward * 2);
      }
	}
}
