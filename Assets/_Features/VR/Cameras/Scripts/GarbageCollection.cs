using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GarbageCollection : MonoBehaviour
{
   public  WaitForSeconds customWait = new WaitForSeconds(.15f);
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Collect());
    }

    IEnumerator Collect()
    {
        while(true)
        {
            yield return customWait;
            GC.Collect();
        }
    }
}
