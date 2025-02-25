/*
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentID : MonoBehaviour
{
    public int ID;
    private void Awake()
    {
        GlobalObjectManager.Instance?.RegisterObject(ID, this.gameObject);
    }

    private void OnDestroy()
    {
        GlobalObjectManager.Instance?.DeRegisterObject(ID);
    }
}
*/
