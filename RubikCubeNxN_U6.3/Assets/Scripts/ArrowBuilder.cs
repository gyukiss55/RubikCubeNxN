using UnityEngine;
using TMPro;

public class ArrowBuilder : MonoBehaviour
{
    void Start()
    {
        CreateArrow("ArrowX", 'X', new Vector3(0, 0, -90), Color.blue);
        CreateArrow("ArrowY", 'Y', new Vector3(0, 0, 0), Color.green);
        CreateArrow("ArrowZ", 'Z', new Vector3(90, 0, 0), Color.red);
    }

    void CreateArrow(string name, char letter, Vector3 rot, Color color)
    {
        Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = color;

        GameObject arrow = new GameObject(name);
        arrow.transform.parent = this.transform;
        arrow.transform.localPosition = Vector3.zero;

        //---------------- Shaft ----------------

        GameObject shaft = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        shaft.transform.SetParent(arrow.transform);

        shaft.transform.localScale =
            new Vector3(0.1f, 0.5f, 0.1f);

        shaft.transform.localPosition =
            new Vector3(0, 0.5f, 0);

        shaft.GetComponent<MeshRenderer>().material = mat;

        //---------------- Cone ----------------

        GameObject cone = new GameObject("Cone");
        cone.transform.SetParent(arrow.transform);

        cone.transform.localPosition =
            new Vector3(0, 1.0f, 0);

        cone.AddComponent<MeshFilter>();
        cone.AddComponent<MeshRenderer>();

        ProceduralCone pc = cone.AddComponent<ProceduralCone>();
        pc.radius = 0.1f;
        pc.height = 0.4f;
        pc.sides = 32;

        float arrowLength = 1.5f;

        cone.GetComponent<MeshRenderer>().material = mat;

        GameObject textGO = new GameObject("Letter");
        textGO.transform.SetParent(arrow.transform);

        TextMeshPro tmp = textGO.AddComponent<TextMeshPro>();

        tmp.text = letter.ToString();
        tmp.fontSize = 20.0f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        textGO.transform.localScale = Vector3.one * 0.1f;

        // Tip of cone
        textGO.transform.localPosition =
            new Vector3(
                0,
                arrowLength,
                0);

        // Face camera if desired
        textGO.transform.localRotation = Quaternion.identity;

        arrow.transform.localRotation = Quaternion.Euler(rot.x, rot.y, rot.z);

    }
}