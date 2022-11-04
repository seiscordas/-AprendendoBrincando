using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageController : MonoBehaviour
{
    [Header("Image Stuff")]
    [SerializeField] private string imagePath;

    public Sprite LoadImage(string imgTheme, string imgName)
    {
        return Resources.Load<Sprite>(imagePath + imgTheme + "/" + imgName);
    }
}
