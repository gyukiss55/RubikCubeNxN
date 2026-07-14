using System;
using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;


public class RubicCubeControl : MonoBehaviour
{

    public int speed = 50;

    UnityEngine.Vector3[] newPositions = null;
    bool isMoving = false;
    GameObject[] rubicSubCubes = null;
    int[] indicesAll = null;
    int[] indicesRotate = null;
    UnityEngine.Vector3[] originalPosition = null;
    UnityEngine.Quaternion[] originalRotation = null;

    char[] macro1 = { 'u', 'l', 'U', 'L', 'U', 'F', 'u', 'f' };
    char[] macro2 = { 'U', 'R', 'u', 'r', 'u', 'f', 'U', 'F' };
    char[] macro3 = { 'F', 'R', 'U', 'r', 'u', 'f' };
    char[] macro4 = { 'R', 'U', 'r', 'U', 'R', 'U', 'U', 'r', 'U' };
    char[] macro5 = { 'U', 'R', 'u', 'l', 'U', 'r', 'u', 'L' };
    char[] macro6 = { 'r', 'd', 'R', 'D' };
    char[] macro11 = { 'L', 'F', 'r', 'f', 'R', 'l', 'U', 'R', 'u', 'r' };
    char[] macro12 = { 'R', 'U', 'r', 'u', 'r', 'L', 'F', 'R', 'f', 'l' };
    /*
        Vector3 rPos = new Vector3(1.51f, 0, 0);
        Vector3 rPos = new Vector3(0, 1.51f, 0);
        Vector3 rPos = new Vector3(1.51f, 0, 0);
        Vector3 rPos = new Vector3(1.51f, 0, 0);
        Vector3 rPos = new Vector3(1.51f, 0, 0);
        Vector3 rPos = new Vector3(1.51f, 0, 0);
        Vector3 rPos = new Vector3(1.51f, 0, 0);
    */

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Start {this.name}");
        originalPosition = new UnityEngine.Vector3[27];
        originalRotation = new UnityEngine.Quaternion[27];
        rubicSubCubes = new GameObject[27];
        indicesAll = new int[27];
        indicesRotate = new int[9];
        newPositions = new UnityEngine.Vector3[27];
        newPositions[0] = new UnityEngine.Vector3(-1, -1, -1);
        newPositions[1] = new UnityEngine.Vector3(0, -1, -1);
        newPositions[2] = new UnityEngine.Vector3(1, -1, -1);

        newPositions[3] = new UnityEngine.Vector3(-1, 0, -1);
        newPositions[4] = new UnityEngine.Vector3(0, 0, -1);
        newPositions[5] = new UnityEngine.Vector3(1, 0, -1);

        newPositions[6] = new UnityEngine.Vector3(-1, 1, -1);
        newPositions[7] = new UnityEngine.Vector3(0, 1, -1);
        newPositions[8] = new UnityEngine.Vector3(1, 1, -1);

        newPositions[9] = new UnityEngine.Vector3(-1, -1, 0);
        newPositions[10] = new UnityEngine.Vector3(0, -1, 0);
        newPositions[11] = new UnityEngine.Vector3(1, -1, 0);

        newPositions[12] = new UnityEngine.Vector3(-1, 0, 0);
        newPositions[13] = new UnityEngine.Vector3(0, 0, 0);
        newPositions[14] = new UnityEngine.Vector3(1, 0, 0);

        newPositions[15] = new UnityEngine.Vector3(-1, 1, 0);
        newPositions[16] = new UnityEngine.Vector3(0, 1, 0);
        newPositions[17] = new UnityEngine.Vector3(1, 1, 0);

        newPositions[18] = new UnityEngine.Vector3(-1, -1, 1);
        newPositions[19] = new UnityEngine.Vector3(0, -1, 1);
        newPositions[20] = new UnityEngine.Vector3(1, -1, 1);

        newPositions[21] = new UnityEngine.Vector3(-1, 0, 1);
        newPositions[22] = new UnityEngine.Vector3(0, 0, 1);
        newPositions[23] = new UnityEngine.Vector3(1, 0, 1);

        newPositions[24] = new UnityEngine.Vector3(-1, 1, 1);
        newPositions[25] = new UnityEngine.Vector3(0, 1, 1);
        newPositions[26] = new UnityEngine.Vector3(1, 1, 1);

