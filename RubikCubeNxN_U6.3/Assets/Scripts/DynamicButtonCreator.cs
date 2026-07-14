using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DynamicButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign a Button prefab in the inspector or create it programmatically
    public Transform buttonParent; // Assign the parent container for the button (e.g., a Canvas or Panel)
    public Transform rubicCubeCore; // Assign the rubicCubeCore 
    public bool inverze = false; 
    public int level = 1;
    public int offsetX = 2000;
    GameObject buttonInverze;
    GameObject buttonRow;
    RubiCubeGenerator rubiCubeGenerator = null;

    void Start()
    {
        CreateButton("RI", OnButtonClickRight, new Vector3(80 + offsetX, 60, 0));
        CreateButton("UP", OnButtonClickUp, new Vector3(-80 + offsetX, 60, 0));
        CreateButton("FR", OnButtonClickFront, new Vector3(-240 + offsetX, 60, 0));
        CreateButton("LE", OnButtonClickLeft, new Vector3(-400 + offsetX, 60, 0));
        CreateButton("DO", OnButtonClickDown, new Vector3(240 + offsetX, 60, 0));
        CreateButton("BA", OnButtonClickBack, new Vector3(400 + offsetX, 60, 0));
        buttonInverze = CreateButton("NI", OnButtonClickInv, new Vector3(80 + offsetX, 120, 0));
        buttonRow = CreateButton("RW", OnButtonClickRow, new Vector3(-80 + offsetX, 120, 0));
        CreateButton("RE", OnButtonClickReset, new Vector3(-240 + offsetX, 120, 0));

        rubiCubeGenerator = rubicCubeCore.GetComponent<RubiCubeGenerator>();
        rubiCubeGenerator.ResetSubCubes();
    }

    // Method to create a button dynamically
    public GameObject CreateButton(string buttonText, UnityEngine.Events.UnityAction onClickAction, Vector3 offset)
    {
        if (buttonPrefab == null)
        {
            Debug.LogError("Button Prefab is not assigned!");
            return null;
        }

        // Instantiate the button prefab
        GameObject newButton = Instantiate(buttonPrefab, buttonParent);
        //string name = buttonText + "Button";
        newButton.name = buttonText + "Button";

        Vector3 pos = newButton.transform.localPosition;
        pos = pos + offset;
        newButton.transform.localPosition = pos;

        // Set the button text
        TextMeshProUGUI buttonLabel = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonLabel != null)
        {
            buttonLabel.text = buttonText;
        }

        // Add the click event listener
        Button buttonComponent = newButton.GetComponent<Button>();
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(onClickAction);
        }
        return newButton;
    }

    int SetRows(int rows)
    {
        int ret = rows;
        if (level == 2)
        {
            if (rows == 1)
                ret |= 0b00010;
            else
                ret |= 0b01000;
        }
        else if(level > 2)
        {
            ret = 0b11111;
        }
        return ret;
    }

    private void OnButtonClickLeft()
    {
        Debug.Log("Dynamic button left click!");
        //if (rubiCubeGenerator != null)
        if (inverze)
            rubiCubeGenerator.RotateSubCubes(3, SetRows(0b00001));
        else
            rubiCubeGenerator.RotateSubCubes(0, SetRows(0b00001));
    }

    private void OnButtonClickRight()
    {
        Debug.Log("Dynamic button right click!");
        if (inverze)
            rubiCubeGenerator.RotateSubCubes(0, SetRows(0b10000));
        else
            rubiCubeGenerator.RotateSubCubes(3, SetRows(0b10000));
    }

    private void OnButtonClickUp()
    {
        Debug.Log("Dynamic button up click!");
        if (inverze)
            rubiCubeGenerator.RotateSubCubes(1, SetRows(0b10000));
        else
            rubiCubeGenerator.RotateSubCubes(4, SetRows(0b10000));
    }

    private void OnButtonClickDown()
    {
        Debug.Log("Dynamic button down click!");
        if (inverze)
            rubiCubeGenerator.RotateSubCubes(4, SetRows(0b00001));
        else
            rubiCubeGenerator.RotateSubCubes(1, SetRows(0b00001));
    }

    private void OnButtonClickFront()
    {
        Debug.Log("Dynamic button front click!");
        if (inverze)
            rubiCubeGenerator.RotateSubCubes(5, SetRows(0b00001));
        else
            rubiCubeGenerator.RotateSubCubes(2, SetRows(0b00001));
    }

    private void OnButtonClickBack()
    {
        Debug.Log("Dynamic button back click!");
        if (inverze)
            rubiCubeGenerator.RotateSubCubes(2, SetRows(0b10000));
        else
            rubiCubeGenerator.RotateSubCubes(5, SetRows(0b10000));
    }

    private void OnButtonClickInv()
    {
        Debug.Log("Dynamic button down click!");
        inverze = ! inverze;
        TextMeshProUGUI buttonLabel = buttonInverze.GetComponentInChildren<TextMeshProUGUI>();
        if (inverze)
            buttonLabel.text = "IV";
        else
            buttonLabel.text = "NI";
    }

    private void OnButtonClickRow()
    {
        Debug.Log("Dynamic button down click!");
        switch (level)
        {
            case 1:
                level++;
                break;
            case 2:
                level = 5;
                break;
            case 5:
                level = 1;
                break;

        }
        TextMeshProUGUI buttonLabel = buttonRow.GetComponentInChildren<TextMeshProUGUI>();
        buttonLabel.text = $"R{level}";
    }

    private void OnButtonClickReset()
    {
        Debug.Log("Dynamic button down click!");
        rubiCubeGenerator.ResetSubCubes();
    }
}
