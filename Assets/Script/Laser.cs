using UnityEngine;
using System.Collections;

public class Laser : Bullet 
{
    //怪物的掩码
    int m_LayerMask;
    //发射点
    public Transform m_StartPoint;
    //发射点到目标点的距离
    private float m_Distance;
  
    //射线的贴图
    public Transform m_Line;
    public GameObject FXef;//激光击中物体的粒子效果
    // Use this for initialization
    // Update is called once per frame
    //void Update () 
    //{
    //    RaycastHit hit;
    //    Vector3 Sc;// 变换大小
    //    Sc.x=0.5f;
    //    Sc.z=0.5f;
    //    //发射射线，通过获取射线碰撞后返回的距离来变换激光模型的y轴上的值
    //    if (Physics.Raycast(transform.position, this.transform.forward, out hit)){
    //        Debug.DrawLine(this.transform.position,hit.point);
    //        Sc.y=hit.distance;
    //        FXef.transform.position=hit.point;//让激光击中物体的粒子效果的空间位置与射线碰撞的点的空间位置保持一致；
    //        FXef.SetActive(true);
    //    }
    //    //当激光没有碰撞到物体时，让射线的长度保持为500m，并设置击中效果为不显示
    //    else{
    //        Sc.y=500;
    //        FXef.SetActive(false);
    //    }

    //    Line.transform.localScale=Sc;
    //}

    //public override bool ReachTarget(Vector3 targetPos)
    //{


    //} 
    public override void Start()
    {
        m_transform = gameObject.transform;
        m_LayerMask = LayerMask.GetMask("Monster");
    }
    void Update()
    {
        if (m_AttackEnemy == null)
        {
            return;
        }
        RaycastHit hit;
        if (Physics.Raycast(m_StartPoint.position, m_transform.forward, out hit,m_Line.localScale.y, m_LayerMask))
        {
            Debug.Log("射线射到了");
            //射到了怪物
            hit.collider.gameObject.SendMessage("GetDamage", m_AttackPower);
            //Destroy(this.gameObject);
            gameObject.SendMessage("Recovery");
        }
        else
        {
            //是否到达目标点
            if (m_Line.localScale.y >= m_Distance)
            {
                //到达目标点
                //Destroy(this.gameObject);
                gameObject.SendMessage("Recovery");
            }
            //没有到达就继续走
            m_Line.localScale += new Vector3(0, m_movingSpeed * Time.deltaTime, 0);
        }
    }

    public override void Shoot(Transform pos)
    {
        m_AttackEnemy = pos.position;
        m_Line.up = (m_AttackEnemy - m_StartPoint.position).normalized;
        m_Distance = Vector3.Distance(m_AttackEnemy, m_StartPoint.position);
    }


    public  void OnTriggerEnter(Collider collider)
    {
        //射击到了怪物
        if (collider.tag.CompareTo("Monster") == 0)
        {
            //调用怪物的getdamage方法
            collider.gameObject.SendMessage("GetDamage", m_AttackPower);
            //Destroy(gameObject);
            gameObject.SendMessage("Recovery");
        }
    }
}
