using UnityEngine;
using System.Collections;

public class MyUIFollowTarget : MonoBehaviour {

    //要跟随的物体
    public Transform m_targetTransform;


    private Transform m_Transform;
    // Use this for initialization
    private bool m_ifVisible = false;

    //物体到摄像机的距离
    private float m_Distance;
    void Awake()
    {
        m_Transform = gameObject.transform;
    }
    void Start()
    {
        m_Distance = Vector3.Distance(m_targetTransform.position, Camera.main.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = Camera.main.WorldToViewportPoint(m_targetTransform.position);
        bool isVisible = pos.x > 0f && pos.x < 1f && pos.y > 0f && pos.y < 1f;
        //Debug.Log(pos);
        SetVisible(isVisible);
        if (m_ifVisible)
        {
            float curDistance = Vector3.Distance(m_targetTransform.position, Camera.main.transform.position);

            Vector3 worldPoint = UICamera.currentCamera.ViewportToWorldPoint(pos);
            worldPoint.z = 0;
            //近大远小
            m_Transform.localScale = Vector3.one * (m_Distance / curDistance);
            m_Transform.position = worldPoint;
        }

    }



    void SetVisible(bool val)
    {
        m_ifVisible = val;
        for (int i = 0, imax = m_Transform.childCount; i < imax; ++i)
        {
            NGUITools.SetActive(m_Transform.GetChild(i).gameObject, val);
        }
    }
}
