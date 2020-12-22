using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    Transform playerHead;

    private void Awake()
    {
        playerHead = GameObject.Find("[PlayerHead]").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerHead);
    }
}
