//using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class RubiCubeGenerator : MonoBehaviour
{
    public int speed = 50;

    public GameObject rubicCube;

    public Dictionary<Vector3Int, int> rubiCubeIndicesMap = new Dictionary<Vector3Int, int>();

    RubicCubeChangeOrder rubicCubeChangeOrder = null;

    List<GameObject> allSsubCubeList = new List<GameObject>();
    List<int> currentIndexList = new List<int>();
    List<int> nextIndexList = new List<int>();
    List<Vector3> originalPosList = new List<Vector3>();

    bool isMovingDone = true;

    void Start()
    {/*
        GenerateCube("cube000", new Vector3(0, 0, 0));
        GenerateCube("cube111", new Vector3(1, 1, 1));
        GenerateCube("cube222", new Vector3(2, 2, 2));
        GenerateCube("cube333", new Vector3(-1, -1, -1));
        GenerateCube("cube444", new Vector3(-2, -2, -2));
        */
        Generate5x5x5Rubic();
        DumpCurrentCubeIndices();
    }


    void Generate5x5x5Rubic()
    {
        int mid = 2;
        float mul = 1.05f;
        for (int z = 0; z < 5; z++)
        {
            float zp = (z - mid) * mul;
            for (int y = 0; y < 5; y++)
            {
                float yp = (y - mid) * mul;
                for (int x = 0; x < 5; x++)
                {
                    float xp = (x - mid) * mul;
                    string name = $"cube{x + 1}{y + 1}{z + 1}";
                    GenerateCube(name, new Vector3(xp, yp, zp));
                }
            }
        }
        Debug.Log("rubiCube: test");
        if (rubicCube != null)
        {
            Debug.Log("rubiCube: get rubiCubeChangeOrder");
            RubicCubeChangeOrder rubicCubeChangeOrder = rubicCube.GetComponent<RubicCubeChangeOrder>();
            if (rubicCubeChangeOrder != null)
            {
                Debug.Log("rubicCubeChangeOrder Initialize");
                rubicCubeChangeOrder.Initialize(5);
            }
        }

        rubicCubeChangeOrder = this.GetComponent<RubicCubeChangeOrder>();
        //Assert.IsNotNull(rubicCubeChangeOrder);
        rubicCubeChangeOrder.Initialize(5);
    }

    void GenerateCube(string name, Vector3 pos)
    {
        // Create a new cube
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Set the cube's position relative to this GameObject
        cube.transform.position = Vector3.zero;

        // Make the cube a child of this GameObject
        cube.transform.parent = this.transform;

        // Optionally, adjust the cube's local position
        cube.transform.localPosition = pos; // 1 unit above the parent

        // Optionally, adjust other properties like scale or rotation
        cube.transform.localScale = new Vector3(1, 1, 1); // Default scale
        cube.transform.localRotation = Quaternion.identity; // Default rotation

        // Add a component to the cube (optional)
        //cube.AddComponent<Rigidbody>();
        cube.name = name;
        
        AddQuad(cube, "Front", new Vector3(0, 0, -0.51f), new Vector3(0, 0, 0), Color.red);
        AddQuad(cube, "Back", new Vector3(0, 0, 0.51f), new Vector3(0, 180, 0), Color.cyan);
        AddQuad(cube, "Right", new Vector3(0.51f, 0, 0), new Vector3(0, -90, 0), Color.blue);
        AddQuad(cube, "Left", new Vector3(-0.51f, 0, 0), new Vector3(0, 90, 0), Color.green);
        AddQuad(cube, "Up", new Vector3(0, 0.51f, 0), new Vector3(90, 0, 0), Color.white);
        AddQuad(cube, "Down", new Vector3(0, -0.51f, 0), new Vector3(-90, 0, 0), Color.yellow);

        CubeManager cubeManager = GetComponent<CubeManager>();
        int index = 0;
        if (cubeManager != null )
        {
            rubiCubeIndicesMap.Add(new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z), index++);
            cubeManager.AddCube(cube,new Vector3Int ((int)pos.x, (int)pos.y, (int)pos.z));
            allSsubCubeList.Add(cube);
            originalPosList.Add(pos);
            currentIndexList.Add(currentIndexList.Count);
        }
    }

    public void ResetSubCubes()
    {
        Debug.Log("ResetSubCubes");
        for (int i = 0; i < allSsubCubeList.Count; ++i)
        {
            allSsubCubeList[i].transform.localPosition = originalPosList[i];
            allSsubCubeList[i].transform.localRotation = Quaternion.identity;
            currentIndexList[i] = i;
        }
    }

    void AddQuad (GameObject parent, string name, Vector3 pos, Vector3 rotAngle, Color col)
    {
        //Debug.Log($"Quad:{name}-{pos}");
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.parent = parent.transform;
        quad.transform.localPosition = pos;
        quad.transform.localRotation = Quaternion.Euler (rotAngle.x, rotAngle.y, rotAngle.z);
        Renderer renderer = quad.GetComponent<Renderer>();

        // Check that the renderer exists and set the color
        if (renderer != null)
        {
            // Change the cube’s color to red
            renderer.material.color = col;
        }
        quad.name = name;

    }

    public void RotateSubCubes(int directionNum, int rows) // direction -> X, Y, Z, x, y, z /  rows -> 0b00001, 0b10000, 0b00011, 0b11000, 0b11111,
    {
        List<int> rotSubCubeList = new List<int>();
        Vector3 dir = Vector3.zero;
        switch (directionNum)
        {
            case 0:
                dir = Vector3.right;
                break;
            case 1:
                dir = Vector3.up;
                break;
            case 2:
                dir = Vector3.forward;
                break;
            case 3:
                dir = Vector3.left;
                break;
            case 4:
                dir = Vector3.down;
                break;
            case 5:
                dir = Vector3.back;
                break;
        }
        DumpCurrentCubeIndices();
        SelectIndices(dir, rows, rotSubCubeList);
        SetNextIndexList(dir, rows);
        ExecuteRotateCube(dir, rotSubCubeList);
        nextIndexList = currentIndexList;
        DumpCurrentCubeIndices();
        Debug.Log($"RotateSubCubes:{dir}, rows:{rows}");
    }

    void SetNextIndexList (Vector3 dir, int rows)
    {
        nextIndexList = currentIndexList;
        List<Array4<int>> list = new List<Array4<int>>();
        int mask = 1;
        for (int i = 0; i < rubicCubeChangeOrder.level - 1; ++i)
        {
            if ((rows & mask) == 0)
                rubicCubeChangeOrder.GetReplaceIndeces(dir, i, list);
            Debug.Log($"mask:{mask}");
            mask <<= 1;
        }
        foreach(Array4<int> a4 in list)
        {
            Debug.Log($"Change index:{a4}");
            nextIndexList[a4[3]] = currentIndexList[a4[0]];
            nextIndexList[a4[0]] = currentIndexList[a4[1]];
            nextIndexList[a4[1]] = currentIndexList[a4[2]];
            nextIndexList[a4[2]] = currentIndexList[a4[3]];
        }

    }

    IEnumerator RotateCubeFinish()
    {
        yield return new WaitUntil(() => isMovingDone);
    }

    IEnumerator RotateCube(UnityEngine.Vector3 direction, List<int> subCubeList)
    {
        isMovingDone = false;
        float remainingAngle = 90;
        UnityEngine.Vector3 rotationCenter = transform.position;
        UnityEngine.Vector3 rotationAxis = -1f * direction;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            for (int i = 0; i < subCubeList.Count; i++)
            {
                allSsubCubeList[subCubeList[i]].transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            }
            remainingAngle -= rotationAngle;
            yield return null;

        }
        isMovingDone = true;
    }


    void ExecuteRotateCube (UnityEngine.Vector3 dir, List<int> subCubeList)
    {
        Debug.Log($"StartRotateCube {dir}, {subCubeList}");
        StartCoroutine(RotateCube(dir, subCubeList));
        StartCoroutine(RotateCubeFinish());
    }

    void SelectIndices(Vector3 dir, int rows, List<int> subCubeList)
    {
        int setRows = 0;
        int mask = 1;
        for (int i = 0; i < 15; ++i)
        {
            if ((mask & rows) != 0)
            {
                setRows++;
            }
            mask <<= 1;
        }
        if (setRows == rubicCubeChangeOrder.level)
        {
            SelectIndices(-1, -1, -1, subCubeList);
        }
        else
        {
            mask = 1;
            for (int i = 0; i < 15; ++i)
            {
                if ((mask & rows) != 0)
                {
                    if (dir == Vector3.right || dir == Vector3.left)
                        SelectIndices(i, -1, -1, subCubeList);
                    if (dir == Vector3.up || dir == Vector3.down)
                        SelectIndices(-1, i, -1, subCubeList);
                    if (dir == Vector3.forward || dir == Vector3.back)
                        SelectIndices( -1, -1, i, subCubeList);
                }
                mask <<= 1;
            }

        }
    }

    void SelectIndices (int x , int y, int z, List<int> subCubeList)
    {
        if (x < 0 && y < 0 && z < 0)
        {
            for (int i = 0; i < currentIndexList.Count; i++)
                subCubeList.Add(currentIndexList[i]);
        } 
        else
        if (x < 0 && y < 0)
        {
            int level = rubicCubeChangeOrder.level;
            for (int yi = 0; yi < level; yi++)
            {
                for (int xi = 0; xi < level; xi++)
                {
                    subCubeList.Add(rubicCubeChangeOrder.Coord2Index(xi, yi, z));
                }
            }
        }
        else
        if (y < 0 && z < 0)
        {
            int level = rubicCubeChangeOrder.level;
            for (int zi = 0; zi < level; zi++)
            {
                for (int yi = 0; yi < level; yi++)
                {
                    subCubeList.Add(rubicCubeChangeOrder.Coord2Index(x, yi, zi));
                }
            }
        }
        else
        if (z < 0 && x < 0)
        {
            int level = rubicCubeChangeOrder.level;
            for (int zi = 0; zi < level; zi++)
            {
                for (int xi = 0; xi < level; xi++)
                {
                    subCubeList.Add(rubicCubeChangeOrder.Coord2Index(xi, y, zi));
                }
            }
        }
    }

    void DumpCurrentCubeIndices()
    {
        Debug.Log($"CurrentCubeIndices begin <");
        int num = rubicCubeChangeOrder.level;
        for (int z = 0; z < num; z++)
        {
            for (int y = 0; y < num; y++)
            {
                int index = z * num * num + y * num;
                List<int> indices = new List<int>();
                for (int x = 0; x < num; x++ )
                {
                    indices.Add(currentIndexList[index + x]);
                }
                Debug.Log($"z:{z}, y:{y}");
                string logStr = "{";
                foreach (int i in indices)
                {
                    logStr += i + ",";
                }
                logStr += "}";
                Debug.Log(logStr);
            }
        }
        Debug.Log($"CurrentCubeIndices end >");
    }

}
