using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RotateAroundAnimation : MonoBehaviour
{
    [Range(10f, 200f)]
    [SerializeField] private float speed;

    private void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.back, speed * Time.deltaTime);
        transform.rotation = Quaternion.identity;
    }
}
