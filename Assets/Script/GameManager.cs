using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Inst
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    public Tiles tileManager;
    public BattleManager battleManager;
    public float gameSpeed = 1f;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        gameSpeed = 1f;
        Application.targetFrameRate = 120;
    }
}