        int i = 0;
        foreach (Transform child in transform)
        {
            Debug.Log(child.gameObject.name);
            if (i < 27)
            {
                child.gameObject.transform.position = newPositions[i];
                Debug.Log($"Set position{child.gameObject.name}");
                rubicSubCubes[i] = child.gameObject;
                indicesAll[i] = i;
                //transforms[i] = new Transform;
                originalPosition[i] = rubicSubCubes[i].transform.position;
                originalRotation[i] = rubicSubCubes[i].transform.rotation;
                i++;
            }
        }
    }


    void Update()
    {
        if (isMoving)
        {
            return;
        }
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;


        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
        bool isCtrlPressed = Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.rightCtrlKey.isPressed;
        //bool isAltPressed = Keyboard.current.leftAltKey.isPressed || Keyboard.current.rightAltKey.isPressed;
        if (isCtrlPressed)
            return;
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            Debug.Log($"Cube Click RightArrow");
            StartCoroutine(Rotate9('R', isShiftPressed));
        }
        else
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            Debug.Log($"Cube Click LeftArrow");
            StartCoroutine(Rotate9('L', isShiftPressed));
        }
        else
        if (Keyboard.current.upArrowKey.isPressed)
        {
            Debug.Log($"Cube Click UpArrow");
            StartCoroutine(Rotate9('U', isShiftPressed));
        }
        else
        if (Keyboard.current.downArrowKey.isPressed)
        {
            Debug.Log($"Cube Click DownArrow");
            StartCoroutine(Rotate9('D', isShiftPressed));
        }
        else
        if (Keyboard.current.homeKey.isPressed)
        {
            Debug.Log($"Cube Click Home");
            StartCoroutine(Rotate9('F', isShiftPressed));
        }
        else
        if (Keyboard.current.endKey.isPressed)
        {
            Debug.Log($"Cube Click End");
            StartCoroutine(Rotate9('B', isShiftPressed));
        }
        else
        if (Keyboard.current.lKey.isPressed)
        {
            Debug.Log($"Cube Click L");
            StartCoroutine(Rotate9('L', isShiftPressed));
        }
        else
        if (Keyboard.current.rKey.isPressed)
        {
            Debug.Log($"Cube Click R");
            StartCoroutine(Rotate9('R', isShiftPressed));
        }
        else
        if (Keyboard.current.uKey.isPressed)
        {
            Debug.Log($"Cube Click U");
            StartCoroutine(Rotate9('U', isShiftPressed));
        }
        else
        if (Keyboard.current.dKey.isPressed)
        {
            Debug.Log($"Cube Click D");
            StartCoroutine(Rotate9('D', isShiftPressed));
        }
        else
        if (Keyboard.current.fKey.isPressed)
        {
            Debug.Log($"Cube Click F");
            StartCoroutine(Rotate9('F', isShiftPressed));
        }
        else
        if (Keyboard.current.bKey.isPressed)
        {
            Debug.Log($"Cube Click B");
            StartCoroutine(Rotate9('B', isShiftPressed));
        }
        else
        if (Keyboard.current.f1Key.isPressed)
        {
            Debug.Log($"Cube Click F1");
            StartCoroutine(PlayMacro(macro1, isShiftPressed));
        }
        else
        if (Keyboard.current.f2Key.isPressed)
        {
            Debug.Log($"Cube Click F2");
            StartCoroutine(PlayMacro(macro2, isShiftPressed));
        }
        else
        if (Keyboard.current.f3Key.isPressed)
        {
            Debug.Log($"Cube Click F3");
            StartCoroutine(PlayMacro(macro3, isShiftPressed));
        }
        else
        if (Keyboard.current.f4Key.isPressed)
        {
            Debug.Log($"Cube Click F4");
            StartCoroutine(PlayMacro(macro4, isShiftPressed));
        }
        else
        if (Keyboard.current.f5Key.isPressed)
        {
            Debug.Log($"Cube Click F5");
            StartCoroutine(PlayMacro(macro5, isShiftPressed));
        }
        else
        if (Keyboard.current.f6Key.isPressed)
        {
            Debug.Log($"Cube Click F6");
            StartCoroutine(PlayMacro(macro6, isShiftPressed));
        }
        else
        if (Keyboard.current.f11Key.isPressed)
        {
            Debug.Log($"Cube Click F11");
            StartCoroutine(PlayMacro(macro11, isShiftPressed));
        }
        else
        if (Keyboard.current.f12Key.isPressed)
        {
            Debug.Log($"Cube Click F12");
            StartCoroutine(PlayMacro(macro12, isShiftPressed));
        }
        else
        if (Keyboard.current.deleteKey.isPressed) // reset
        {
            ResetRubicCube();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        { // Left mouse button
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("You clicked on: " + hit.transform.gameObject.name);
                // You can store the clicked GameObject if needed
                GameObject clickedObject = hit.transform.gameObject;
                UnityEngine.Vector3 pos = clickedObject.transform.position;
                UnityEngine.Quaternion rot = hit.transform.rotation;
                Debug.Log($"Click on {hit.transform.gameObject.name} pos:{pos.x}; {pos.y}; {pos.z} rot:{rot.w}; {rot.x}; {rot.y}; {rot.z}");
            }
        }

    }

    public void ResetRubicCube()
    {
        Debug.Log($"Cube Click Reset");
        int i = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.transform.position = originalPosition[i];
            child.gameObject.transform.rotation = originalRotation[i];
            indicesAll[i] = i;
            i++;
        }
    }

    public void RandomRubicCube()
    {
        Debug.Log($"Cube Click Random");
        int speedTmp = speed;
        char[] choice = { 'F', 'f', 'B', 'b', 'L', 'l', 'R', 'r', 'U', 'u', 'D', 'd', 'X', 'x', 'Y', 'y' };
        speed = 1;
        DateTime t = DateTime.Now;
        const int num = 50;
        char[] macro = new char[num];
        for (int i = 0; i < 25; i++)
        {
            int index = UnityEngine.Random.Range(0, t.Millisecond);
            index = index % choice.Length;
            macro[i] = choice[index];
        }

        StartCoroutine(PlayMacro(macro, false));

        speed = speedTmp;
    }



    public void SpeedMultiply(float mul)
    {
        float speedf = speed;
        speedf *= mul;
        if (speedf < 1)
            speed = 1;
        else speed = (int)speedf;
    }

    public void RotateCube(bool axisY, bool negative)
    {
        if (axisY)
            StartCoroutine(Rotate9('Y', negative));
        else
            StartCoroutine(Rotate9('X', negative));
    }

    public void PlayMacro(int index, bool isShiftPressed)
    {
        switch (index)
        {
            case 1:
                StartCoroutine(PlayMacro(macro1, isShiftPressed));
                break;
            case 2:
                StartCoroutine(PlayMacro(macro2, isShiftPressed));
                break;
            case 3:
                StartCoroutine(PlayMacro(macro3, isShiftPressed));
                break;
            case 4:
                StartCoroutine(PlayMacro(macro4, isShiftPressed));
                break;
            case 5:
                StartCoroutine(PlayMacro(macro5, isShiftPressed));
                break;
            case 6:
                StartCoroutine(PlayMacro(macro6, isShiftPressed));
                break;
            case 7:
                //StartCoroutine(PlayMacro(macro7, isShiftPressed));
                break;
            case 8:
                //StartCoroutine(PlayMacro(macro8, isShiftPressed));
                break;
            case 11:
                StartCoroutine(PlayMacro(macro11, isShiftPressed));
                break;
            case 12:
                StartCoroutine(PlayMacro(macro12, isShiftPressed));
                break;

        }
    }

    IEnumerator Roll(UnityEngine.Vector3 direction)
    {

        isMoving = true;
        float remainingAngle = 90;
        UnityEngine.Vector3 rotationCenter = transform.position + direction * 3 / 2 + UnityEngine.Vector3.down * 3 / 2;
        rotationCenter = transform.position;
        UnityEngine.Vector3 rotationAxis = UnityEngine.Vector3.Cross(UnityEngine.Vector3.up, direction);

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;

        }
        isMoving = false;
    }

    void UpdateIndices(int[] order, bool isShiftPressed)
    {
        int tmp = indicesAll[order[0]];
        if (isShiftPressed)
        {
            indicesAll[order[0]] = indicesAll[order[3]];
            indicesAll[order[3]] = indicesAll[order[2]];
            indicesAll[order[2]] = indicesAll[order[1]];
            indicesAll[order[1]] = tmp;

        }
        else
        {
            indicesAll[order[0]] = indicesAll[order[1]];
            indicesAll[order[1]] = indicesAll[order[2]];
            indicesAll[order[2]] = indicesAll[order[3]];
            indicesAll[order[3]] = tmp;
        }
    }

    void DumpIndicesAll()
    {
        Debug.Log($"Indices:");

        int j = 0;
        for (int i = 0; i < 9; ++i, j += 3)
        {
            Debug.Log($" {j},{j + 1},{j + 2}={indicesAll[j]},{indicesAll[j + 1]},{indicesAll[j + 2]}");
            if (i % 3 == 2)
                Debug.Log("");
        }
    }

    void UpdateIndices(char ch, bool isShiftPressed)
    {
        switch (ch)
        {
            case 'X':
                {
                    int[] changeOrder1 = { 0, 18, 24, 6 };
                    int[] changeOrder2 = { 3, 9, 21, 15 };
                    int[] changeOrder3 = { 1, 19, 25, 7 };
                    int[] changeOrder4 = { 4, 10, 22, 16 };
                    int[] changeOrder5 = { 2, 20, 26, 8 };
                    int[] changeOrder6 = { 5, 11, 23, 17 };
                    UpdateIndices(changeOrder1, isShiftPressed);
                    UpdateIndices(changeOrder2, isShiftPressed);
                    UpdateIndices(changeOrder3, isShiftPressed);
                    UpdateIndices(changeOrder4, isShiftPressed);
                    UpdateIndices(changeOrder5, isShiftPressed);
                    UpdateIndices(changeOrder6, isShiftPressed);
                }
                break;
            case 'Y':
                {
                    int[] changeOrder1 = { 6, 8, 26, 24 };
                    int[] changeOrder2 = { 7, 17, 25, 15 };
                    int[] changeOrder3 = { 3, 5, 23, 21 };
                    int[] changeOrder4 = { 4, 14, 22, 12 };
                    int[] changeOrder5 = { 0, 2, 20, 18 };
                    int[] changeOrder6 = { 1, 11, 19, 9 };
                    UpdateIndices(changeOrder1, isShiftPressed);
                    UpdateIndices(changeOrder2, isShiftPressed);
                    UpdateIndices(changeOrder3, isShiftPressed);
                    UpdateIndices(changeOrder4, isShiftPressed);
                    UpdateIndices(changeOrder5, isShiftPressed);
                    UpdateIndices(changeOrder6, isShiftPressed);
                }
                break;
            case 'L':
                {
                    int[] changeOrder1 = { 6, 24, 18, 0 };
                    int[] changeOrder2 = { 3, 15, 21, 9 };
                    UpdateIndices(changeOrder1, isShiftPressed);
                    UpdateIndices(changeOrder2, isShiftPressed);
                }
                break;
            case 'R':
                {
                    int[] changeOrder1 = { 2, 20, 26, 8 };
                    int[] changeOrder2 = { 5, 11, 23, 17 };
                    UpdateIndices(changeOrder1, isShiftPressed);
                    UpdateIndices(changeOrder2, isShiftPressed);
                }
                break;
            case 'U':
                {
                    int[] changeOrder1 = { 6, 8, 26, 24 };
                    int[] changeOrder2 = { 7, 17, 25, 15 };
                    UpdateIndices(changeOrder1, isShiftPressed);
                    UpdateIndices(changeOrder2, isShiftPressed);
                }
                break;
            case 'D':
                {
                    int[] changeOrder1 = { 0, 18, 20, 2 };
                    int[] changeOrder2 = { 1, 9, 19, 11 };
                    UpdateIndices(changeOrder1, isShiftPressed);
                    UpdateIndices(changeOrder2, isShiftPressed);
                }
                break;
            case 'F':
                {
                    int[] changeOrder1 = { 0, 2, 8, 6 };
                    int[] changeOrder2 = { 1, 5, 7, 3 };
                    UpdateIndices(changeOrder1, isShiftPressed);
                    UpdateIndices(changeOrder2, isShiftPressed);
                }
                break;
            case 'B':
                {
                    int[] changeOrder1 = { 20, 18, 24, 26 };
                    int[] changeOrder2 = { 23, 19, 21, 25 };
                    UpdateIndices(changeOrder1, isShiftPressed);
                    UpdateIndices(changeOrder2, isShiftPressed);
                }
                break;
        }
        DumpIndicesAll();
    }

    public void RotateRubicCube(char ch, bool isInvert)
    {
        StartCoroutine(Rotate9(ch, isInvert));
    }


    IEnumerator Rotate9(char ch, bool isShiftPressed)
    {
        int[] leftIndices = { 0, 3, 6, 9, 12, 15, 18, 21, 24 };
        int[] rightIndices = { 2, 5, 8, 11, 14, 17, 20, 23, 26 };
        int[] frontIndices = { 18, 19, 20, 21, 22, 23, 24, 25, 26 };
        int[] backIndices = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        int[] upIndices = { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
        int[] downIndices = { 6, 7, 8, 15, 16, 17, 24, 25, 26 };
        bool allIndices = false;

        UnityEngine.Vector3 direction = UnityEngine.Vector3.right;
        UnityEngine.Vector3 rotationCenter = transform.position + new UnityEngine.Vector3(-.5f, -.5f, -.5f);

        bool doRotate = true;
        switch (ch)
        {
            case 'X':
                direction = UnityEngine.Vector3.right;
                allIndices = true;
                break;
            case 'Y':
                direction = UnityEngine.Vector3.up;
                allIndices = true;
                break;
            case 'L':
                direction = UnityEngine.Vector3.left;
                indicesRotate = leftIndices;
                break;
            case 'R':
                direction = UnityEngine.Vector3.right;
                indicesRotate = rightIndices;
                rotationCenter = transform.position + new UnityEngine.Vector3(0, -1, 0);
                break;
            case 'D':
                direction = UnityEngine.Vector3.down;
                indicesRotate = upIndices;
                rotationCenter = transform.position + new UnityEngine.Vector3(0, -1, 0);
                break;
            case 'U':
                direction = UnityEngine.Vector3.up;
                indicesRotate = downIndices;
                rotationCenter = transform.position + new UnityEngine.Vector3(0, -1, 0);
                break;
            case 'B': // back
                direction = UnityEngine.Vector3.forward;
                indicesRotate = frontIndices;
                rotationCenter = transform.position + new UnityEngine.Vector3(0, -1, 0);
                break;
            case 'F':
                direction = UnityEngine.Vector3.back;
                indicesRotate = backIndices;
                rotationCenter = transform.position + new UnityEngine.Vector3(0, -1, 0);
                break;
            default:
                doRotate = false;
                break;
        }

        if (doRotate)
        {
            if (isShiftPressed)
                direction = -direction;
            rotationCenter = transform.position + new UnityEngine.Vector3(0, -1, 0);

            isMoving = true;
            float remainingAngle = 90;
            UnityEngine.Vector3 rotationAxis = direction;

            while (remainingAngle > 0)
            {
                float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
                if (allIndices)
                {
                    for (int i = 0; i < 3 * 3 * 3; i++)
                    {
                        rubicSubCubes[i].transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
                    }
                }
                else
                {
                    for (int i = 0; i < indicesRotate.Count(); i++)
                    {
                        rubicSubCubes[indicesAll[indicesRotate[i]]].transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
                    }
                }

                remainingAngle -= rotationAngle;
                yield return null;

            }
            string msg = "Rot:";
            for (int i = 0; i < indicesRotate.Count(); i++)
            {
                msg += $" {indicesRotate[i]}>>{indicesAll[indicesRotate[i]]}";
            }
            Debug.Log(msg);
            UpdateIndices(ch, isShiftPressed);
        }
        isMoving = false;
    }

    IEnumerator PlayMacro(char[] macro, bool isShiftPressed)
    {
        while (isMoving)
        {
            yield return null;
        }
        if (!isShiftPressed)
        {
            foreach (char c in macro)
            {
                isMoving = true;
                switch (c)
                {
                    case 'L':
                    case 'R':
                    case 'U':
                    case 'D':
                    case 'F':
                    case 'B':
                        StartCoroutine(Rotate9(c, false));
                        break;
                    case 'l':
                    case 'r':
                    case 'u':
                    case 'd':
                    case 'f':
                    case 'b':
                        StartCoroutine(Rotate9(Char.ToUpper(c), true));
                        break;
                    default:
                        isMoving = false;
                        break;
                }
                while (isMoving)
                {
                    yield return null;
                }
            }
        }
        else
        {
            for (int i = macro.Length - 1; i >= 0; --i)
            {
                isMoving = true;
                char c = macro[i];
                switch (c)
                {
                    case 'L':
                    case 'R':
                    case 'U':
                    case 'D':
                    case 'F':
                    case 'B':
                        StartCoroutine(Rotate9(c, true));
                        break;
                    case 'l':
                    case 'r':
                    case 'u':
                    case 'd':
                    case 'f':
                    case 'b':
                        StartCoroutine(Rotate9(Char.ToUpper(c), false));
                        break;
                    default:
                        isMoving = false;
                        break;
                }
                while (isMoving)
                {
                    yield return null;
                }
            }
        }
    }
}
