using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RubiCubeGenerator : MonoBehaviour
{
    public int speed = 50;

    public GameObject rubicCube;

    public Dictionary<Vector3Int, int> rubiCubeIndicesMap = new Dictionary<Vector3Int, int>();

    RubicCubeChangeOrder rubicCubeChangeOrder = null;

    List<GameObject> cubeList = new List<GameObject>();
    List<int> currentIndexList = new List<int>();
    List<int> nextIndexList = new List<int>();
    List<Vector3> originalPosList = new List<Vector3>();

    bool isMovingDone = true;

    void Start()
    {
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
                    string name = $"cube{z + 1}{y + 1}{x + 1}-{z * 25 + y * 5 + x}";
                    GenerateCube(name, new Vector3(xp, yp, zp));
                }
            }
        }
        JustCheck();
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

        rubicCubeChangeOrder.Initialize(5);
    }

    void JustCheck()
    {
        Vector3 dir = Vector3.right;
        Debug.Log($"Vector3.right: {dir.x}, {dir.y}, {dir.z}");
        dir = Vector3.up;
        Debug.Log($"Vector3.up: {dir.x}, {dir.y}, {dir.z}");
        dir = Vector3.forward;
        Debug.Log($"Vector3.forward: {dir.x}, {dir.y}, {dir.z}");
    }

    void GenerateCube(string name, Vector3 pos)
    {
        // Create a new cube
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Set the cube's position relative to this GameObject
        cube.transform.position = Vector3.zero;

        // Make the cube a child of this GameObjectdir = Vector3.right;
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
            cubeList.Add(cube);
            originalPosList.Add(pos);
            currentIndexList.Add(currentIndexList.Count);
        }
    }

    public void ResetSubCubes()
    {
        Debug.Log("ResetSubCubes");
        for (int i = 0; i < cubeList.Count; ++i)
        {
            cubeList[i].transform.localPosition = originalPosList[i];
            cubeList[i].transform.localRotation = Quaternion.identity;
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

        AddText(parent.name, quad);

    }

    void AddText(string nameParent, GameObject parent)
    {
        string name = nameParent.Substring(4, nameParent.Length - 4);
        GameObject textGO = new GameObject(name + "-text");
        textGO.transform.SetParent(parent.transform);

        TextMeshPro tmp = textGO.AddComponent<TextMeshPro>();

        tmp.text = name.ToString();
        tmp.fontSize = 30.0f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.black;

        textGO.transform.localScale = Vector3.one * 0.1f;

        float delta = -0.05f;
        // Tip of cone
        textGO.transform.localPosition =
            new Vector3(
                0,
                0,
                delta);

        // Face camera if desired
        textGO.transform.localRotation = Quaternion.identity;
    }

    public void RotateSubCubes(int directionNum, int rows) // direction -> X, Y, Z, x, y, z /  rows -> 0b00001, 0b10000, 0b00011, 0b11000, 0b11111,
    {
        if(!isMovingDone)
        {
            Debug.Log("RotateSubCubes: isMovingDone false");
            return;
        }
        isMovingDone = false;

        RefreshIndexList();

        List<int> rotSubCubeList = new List<int>();
        Vector3 dir = Vector3.zero;
        switch (directionNum)
        {
            case 0:
                dir = Vector3.right;
                break;
            case 4:
                dir = Vector3.back;
                break;
            case 2:
                dir = Vector3.up;
                break;
            case 3:
                dir = Vector3.left;
                break;
            case 1:
                dir = Vector3.forward;
                break;
            case 5:
                dir = Vector3.down;
                break;
        }
        DumpCurrentCubeIndices();
        SelectIndices(directionNum, rows, rotSubCubeList);
        //SetNextIndexList(dir, rows);
        ExecuteRotateCube(dir, rotSubCubeList);
        //nextIndexList = currentIndexList;
        //DumpCurrentCubeIndices();
        Debug.Log($"RotateSubCubes:{dir}, rows:{rows}");
    }


    static int Vector3ToInt(Vector3 vec)
    {
        const float Eps = 0.001f;
        return (int)(vec.x*10 + (vec.y * 10) * 1000 + (vec.z * 10) * 1000000);
    }

    void RefreshIndexList ()
    {
        List<int> refreshIndexList = new List<int>();
        SortedDictionary<int, int> sortedPairList = new SortedDictionary<int, int>();

        for (int i = 0; i < cubeList.Count; i++)
        {
            Vector3 pos = cubeList[i].transform.localPosition;
            int key = Vector3ToInt(pos);
            if (sortedPairList.ContainsKey(key))
            {
                Debug.LogWarning($"!!! RefreshIndexList: duplicate key:{key}, i:{i},n:{cubeList.Count}");
                for (int k = 0; k < cubeList.Count; k++)
                {
                    Vector3 posTmp = cubeList[k].transform.localPosition;
                    int keyTmp = Vector3ToInt(posTmp);
                    Debug.Log($"{k}.{posTmp}-{keyTmp}");
                }
            }
            sortedPairList.Add(key, i);
        }
        int j = 0;
        Debug.Log($"RefreshIndexList:{sortedPairList.Count}\n");
        foreach (var pair in sortedPairList)
        {
            refreshIndexList.Add(pair.Value);
            Debug.Log($"{j}.{pair.Key}-{pair.Value}&");
            j++; 
        }
        Debug.Log("RefreshIndexList end\n");

        currentIndexList = refreshIndexList;
    }

    void SetNextIndexList (Vector3 dir, int rows)
    {
        nextIndexList = currentIndexList;
        List<Array4<int>> list = new List<Array4<int>>();
        int mask = 1;
        for (int i = 0; i < rubicCubeChangeOrder.level - 1; ++i)
        {
            if ((rows & mask) != 0) { 
                rubicCubeChangeOrder.GetReplaceIndeces(dir, i, list);
                Debug.Log($"GetReplaceIndeces dir:{dir}, i:{i}\n");
                foreach (Array4<int> a4 in list)
                    Debug.Log($"Change index:{a4}");
            }
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
                cubeList[subCubeList[i]].transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
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

    void SelectIndices(int directionNum, int rows, List<int> subCubeList)// directionNum -> X, Y, Z, x, y, z /  rows -> 0b00001, 0b10000, 0b00011, 0b11000, 0b11111,
    {

        float level = 0f;
        Vector3 dir = Vector3.zero;
        switch (directionNum)
        {
            case 0:
            case 3:
                dir = Vector3.right;
                break;
            case 2:
            case 5:
                dir = Vector3.up;
                break;
            case 1:
            case 4:
                dir = Vector3.forward;
                break;
        }
        Vector3 posMin = cubeList[0].transform.localPosition;
        Vector3 posMax = cubeList[cubeList.Count - 1].transform.localPosition;
        int Level = rubicCubeChangeOrder.level;
        float mul = (posMax.x - posMin.x) / (Level - 1);
        //Vector3 distance = (dir - posMin) / (Level - 1);
        int step = 0;
        for (int mask = 1; mask < 0b11111; mask <<= 1, step++)
        {
            if ((mask & rows) == 0)
                continue;
            Vector3 pos = posMin + dir * step * mul;
            Debug.Log($"!!!level: step:{step}, pos:{pos}, min:{posMin}, max:{posMax}, dir:{dir}, rows:{rows}");
            List<int> subCubeIndices = new List<int>();
            for(int i = 0; i < cubeList.Count; i++)
            {
                GameObject cube = cubeList[i];
                Vector3 cubePos = cube.transform.localPosition;
                float dotProduct = Vector3.Dot(cubePos - pos, dir);
                if (Mathf.Abs(dotProduct) < 0.01f)
                {
                    subCubeIndices.Add(i);
                }
            }
            if (subCubeIndices.Count > 0)
            {
                if (subCubeIndices.Count == Level * Level)
                    subCubeList.AddRange(subCubeIndices);
                else
                    Debug.LogWarning($"SelectIndices: subCubeIndices.Count != Level * Level, count:{subCubeIndices.Count}, Level:{Level}");
            }
        }
    }

    void SelectIndicesOld(Vector3 dir, int rows, List<int> subCubeList)
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
