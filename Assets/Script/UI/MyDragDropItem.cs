using UnityEngine;
using System.Collections;

public class MyDragDropItem :UIDragDropItem 
{

    protected override void OnDragDropMove(Vector3 delta)
    {
        base.OnDragDropMove(delta);

    }
    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        Debug.Log("start");
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        //进行处理
        if(!surface)
        {
            return;
        }
        //位于中心点
        this.transform.parent = surface.transform;
        this.transform.localPosition = Vector3.one;
    }



}
