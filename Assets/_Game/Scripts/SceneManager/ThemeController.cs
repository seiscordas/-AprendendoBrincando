using LearningByPlaying.gameTheme;
using System.Collections.Generic;
using UnityEngine;
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
            int imageName = i + 1;
            Texture image = Resources.Load<Texture>(imagePath + imageName);
            print(i + " image: " + imageName);
            if (image != null)
            {
                imageList[i].material.mainTexture = image;
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
