using UnityEngine;
using UnityEngine.UI;

public class SceneMove : MonoBehaviour
{
    public float speed;

    void Update()
    {
        Vector2 offset = new Vector2(Time.time * speed, 0);
        GetComponent<Image>().material.mainTextureOffset = offset;
    }
}
