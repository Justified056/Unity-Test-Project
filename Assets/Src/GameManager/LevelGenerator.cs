using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGen
{
    class LevelGenerator : MonoBehaviour
    {
        public int levelWidth;
        public int levelHeight;

        private Vector3[] vertices;

        private Mesh mesh;


        private void Awake()
        {
            // generate();

            #if UNITY_EDITOR
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
            StartCoroutine(generateOverTime(0.5f));
            #endif
        }

        public void generate()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.name = "LevelGrid";

            vertices = new Vector3[(levelWidth + 1) * (levelHeight + 1)];

            for(int i = 0, y = 0; y <= levelHeight; y++)
            {
                for(int x =0; x <= levelWidth; x++, i++)
                {
                    vertices[i] = new Vector3(x, y);
                }
            }

            mesh.vertices = vertices;

            int[] triangles = new int[levelWidth * levelHeight * 6];
            for (int ti = 0, vi = 0, y = 0; y < levelHeight; y++, vi++) // ti = triangle index, vi = vertex index
            {
                for (int x = 0; x < levelWidth; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + levelWidth + 1;
                    triangles[ti + 5] = vi + levelWidth + 2;
                }
            }

            mesh.triangles = triangles;
        }
    

        // debug functions

        // plz dont call this and the other generate.....could cause some interesting issues
        private IEnumerator generateOverTime(float secondsToWait)
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            mesh.name = "LevelGrid";

            WaitForSeconds waitTime = new WaitForSeconds(secondsToWait);

            vertices = new Vector3[(levelWidth + 1) * (levelHeight + 1)];

            for (int i = 0, y = 0; y <= levelHeight; y++)
            {
                for (int x = 0; x <= levelWidth; x++, i++)
                {
                    vertices[i] = new Vector3(x, y);
                    // yield return waitTime; // uncomment to watch the grid vertex points appear
                }
            }

            mesh.vertices = vertices;

            int[] triangles = new int[levelWidth * levelHeight * 6];
            for(int ti = 0, vi = 0, y = 0; y < levelHeight; y++, vi++) // ti = triangle index, vi = vertex index
            {
                for (int x = 0; x < levelWidth; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + levelWidth + 1;
                    triangles[ti + 5] = vi + levelWidth + 2;
                    mesh.triangles = triangles; // comment out if u dont need to see the triangles go in one by one
                    yield return waitTime;
                }
            }

            // mesh.triangles = triangles; // uncomment to update after the triangles in the mesh are calculated
        }

        private void OnDrawGizmos() 
        {
            if(vertices == null)
            {
                return;
            }

            Gizmos.color = Color.black;

            for(int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }
        }
    }
}
