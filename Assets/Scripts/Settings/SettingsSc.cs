using Assets.Scripts.Memory;
using Assets.Scripts.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSc : MonoBehaviour
{
    public const int ScreenHeight = 1080;//2340;
    public static int ScreenWidth = 2340;//1080;

    public static float ConscriptKoeff;

    public static bool IsPaused
    {
        get;
        private set;
    }

    public static bool IsFinished
    {
        get;
        private set;
    }

    public static BasePlayer Winner
    {
        get;
        private set;
    }

    public static bool RegenerateMap
    {
        get;
        set;
    }

    public static CastleStoredData Stored
    {
        get;
        set;
    }

    static SettingsSc()
    {
        RegenerateMap = true;
        Stored = new CastleStoredData();

    }

    // Use this for initialization
    void Start()
    {
        IsFinished = false;
        
        ConscriptKoeff = 1.3f;

        var spawner = GameObject.FindGameObjectWithTag("Respawn").GetComponent<CastleSpawnerSc>();

        if (spawner == null)
            Debug.LogWarning("CastleSpawner is null");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SetPause(bool value)
    {
        IsPaused = value;
    }

    //public static SettingsSc FindMe()
    //{
    //    return GameObject.FindGameObjectWithTag("Settings").GetComponent<SettingsSc>();
    //}

    public static void SetWinner(BasePlayer winner)
    {
        IsFinished = true;
        Winner = winner;
    }
}
