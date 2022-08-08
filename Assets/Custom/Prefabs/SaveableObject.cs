using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;


public class SaveableObject : MonoBehaviour
{
  
    public float goid;
    public SaveGame saveGame;

    private void Awake()
    {

        if(goid == 0)
        {
            GenerateID();
        }
    }
    private void Start()
    {
        saveGame = GameObject.FindObjectOfType<SaveGame>();
    }
  
    private void GenerateID()
    {
        //id = Guid.NewGuid().ToString();
        goid = Random.Range(1, 999999999);
    }
}
