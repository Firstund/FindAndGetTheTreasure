using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera _cinemachineVirtualCamera = null;
    public CinemachineVirtualCamera cinemachineVirtualCamera
    {
        get { return _cinemachineVirtualCamera; }
    }

    [SerializeField]
    private GameObject[] _stages;
    public GameObject[] stages
    {
        get { return _stages; }
    }

    [SerializeField]
    private CharacterMove _player = null;
    public CharacterMove player
    {
        get
        {
            if (_player == null)
            {
                _player = FindObjectOfType<CharacterMove>();
            }

            return _player;
        }
    }
    [SerializeField]
    private CharacterStat _characterStat = null;
    public CharacterStat characterStat
    {
        get { return _characterStat; }
        set { _characterStat = value; }
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject temp = new GameObject("GameManager");
                    instance = temp.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    private float slowTime = 0f;
    private int _currentStage = 0;
    public int currentStage
    {
        get { return _currentStage; }
    }

    private Func<float, float> TimeSlow;
    public event Action<int> SpawnStages;

    void Awake()
    {
        TimeSlow = a =>
        {
            if (a > 0f)
            {
                a -= Time.deltaTime;
                Time.timeScale = 0.4f;
            }
            else
            {
                a = 0f;
                Time.timeScale = 1f;
            }

            return a;
        };

        SpawnStages = stageNum => 
        {
            SceneManager.LoadScene("StageScene");
        if (stageNum < 1)
        {
            stageNum = 1;
        }

        _currentStage = stageNum;
        };
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        slowTime = TimeSlow(slowTime);
    }
    public void OnClickSpawnStageBtn(int stage)
    {
        SpawnStages(stage);
    }
    public void SetSlowTime(float time)
    {
        slowTime = time;
    }
}
