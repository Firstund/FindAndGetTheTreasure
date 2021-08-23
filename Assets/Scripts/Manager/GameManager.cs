using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
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
    private DontDestroyOnLoadManager dontDestroyOnLoadManager = null;

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

    private float slowTime = 0f;
    private int _currentStage = 0;
    public int currentStage
    {
        get { return _currentStage; }
    }

    private Func<float, float> TimeSlow;
    public event Action<int> SpawnStages;
    public Action<bool> GameEnd;

    private bool _stopTime = false;
    public bool stopTime
    {
        get { return _stopTime; }
    }

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

        GameEnd = gameClear =>
        {
            if(gameClear)
            {
                // 게임을 클리어했을 때
            }
            else
            {
                // 게임 오버상태일 때
            }

            SceneManager.LoadScene("MenuScene");
        };

    }
    void Start()
    {
        dontDestroyOnLoadManager = DontDestroyOnLoadManager.Instance;
        
        dontDestroyOnLoadManager.DoNotDestroyOnLoad(gameObject);
    }
    void Update()
    {
        slowTime = TimeSlow(slowTime);
    }
    private void OnClickSpawnStageBtn(int stage)
    {
        SpawnStages(stage);
    }
    public void SetSlowTime(float time)
    {
        slowTime = time;
    }
    public void StopTime(bool st)
    {
        _stopTime = st;
    }

}
