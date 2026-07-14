//using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public int maxSubIndex = 0;
    List<GameObject> cubeList = new List<GameObject>();
    List<int> cubeIndices = new List<int>();
    Dictionary<int, int> mapCubeIndices = new Dictionary<int, int>();
    Dictionary<int, List<Vector3Int>>replaceMap = new Dictionary<int, List<Vector3Int>>();

    public void AddCube(GameObject cube, Vector3Int pos)
    {
        int index = cubeList.Count;
        cubeIndices.Add(index);
        mapCubeIndices.Add(Vector3IntToInt(pos), index);
        cubeList.Add(cube);

        if (pos.x + 1 > maxSubIndex)
            maxSubIndex = pos.x + 1;
    }
    public void GenerateRotationReplacement()
    {
        for (int axis = 0; axis < 3; axis++)
        {
            for(int level = 0; level < maxSubIndex; level++)
            {
                int key = KeyFromAxisLevel(axis, level);
                List<Vector3Int> vector3Ints = new List<Vector3Int>();
            }
        }
    }

    int KeyFromAxisLevel (int axis, int level) { 
        return level * 10 + axis; 
    }

    int Vector3IntToInt(Vector3Int vec)
    {
        return (int)(vec.x + vec.y*100 + vec.z*10000);
    }
}
