﻿using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve curve, int levelOfDetail)
    {
        //To avoid weird hills spikes now every thread has its own animation curve. :(
        AnimationCurve heightCurve = new AnimationCurve(curve.keys);
        
        int width = heightMap.GetLength(0); // Getting the array´s 1st dimension
        int height = heightMap.GetLength(1); // Getting the array´s 2nd dimension
        
        // To set the vertex to the center
        float topLeftX = (width - 1)/-2f;
        float topLeftZ = (height - 1)/2f;

        int meshSimplificationIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;
        
        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;
        
        for (int y = 0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {
                float meshHeight = heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier;
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x,meshHeight, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                
                vertexIndex++;
            }
        }

        return meshData;
    }
}
