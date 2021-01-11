using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Gameplay.Mobile
{

    public class VisionCone : MonoBehaviour
    {
        private Mesh mesh;
        public float radius;
        public int numberVertices;
        public float angle;
        private float newAngle;
        private  Vector3 newVerticePos;
        Vector3[] vertices;
        public  List<Vector3> newVerticesPos;
        public int[] triangles;

        private void Start()
        {
            mesh = new Mesh();

            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void Update()
        {

            vertices = new Vector3[numberVertices];
            triangles = new int[(numberVertices * 3)];
            newAngle = angle / (float)(numberVertices);
            newVerticesPos.Clear();
            vertices[0] = transform.position;
            newVerticesPos.Add(vertices[0]);
            for (int i = 1; i < numberVertices; ++i)
            {
                RaycastHit hit;
                newVerticePos = Quaternion.AngleAxis((newAngle) * (float)(i - 1), transform.up) * transform.forward * radius ;
                

                if (Physics.Raycast(vertices[0], newVerticePos, out hit, radius))
                {
                    vertices[i] = new Vector3(hit.point.x, vertices[i].y, hit.point.z);
                   
                }
                else
                {
                    vertices[i] = newVerticePos + transform.position;

                }
                vertices[i] = transform.rotation * (vertices[i] - vertices[0]) + vertices[0];
                newVerticesPos.Add(vertices[i]);
            }
            


            for (int i = 0; i + 2 < numberVertices; ++i)
            {
                int index = i * 3;
                triangles[index + 0] = 0;
                triangles[index + 1] = i + 1;
                triangles[index + 2] = i + 2;
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;

        }

        private void OnDrawGizmos()
        {
            if(newVerticesPos.Count != 0)
            {
                for (int i = 0; i < newVerticesPos.Count; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(newVerticesPos[i], .1f);

                }
            }

            Vector3 axis = Quaternion.AngleAxis((transform.eulerAngles.y), transform.up) * transform.forward * radius;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, newVerticePos);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, axis);

        }

    }
}

