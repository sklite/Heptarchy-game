using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VoronoiDiagramSc : MonoBehaviour
{
	//private Vector2Int _imageDim = new Vector2Int(Screen.width, Screen.height);
	public Vector2Int _imageDim = new Vector2Int(256, 256);
	public int regionAmount;
	private void Start()
    {
		GetComponent<SpriteRenderer>().sprite = Sprite.Create(GetDiagram(), new Rect(0, 0, _imageDim.x, _imageDim.y), Vector2.one * 0.5f);
		
	}
	Texture2D GetDiagram()
	{
		Vector2Int[] centroids = new Vector2Int[regionAmount];
		Color[] regions = new Color[regionAmount];
		for (int i = 0; i < regionAmount; i++)
		{
			centroids[i] = new Vector2Int(Random.Range(0, _imageDim.x), Random.Range(0, _imageDim.y));
			regions[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
		}
		Color[,] pixelColors = new Color[_imageDim.x, _imageDim.y];
		for (int x = 0; x < _imageDim.x; x++)
		{
			for (int y = 0; y < _imageDim.y; y++)
			{
				//int index = x * _imageDim.x + y;
                //if (pixelColors.Length <= index)
                //    throw new Exception("index was more!");
                var closestCentroid = GetClosestCentroidIndex(new Vector2Int(x, y), centroids);
                //if (regions.Length <= closestCentroid)
                //    throw new Exception("closestCentroid was more!");

				pixelColors[x,y] = regions[closestCentroid];
			}
		}
		return GetImageFromColorArray(pixelColors);
	}
	//Texture2D GetDiagramByDistance()
	//{
	//	Vector2Int[] centroids = new Vector2Int[regionAmount];

	//	for (int i = 0; i < regionAmount; i++)
	//	{
	//		centroids[i] = new Vector2Int(Random.Range(0, _imageDim.x), Random.Range(0, _imageDim.y));
	//	}
	//	Color[] pixelColors = new Color[_imageDim.x * _imageDim.y];
	//	float[] distances = new float[_imageDim.x * _imageDim.y];

	//	//you can get the max distance in the same pass as you calculate the distances. :P oops!
	//	float maxDst = float.MinValue;
	//	for (int x = 0; x < _imageDim.x; x++)
	//	{
	//		for (int y = 0; y < _imageDim.y; y++)
	//		{
	//			int index = x * _imageDim.x + y;
	//			distances[index] = Vector2.Distance(new Vector2Int(x, y), centroids[GetClosestCentroidIndex(new Vector2Int(x, y), centroids)]);
	//			if (distances[index] > maxDst)
	//			{
	//				maxDst = distances[index];
	//			}
	//		}
	//	}

	//	for (int i = 0; i < distances.Length; i++)
	//	{
	//		float colorValue = distances[i] / maxDst;
	//		pixelColors[i] = new Color(colorValue, colorValue, colorValue, 1f);
	//	}
	//	return GetImageFromColorArray(To1DArray(pixelColors));
	//}
	/* didn't actually need this
	float GetMaxDistance(float[] distances)
	{
		float maxDst = float.MinValue;
		for(int i = 0; i < distances.Length; i++)
		{
			if(distances[i] > maxDst)
			{
				maxDst = distances[i];
			}
		}
		return maxDst;
	}*/
	int GetClosestCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
	{
		float smallestDst = float.MaxValue;
		int index = 0;
		for (int i = 0; i < centroids.Length; i++)
		{
			if (Vector2.Distance(pixelPos, centroids[i]) < smallestDst)
			{
				smallestDst = Vector2.Distance(pixelPos, centroids[i]);
				index = i;
			}
		}
		return index;
	}
	Texture2D GetImageFromColorArray(Color[,] pixelColors)
	{
		Texture2D tex = new Texture2D(_imageDim.x, _imageDim.y);
		tex.filterMode = FilterMode.Point;
		tex.SetPixels(To1DArray(pixelColors));
		//tex.set
		tex.Apply();
		return tex;
	}

    static Color[] To1DArray(Color[,] input)
    {
        // Step 1: get total size of 2D array, and allocate 1D array.
        int size = input.Length;
        Color[] result = new Color[size];

        // Step 2: copy 2D array elements into a 1D array.
        int write = 0;
        for (int i = 0; i <= input.GetUpperBound(0); i++)
        {
            for (int z = 0; z <= input.GetUpperBound(1); z++)
            {
                result[write++] = input[i, z];
            }
        }
        // Step 3: return the new array.
        return result;
    }

}
