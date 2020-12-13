using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateXrayShader : MonoBehaviour
{
    public MeshRenderer xrayObject;
    public Material xrayMaterial, normalMaterial;

    private void Start()
    {
        xrayObject.material = normalMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) xrayObject.material = xrayMaterial;
        if (Input.GetKeyUp(KeyCode.Space)) xrayObject.material = normalMaterial;
    }
}
