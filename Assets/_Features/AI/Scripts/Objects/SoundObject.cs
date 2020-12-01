using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.AI;

public class SoundObject : MonoBehaviour
{
    public LayerMask layerMask;
    public float radius;

    private bool active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor" && active)
        {
            _Distraction();
        }
    }

    public void _Distraction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, layerMask);

        foreach (Collider col in colliders)
        {
            //Debug.Log(col.transform.parent.name);
            if (col.transform.parent.tag == "Enemy")
            {
                GuardManager guard = col.transform.parent.GetComponent<GuardManager>();

                guard.DistractTo(transform.position);
            }
        }

        //active = false;
    }

    public void Grab()
    {
        Wait();

        active = true;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
    }
}
