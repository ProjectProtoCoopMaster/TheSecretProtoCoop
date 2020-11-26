using Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CallableFunction function;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MyUpdate());
    }

    // Update is called once per frame
    IEnumerator MyUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);
            function.Raise();
            yield return null;
        }
    }
}
