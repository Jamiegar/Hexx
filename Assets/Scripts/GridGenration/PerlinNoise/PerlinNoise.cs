using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PerlinNoise
{
    public static float[,] GenrateNoiseMap(GridPosition gridDimentions, NoiseSettings settings)
    {
        UnityEngine.Random.InitState(settings.seed);
        System.Random prng = new System.Random(settings.seed);
        Vector2[] octaveOffsets = new Vector2[settings.octaves];
        for(int i = 0; i < settings.octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + settings.offset.x;
            float offsetY = prng.Next(-100000, 100000) + settings.offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        float[,] noiseMap = new float[gridDimentions.x, gridDimentions.y];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for(int x = 0; x < gridDimentions.x; x++)
        {
            for(int y = 0; y < gridDimentions.y; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;


                for(int i = 0; i < settings.octaves; i++)
                {
                    float sampleX = x / settings.noiseScale * frequency + octaveOffsets[i].x;
                    float sampleY = y / settings.noiseScale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= settings.persistance;
                    frequency *= settings.lacunarity;
                }

                if(noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if(noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;
                

                noiseMap[x, y] = noiseHeight;

            }
           
        }

        for (int x = 0; x < gridDimentions.x; x++)
        {
            for (int y = 0; y < gridDimentions.y; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);

            }
        }

                return noiseMap;
    }


    public static float RangeMap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return outMin + (inputValue - inMin) * (outMax - outMin) / (inMax - inMin); 
    }

    public static Texture2D GenerateTexture(GridPosition gridSize, NoiseSettings settings)
    {
        Texture2D texture = new Texture2D(gridSize.x, gridSize.y);
        float[,] noiseMap = GenrateNoiseMap(gridSize, settings);

        for(int x = 0; x < gridSize.x; x++)
        {
            for(int y = 0; y < gridSize.y; y++)
            {
                float noiseValue = noiseMap[x, y];
                Color pixelColour = new Color(noiseValue, noiseValue, noiseValue);
                texture.SetPixel(x, y, pixelColour);
            }
        }
        texture.Apply();
        return texture;
    }

    public static void MakeSeamlessHorizontally(float[,] noiseMap, int stitchWidth)
    {
        int width = noiseMap.GetUpperBound(0) + 1;
        int height = noiseMap.GetUpperBound(1) + 1;

        // iterate on the stitch band (on the left
        // of the noise)
        for (int x = 0; x < stitchWidth; x++)
        {
            // get the transparency value from
            // a linear gradient
            float v = x / (float)stitchWidth;
            for (int y = 0; y < height; y++)
            {
                // compute the "mirrored x position":
                // the far left is copied on the right
                // and the far right on the left
                int o = width - stitchWidth + x;
                // copy the value on the right of the noise
                noiseMap[o, y] = Mathf.Lerp(noiseMap[o, y], noiseMap[stitchWidth - x, y], v);
            }
        }
    }

    public static List<GridPosition> GetLocalHighest(float[,] noiseMap)
    {
        List<GridPosition> maximas = new List<GridPosition>();
        
        for(int x = 0; x < noiseMap.GetLength(0); x++)
        {
            for(int y = 0; y < noiseMap.GetLength(1); y++)
            {
                float value = noiseMap[x, y];
                if (CheckNeighbours(x, y, noiseMap, (neighbourNoise) => neighbourNoise > value))
                {
                    maximas.Add(new GridPosition(x, y));
                }
            }
        }
        return maximas;
    }

    public static List<GridPosition> GetLocalLowest(float[,] noiseMap)
    {
        List<GridPosition> minima = new List<GridPosition>();

        for (int x = 0; x < noiseMap.GetLength(0); x++)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                float value = noiseMap[x, y];
                if(CheckNeighbours(x, y, noiseMap, (neighbourNoise)=> neighbourNoise < value))
                {
                    minima.Add(new GridPosition(x, y)); 
                }

            }
        }
        return minima;
    }



    private static bool CheckNeighbours(int x, int y, float[,] noiseMap, Func<float, bool> failCondition)
    {
        GridSearch gridSearch = new GridSearch();
        List<TileInfo> neighbours = gridSearch.GetNeighboursOfTile(new GridPosition(x, y));

        foreach(TileInfo info in neighbours)
        {
            GridPosition gridPosition = info.m_gridPosition;
            if (failCondition(noiseMap[gridPosition.x, gridPosition.y]))
            {
                return false;
            }
        }
        return true;
    }
}
