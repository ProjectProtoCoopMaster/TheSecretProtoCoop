using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{
    [ExecuteInEditMode]
    public class ElectricalLinePlacement : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        private GameObject firstTransform;
        private GameObject secondTransform;
        private Vector3 offsetToFirstTransform;
        private Vector3 offsetToSecondTransform;

        public void OnEnable()
        {
            if (firstTransform != null && offsetToFirstTransform == null)
            {
                offsetToFirstTransform = lineRenderer.GetPosition(0) - firstTransform.transform.position;
            }

            if (secondTransform != null && offsetToSecondTransform == null)
            {
                offsetToSecondTransform = lineRenderer.GetPosition(1) - secondTransform.transform.position;
            }

        }

        public void Initialize(GameObject first, GameObject second)
        {
            firstTransform = first;
            secondTransform = second;
        }

        public void Update()
        {
            if (firstTransform != null & secondTransform != null)
            {
                lineRenderer.SetPosition(0, firstTransform.transform.position - offsetToFirstTransform);
                lineRenderer.SetPosition(3, secondTransform.transform.position - offsetToSecondTransform);
            }

        }

    }
}

