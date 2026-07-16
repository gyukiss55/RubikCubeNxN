
using TMPro;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public GameObject rubicCube;
    public GameObject mainCamera;
    public TextMeshProUGUI textMeshProUGUI;
    //private int viewIndex = 0;

    void Start()
    {
        if (textMeshProUGUI != null)
        {
            int width = Screen.width;
            int height = Screen.height;
            string type = "??";
            string subTypetype = "";
            if (Application.platform == RuntimePlatform.Android)
            {
                type = "And";
                if (width == 720 && height == 1560)
                {
                    subTypetype = "-HW P30";
                    UIControlPosition(100);
                }
                else
                if (width == 1800 && height == 2880)
                {
                    subTypetype = "-X P6";
                    UIControlPosition(400);
                }
            }
            else if (Application.platform == RuntimePlatform.OSXPlayer)
                type = "iOS";
            else if (Application.platform == RuntimePlatform.WindowsPlayer)
                type = "WP";
            else if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                type = "WE";
                UIControlPosition(-20);
            }
            string status = $"{type}{subTypetype}, {width}x{height}";
            textMeshProUGUI.text = status;
        }
    }

    // Update is called once per frame
    void UIControlPosition(int offset)
    {
        foreach (Canvas canvas in GetComponentsInChildren<Canvas>())
        {
            if (canvas != null)
            {
                foreach (Transform child in canvas.transform)
                {
                    Vector3 pos = child.transform.localPosition;
                    if (child.tag != "Lower")
                        pos.y += offset;
                    else
                        pos.y -= offset;
                    child.transform.localPosition = pos;
                }
            }
        }
    }

    /*
    void ButtonRotateClick(char ch, bool inv)
    {
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.RotateRubicCube(ch, inv);
            }
        }

    }

    void ButtonRotateCubesClick(int dirNum, int rows) // dirNum - 'X', 'Y', 'Z', 'x', 'y', 'z' rows - 0b00001, 0b00011, 0b10000, 0b11000, 0b11111
    {
        if (rubicCube != null)
        {
            RubiCubeGenerator rubicCubeGenerator = rubicCube.GetComponent<RubiCubeGenerator>();
            if (rubicCubeGenerator != null)
            {
                rubicCubeGenerator.RotateSubCubes(dirNum, rows); 
            }
        }
    }

    public void ButtonRPClick()
    {
        ButtonRotateClick('R', false);
    }
    public void ButtonRMClick()
    {
        ButtonRotateClick('R', true);
    }
    public void ButtonTPClick()
    {
        ButtonRotateClick('U', false);
    }
    public void ButtonTMClick()
    {
        ButtonRotateClick('U', true);
    }
    public void ButtonFPClick()
    {
        ButtonRotateClick('F', false);
    }
    public void ButtonFMClick()
    {
        ButtonRotateClick('F', true);
    }
    public void ButtonBPClick()
    {
        ButtonRotateClick('D', false);
    }
    public void ButtonBMClick()
    {
        ButtonRotateClick('D', true);
    }
    public void ButtonCPClick()
    {
        ButtonRotateClick('B', false);
    }
    public void ButtonCMClick()
    {
        ButtonRotateClick('B', true);
    }
    public void ButtonLPClick()
    {
        ButtonRotateClick('L', false);
    }
    public void ButtonLMClick()
    {
        ButtonRotateClick('L', true);
    }
    public void ButtonM1PClick()
    {
        MacroCommand(1, false);
    }
    public void ButtonM1MClick()
    {
        MacroCommand(1, true);
    }
    public void ButtonM2PClick()
    {
        MacroCommand(2, false);
    }
    public void ButtonM2MClick()
    {
        MacroCommand(2, true);
    }
    public void ButtonM3PClick()
    {
        MacroCommand(3, false);
    }
    public void ButtonM3MClick()
    {
        MacroCommand(3, true);
    }
    public void ButtonM4PClick()
    {
        MacroCommand(4, false);
    }
    public void ButtonM4MClick()
    {
        MacroCommand(4, true);
    }
    public void ButtonM5PClick()
    {
        MacroCommand(5, false);
    }
    public void ButtonM5MClick()
    {
        MacroCommand(5, true);
    }
    public void ButtonM6PClick()
    {
        MacroCommand(6, false);
    }
    public void ButtonM6MClick()
    {
        MacroCommand(6, true);
    }
    public void ButtonM7PClick()
    {
        MacroCommand(11, false);
    }
    public void ButtonM7MClick()
    {
        MacroCommand(11, true);
    }
    public void ButtonXPClick()
    {
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.RotateCube(false, false); // X axis ++
            }
        }
    }
    public void ButtonXMClick()
    {
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.RotateCube(false, true); // X axis --
            }
        }
    }
    public void ButtonYPClick()
    {
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.RotateCube(true, false); // Y axis ++
            }
        }
    }
    public void ButtonYMClick()
    {
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.RotateCube(true, true); // Y axis --
            }
        }
    }


    public void MacroCommand(int index, bool inv)
    {
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.PlayMacro(index, inv);
            }
        }
    }

    public void ButtonVPClick()
    {
        if (mainCamera != null)
        {
            CameraController cameraControl = mainCamera.GetComponent<CameraController>();
            if (cameraControl != null)
            {
                viewIndex++;
                Debug.Log($"VP:{viewIndex}");
                cameraControl.SetViewPoint(viewIndex);
            }
        }
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.SpeedMultiply(0.5f);
            }
        }

    }
    public void ButtonVMClick()
    {
        if (mainCamera != null)
        {
            CameraController cameraControl = mainCamera.GetComponent<CameraController>();
            if (cameraControl != null)
            {
                viewIndex--;
                Debug.Log($"VM:{viewIndex}");
                cameraControl.SetViewPoint(viewIndex);
            }
            if (rubicCube != null)
            {
                RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
                if (rubicCubeControl != null)
                {
                    rubicCubeControl.SpeedMultiply(2);
                }
            }
        }

    }

    public void ButtonRCReset()
    {
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.ResetRubicCube(); // Call a method from MyScript
            }
        }
    }

    public void ButtonRCRandom()
    {
        if (rubicCube != null)
        {
            RubicCubeControl rubicCubeControl = rubicCube.GetComponent<RubicCubeControl>();
            if (rubicCubeControl != null)
            {
                rubicCubeControl.RandomRubicCube(); // Call a method from MyScript
            }
        }
    }
    */
}
