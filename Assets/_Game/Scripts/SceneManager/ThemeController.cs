using LearningByPlaying.gameTheme;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ThemeController : MonoBehaviour
{
    public static ThemeController Instance;

    [SerializeField] Transform backGround;

    private string themeName;
    [SerializeField] private List<Image> imageList;

    private void Awake()
    {
        Instance = this;
        themeName = CurrentGameTheme.GetGameTheme();
    }

    private void Start()
    {
        GetMaterials();
        SetBGTheme();
    }

    public void SetBGTheme()
    {
        string imagePath = "Themes/" + CurrentGameTheme.GetGameTheme() + "/Default/";
        for (int i = 0; i < imageList.Count; i++)
        {
            Texture image = Resources.Load<Texture>(imagePath + i);
            if (image != null)
            {
                imageList[i].material.mainTexture = Resources.Load<Texture>(imagePath + i);
                imageList[i].material.mainTexture.wrapMode = TextureWrapMode.Repeat;
            }
        }
    }

    private void GetMaterials()
    {
        for (int i = 0; i < backGround.childCount; i++)
        {
            imageList.Add(backGround.GetChild(i).GetComponent<Image>());
        }
    }
}
