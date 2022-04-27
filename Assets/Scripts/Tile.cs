using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile 
{
    private bool free;
    private Vector3 position;
    private GameObject prefab;

    public bool Free { get => free; set => free = value; }
    public Vector3 Postion { get => position; set => position = value; }
    public GameObject Prefab { get => prefab; set => prefab = value; }

    public Tile(Vector3 pos, bool state)
    {
        free = state;
        position = pos;
        Prefab = null;
    }

    
}
