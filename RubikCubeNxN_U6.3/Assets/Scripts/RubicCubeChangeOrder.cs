using System;
using System.Collections.Generic;
using UnityEngine;

public struct Array4<T>
{
    public T _item0 { get; private set; }
    public T _item1 { get; private set; }
    public T _item2 { get; private set; }
    public T _item3 { get; private set; }


    public Array4(T i1, T i2, T i3, T i4)
    {
        _item0 = i1;
        _item1 = i2;
        _item2 = i3;
        _item3 = i4;
    }
    public T this[int index]
    {
        get => index switch
        {
            0 => _item0,
            1 => _item1,
            2 => _item2,
            3 => _item3,
            _ => throw new IndexOutOfRangeException("Index must be between 0 and 3.")
        };
        set
        {
            switch (index)
            {
                case 0: _item0 = value; break;
                case 1: _item1 = value; break;
                case 2: _item2 = value; break;
                case 3: _item3 = value; break;
                default: throw new IndexOutOfRangeException("Index must be between 0 and 3.");
            }
        }
    }
    public override string ToString()
    {
        return $"[{_item0}->{_item1}->{_item2}->{_item3}]";
    }
}

public class RubicCubeChangeOrder : MonoBehaviour
{
    public int level { get; private set; }

    public Dictionary<Vector2Int, List<Array4<Vector3Int>>>  replaceOrder = new Dictionary<Vector2Int, List<Array4<Vector3Int>>>();

    public bool Initialize (int levelIn)
    {
        Debug.Log($"RubiCubeChangeOrder.Initialize: in-{levelIn}");
        level = 0;
        replaceOrder.Clear ();
        switch (levelIn)
        {
            case 3:
            case 5:
                level = levelIn;
                break;
        }
        Debug.Log($"RubiCubeChangeOrder.Initialize:{level}");
        if (level == 0)
            return false;
        for (int dir = 0; dir < 3; dir++)
        {
            for (int lev = 0; lev < level - 1; lev++)
            {
                GenerateChangeOrder(dir, lev);
            }
        }
        DumpReplaceOrder();
        return true;
    }

    public void GenerateChangeOrder(int dir, int lev)
    {
        Vector2Int key = new Vector2Int(dir, level);
        List<Array4<Vector3Int>> replaceOrder1 = new List<Array4<Vector3Int>>();
        for (int coord1  = 0; coord1 < level - 1; coord1++)
        {
            Array4<Vector3Int> arra4 =  GenerateArray4(dir, lev, coord1);
            replaceOrder1.Add(arra4);
        }
        replaceOrder.Add(new Vector2Int(dir, lev), replaceOrder1);
    }

    public int Coord2Index(int x, int y, int z)
    {
        int ret = x + y * level + z * level * level;
        Debug.Log($"Coord2Index:{x},{y},{z}->{ret}");
        return ret;
    }

    Array4<Vector3Int> GenerateArray4(int dir, int lev, int coord1)
    {
        Array4<Vector3Int> ret = new Array4<Vector3Int>();
        Vector3Int v1;
        Vector3Int v2;
        Vector3Int v3;
        Vector3Int v4;

        switch (dir)
        {
            case 0:
                v1 = new Vector3Int(lev, coord1, 0);
                v2 = new Vector3Int(lev, level - 1, coord1);
                v3 = new Vector3Int(lev, level - 1 - coord1, level - 1);
                v4 = new Vector3Int(lev, 0, level - 1 - coord1);
                ret = new Array4<Vector3Int>(v1,v2,v3,v4);
                break;
            case 1:
                v1 = new Vector3Int(0, lev, coord1);
                v2 = new Vector3Int(coord1, lev, level - 1);
                v3 = new Vector3Int(level - 1, lev, level - 1 - coord1);
                v4 = new Vector3Int(level - 1 - coord1, lev, 0);
                ret = new Array4<Vector3Int>(v1, v2, v3, v4);
                break;
            case 2:
                v1 = new Vector3Int(coord1, 0, lev );
                v2 = new Vector3Int(level - 1, coord1, lev);
                v3 = new Vector3Int(level - 1 - coord1, level - 1, lev);
                v4 = new Vector3Int(0, level - 1 - coord1, lev);
                ret = new Array4<Vector3Int>(v1, v2, v3, v4);
                break;

        }
        return ret;
    }

    void DumpReplaceOrder ()
    {
        foreach (var v1  in replaceOrder)
        {
            Debug.Log($"Key:{v1.Key}:");
            foreach (var v2 in v1.Value)
            {
                Debug.Log($"v4:{v2.ToString()}:");
            }
        }
    }


    public void GetReplaceIndeces (Vector3 dir, int row, List<Array4<int>> list)
    {
        int dirNum = 0;
        bool inv = false;
        if (dir == Vector3.left || dir == Vector3.right)
        {
            if (dir == Vector3.left)
                inv = true;
            dirNum = 0;
        }
        if (dir == Vector3.up || dir == Vector3.down)
        {
            if (dir == Vector3.down)
                inv = true;
            dirNum = 1;
        }
        if (dir == Vector3.forward || dir == Vector3.back)
        {
            if (dir == Vector3.forward)
                inv = true;
            dirNum = 0;
        }
        Vector2Int key = new Vector2Int(dirNum, row);
        Debug.Log($"Key:{key}");
        if (replaceOrder.ContainsKey(key))
        {
            List<Array4<Vector3Int>> value = replaceOrder[key];
            foreach (Array4<Vector3Int> v in value)
            {
                int i0 =Coord2Index(v._item0.x, v._item0.y, v._item0.z);
                int i1 =Coord2Index(v._item1.x, v._item1.y, v._item1.z);
                int i2 =Coord2Index(v._item2.x, v._item2.y, v._item2.z);
                int i3 =Coord2Index(v._item3.x, v._item3.y, v._item3.z);
                if (inv)
                {
                    Array4<int> array4 = new Array4<int>(i3,i2,i1,i0);
                    list.Add(array4);
                } 
                else
                {
                    Array4<int> array4 = new Array4<int>(i0,i1,i2,i3);
                    list.Add(array4);
                }
            }
        }  
    }
}
