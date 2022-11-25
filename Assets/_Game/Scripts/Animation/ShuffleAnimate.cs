using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShuffleAnimate
{
    private static List<GameObject> chidrenShuffle;

    public static List<int> intList = new();

    public static void Animate(List<GameObject> chidren)
    {
        Shuffle(chidren);
        for (int i = 0; i < chidren.Count; i++)
        {
            Transform currentPosition = chidren[i].transform;
            Vector3 targetPosition = chidrenShuffle[i].transform.localPosition;
            CallMethodDynamically(currentPosition, targetPosition);
        }
    }

    private static void CallMethodDynamically(Transform currentPosition, Vector3 targetPosition)
    {
        int i = RandomNonRepeat(2);
        switch (i)
        {
            case 1:
                Jump(currentPosition, targetPosition);
                break;
            case 2:
                Move(currentPosition, targetPosition);
                break;
            default:
                Jump(currentPosition, targetPosition);
                break;
        }

    }
    private static void Jump(Transform currentPosition, Vector3 targetPosition)
    {
        currentPosition.DOLocalJump(targetPosition, 100f, 1, 2f);
    }
    private static void Move(Transform currentPosition, Vector3 targetPosition)
    {
        currentPosition.DOLocalMove(targetPosition, 1);
    }

    private static void Shuffle(List<GameObject> chidren)
    {        
        chidrenShuffle = chidren.OrderBy(x => Guid.NewGuid()).ToList();
    }

    private static int RandomNonRepeat(int range)
    {
        if(intList.Count == 0)
        {
            for (int i = 1; i <= range; i++)
            {
                intList.Add(i);
            }
        }
        int index = UnityEngine.Random.Range(0, intList.Count);
        int number = intList[index];
        intList.Remove(number);
        return number;
    }
}
