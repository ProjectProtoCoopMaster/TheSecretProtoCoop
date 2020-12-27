using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay.AI;

public class SoundObject : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float radius; public float Radius { get => radius; }

    private bool active = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor" && active)
        {
            Distraction();
        }
    }

    private void Distraction()
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

    public void WaitForGrab()
    {
        Wait();

        active = true;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
    }
}
