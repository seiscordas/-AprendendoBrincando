using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsDataBase : MonoBehaviour
{
    public static PlayerPrefsDataBase Instance;

    public const string CONST_POINTS = "AB_POINTS";
    public const string CONST_TUTORIAL = "AB_TUTORIAL";
    public const string CURRENT_HISTORY = "CurrentHistory";
    public static string CONST_TUTORIAL1 => CONST_TUTORIAL;

    [SerializeField] private int points;

    public int Points { get => points; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        points = ReadIntInfo(CONST_POINTS);
    }

    public int ReadIntInfo(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetInt(key);
        }
        else
        {
            return 0;
        }
    }

    public string ReadStringInfo(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            //Debug.Log(key+": "+ PlayerPrefs.GetString(key));
            return PlayerPrefs.GetString(key);
        }
        else
        {
            return null;
        }
    }

    public void SaveIntInfo(string key, int valor)
    {
        PlayerPrefs.SetInt(key, valor);
    }

    public void SaveStringInfo(string key, string valor)
    {
        //Debug.Log(key+" : "+ valor);
        PlayerPrefs.SetString(key, valor);
    }

    internal void SavePoints()
    {
        points++;
        //Debug.Log("Gravando Pontos : "+ intPontos);
        SaveIntInfo(CONST_POINTS, points);
    }
}

