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


        public Sprite[] testSpriteTextures;
        private Shader spriteDefaultShader;

        private Mesh levelMesh;

        private void Awake()
        {
            // this is just a current workaround to make unity focus to the scene view when in the editor...remove this later if u dont want that
            #if UNITY_EDITOR
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
            // initialize();
            generate();
            #endif

            /*#if UNITY_EDITOR
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
            StartCoroutine(generateOverTime(0.5f));
            #endif*/
        }

        public void generate()
        {
            initMesh();
            initMeshRenderer(); 
        }

        private Vector3[] calculateVertices()
        {
            Vector3[] vertices = new Vector3[(levelWidth + 1) * (levelHeight + 1)];

            for (int i = 0, y = 0; y <= levelHeight; y++)
            {
                for (int x = 0; x <= levelWidth; x++, i++)
                {
                    vertices[i] = new Vector3(x, y);
                }
            }

            return vertices;
        }

        private int[] calculateTriangles()
        {
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

            return triangles;
        }

        private void initMesh()
        {
            Vector3[] levelVertices = calculateVertices();
            int[] levelTriangles = calculateTriangles();

            if (levelVertices.Length != 0 & levelTriangles.Length != 0)
            {
                levelMesh = new Mesh();
                levelMesh.name = "Level Grid";
                levelMesh.vertices = levelVertices;
                levelMesh.triangles = levelTriangles;
                levelMesh.RecalculateNormals();

                GetComponent<MeshFilter>().mesh = levelMesh;
            }
            else
            {
                return; // get out if there are no vertices or triangles to calculate a mesh
            }
        }

        private void initMeshRenderer()
        {
            if (testSpriteTextures.Length != 0)
            {
                MeshRenderer levelMeshRenderer = GetComponent<MeshRenderer>();
                // levelMeshRenderer.materials = (Material[])testSpriteTextures;
                spriteDefaultShader = Shader.Find("Sprites/Default");
                levelMeshRenderer.material.shader = spriteDefaultShader;
            }
            else
            {
                return; // get out if there isn't a sprite
            }
        }
    

        // debug functions
    }
}
