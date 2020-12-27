using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Mobile
{

    public class VisionCone : MonoBehaviour
    {
        private Mesh mesh;
        public float radius;
        public int numberVertices;
        public float angle;
        private float newAngle;
        private void Start()
        {
            //mesh = new Mesh();
            //GetComponent<MeshFilter>().mesh = mesh;
            //mesh.vertices = vertices;
            //mesh.uv = uv;
            //mesh.triangles = triangles;

        }


        private void Update()
        {
            
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            Vector3[] vertices = new Vector3[numberVertices];
            int[] triangles = new int[(numberVertices * 3)];

            vertices[0] = transform.localPosition;
            newAngle = angle / (float)(numberVertices - 1);

            for (int i = 1; i < numberVertices; ++i)
            {
                RaycastHit hit;
                //Vector3 axis = Quaternion.AngleAxis(((newAngle + transform.eulerAngles.y) * (float)(i - 1)), transform.up) * transform.forward * radius;
                Vector3 newVerticePos = Quaternion.AngleAxis((newAngle) * (float)(i - 1), transform.up) *  transform.forward* radius;
                if (Physics.Raycast(transform.position, newVerticePos, out hit, radius))
                {
                    vertices[i] = new Vector3( hit.point.x,vertices[i].y,hit.point.z);
                }
                else
                {
                    vertices[i] = newVerticePos;
                }

            }

            for (int i = 0; i + 2 < numberVertices; ++i)
            {
                int index = i * 3;
                triangles[index + 0] = 0;
                triangles[index + 1] = i + 1;
                triangles[index + 2] = i + 2;
            }

            int lastTriangleIndex = triangles.Length - 3;
            triangles[lastTriangleIndex + 0] = 0;
            triangles[lastTriangleIndex + 1] = numberVertices - 1;
            triangles[lastTriangleIndex + 2] = 1;

            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }

        private void OnDrawGizmos()
        {
            Vector3 axis = Quaternion.AngleAxis((transform.eulerAngles.y), transform.up) * transform.forward * radius;
            Vector3 newVerticePos = Quaternion.AngleAxis((newAngle), transform.up) * transform.forward * radius;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, newVerticePos);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, axis);
        }
    }
}

