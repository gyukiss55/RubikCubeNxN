using System.Collections;
using System.IO;
using UnityEngine;

public class ReadJPG : MonoBehaviour
{
    public string fileName = "image.jpg"; // Change to your file name

    string[] jpgFiles;

    void Start()
    {
        ListJPGFilesInStreamingAssets();
        StartCoroutine(LoadImage());
    }

    IEnumerator LoadImage()
    {
        string path = Application.streamingAssetsPath;
        Debug.Log("StreamingAssets Path: " + path);

        if (jpgFiles.Length > 0)
            fileName = jpgFiles[0];

        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            yield break;
        }

        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        Debug.Log($"Image Size: {texture.width} x {texture.height}");

        // Print RGB values of all pixels
        Color[] pixels = texture.GetPixels();
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                Color pixel = pixels[y * texture.width + x];
                Debug.Log($"Pixel[{x},{y}] - R:{pixel.r * 255}, G:{pixel.g * 255}, B:{pixel.b * 255}");
            }
        }
    }
    void ListJPGFilesInStreamingAssets()
    {
        string folderPath = Application.streamingAssetsPath;

        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("StreamingAssets folder not found!");
            return;
        }

        jpgFiles = Directory.GetFiles(folderPath, "*.jpg");

        if (jpgFiles.Length == 0)
        {
            Debug.Log("No JPG files found in StreamingAssets.");
        }
        else
        {
            Debug.Log("JPG Files in StreamingAssets:");
            foreach (string file in jpgFiles)
            {
                Debug.Log(Path.GetFileName(file));
            }
        }
    }
}
