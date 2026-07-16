using TMPro;
using UnityEngine;

public class Arrow3D : MonoBehaviour
{
    [Header("Assign a Cone Prefab")]
    public GameObject conePrefab;

    public char letter = 'A';

    const float shaftLength = 1.0f;
    const float shaftDiameter = 0.1f;

    const float coneLength = 0.2f;
    const float coneDiameter = 0.2f;

    void Start()
    {
        CreateArrow(letter);
    }

    public void CreateArrow(char c)
    {
        //-------------------------
        // Parent
        //-------------------------
        GameObject root = new GameObject("Arrow");

        //-------------------------
        // Shaft
        //-------------------------
        GameObject shaft = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        shaft.name = "Shaft";
        shaft.transform.SetParent(root.transform);

        // Unity cylinder height = scale.y * 2
        shaft.transform.localScale = new Vector3(
            shaftDiameter,
            shaftLength * 0.5f,
            shaftDiameter);

        shaft.transform.localPosition =
            new Vector3(0, shaftLength * 0.5f, 0);

        //-------------------------
        // Cone
        //-------------------------
        GameObject cone = Instantiate(conePrefab);
        cone.name = "Head";
        cone.transform.SetParent(root.transform);

        // Assumes cone mesh height = 1
        cone.transform.localScale = new Vector3(
            coneDiameter,
            coneLength,
            coneDiameter);

        cone.transform.localPosition =
            new Vector3(0,
                shaftLength + coneLength * 0.5f,
                0);

        //-------------------------
        // Letter
        //-------------------------
        GameObject textGO = new GameObject("Letter");
        textGO.transform.SetParent(root.transform);

        TextMeshPro tmp = textGO.AddComponent<TextMeshPro>();

        tmp.text = c.ToString();
        tmp.fontSize = 20.0f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        textGO.transform.localScale = Vector3.one * 0.1f;

        // Tip of cone
        textGO.transform.localPosition =
            new Vector3(
                0,
                shaftLength + coneLength,
                0);

        // Face camera if desired
        textGO.transform.localRotation = Quaternion.identity;
    }
}