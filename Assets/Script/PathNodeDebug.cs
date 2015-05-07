﻿using UnityEngine;
using System.Collections;
using UnityEditor;
public class PathNodeDebug : ScriptableObject {
    public bool m_ifShowAllNode = false;
	// Use this for initialization
	void Start () 
    {
	
	}
    

    
    [MenuItem("Debug/Show All PathNode")]
    static void ShowAll()
    {
        GameObject nodeParent = GameObject.Find("PathNodeManager");
        if(nodeParent)
        {
            foreach (Transform child in nodeParent.transform)
            {
                MeshRenderer render = child.GetComponent<MeshRenderer>();
                render.enabled = true;
             }  
        }
    }

     [MenuItem("Debug/Hide All PathNode")]
    static void HideAll()
    {
        GameObject nodeParent = GameObject.Find("PathNodeManager");
        if (nodeParent)
        {
            foreach (Transform child in nodeParent.transform)
            {
                MeshRenderer render = child.GetComponent<MeshRenderer>();
                render.enabled = false;
            }
        }
    }


    [MenuItem("Debug/Hide All Plane")]
    static void HideAllPlane()
    {
        GameObject nodeParent = GameObject.Find("PlaneManager");
        if (nodeParent)
        {
            foreach (Transform child in nodeParent.transform)
            {
                MeshRenderer render = child.GetComponent<MeshRenderer>();
                render.enabled = false;
            }
        }
    }


    [MenuItem("Debug/Show All Plane")]
    static void ShowAllPlane()
    {
        GameObject nodeParent = GameObject.Find("PlaneManager");
        if (nodeParent)
        {
            foreach (Transform child in nodeParent.transform)
            {
                MeshRenderer render = child.GetComponent<MeshRenderer>();
                render.enabled = true;
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
