using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PC_Initializer : MonoBehaviour
{
    public List<MonoBehaviour> scripts;

    private void Awake()
    {
        if(XRSettings.enabled == true)
            foreach (MonoBehaviour item in scripts)
                item.enabled = false;

        if (XRSettings.enabled == false)
            foreach (MonoBehaviour item in scripts) 
                item.enabled = true;
    }
}
