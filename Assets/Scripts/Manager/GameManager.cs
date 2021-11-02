using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public static class ScriptHelper
{
    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        foreach(var item in list)
        {
            action(item);
        }
    }
    public static Vector2 Sum(this Vector2 vec1, Vector2 vec2)
    {
        return new Vector2(vec1.x + vec2.x, vec1.y + vec2.y);
    }
    public static Vector3 Sum(this Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
    }
}
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
                    Debug.LogError("GameManager is disappear!");
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
    private float slowTimeByLerp = 0f;
    private float firstSlowTimeByLerp = 0f;
    [SerializeField]
    private float slowTimeSomeObjectsTime = 3f;
    public float SlowTimeSomeObjectsTime
    {
        get { return slowTimeSomeObjectsTime; }
        set { slowTimeSomeObjectsTime = value; }
    }
    private float slowTimeSomeObjectsTimer = 0f;

    private int _currentStageNum = 0;
    public int currentStageNum
    {
        get { return _currentStageNum; }
    }

    private Func<float, float> TimeSlow;
    private Func<float, float> TimeSlowByLerp;
    private Func<float, float> FuncSlowTimeSomeObjects;
    public Action StopSlowTimeByLerp;
    public event Action SetFalseSlowTimeSomeObjects;
    public event Action<int> SpawnStages;
    public Action<bool> StageEnd;

    private bool _stopTime = false;
    public bool stopTime
    {
        get { return _stopTime; }
    }
    private bool slowTimeSomeObjects = false;
    public bool SlowTimeSomeObjects
    {
        get { return slowTimeSomeObjects; }
        set
        {
            if (value)
            {
                slowTimeSomeObjectsTimer = slowTimeSomeObjectsTime;
            }
            else
            {
                SetFalseSlowTimeSomeObjects();
            }

            slowTimeSomeObjects = value;
        }
    }
    private bool isGameEnd = false;
    public bool IsGameEnd
    {
        get { return isGameEnd; }
    }

    [SerializeField]
    private int slowTimeNum = 20; // 몇몇 오브젝트들만 느려지는 코드를 실행시킬 때 1/n으로 속도를 조정
    public int SlowTimeNum
    {
        get { return slowTimeNum; }
    }
    private int currentSlowTimeNum = 0;
    public int CurrentSlowTimeNum
    {
        get { return currentSlowTimeNum; }
    }
    public int CurrentSlowTimePerSlowTime
    {
        get { return currentSlowTimeNum / slowTimeNum; }
    }

    void Awake()
    {
        TimeSlow = a =>
        {
            if (a > 0f)
            {
                a -= Time.deltaTime;
                Time.timeScale = 0.8f;
            }
            else
            {
                a = 0f;
                Time.timeScale = 1f;
            }

            return a;
        };

        TimeSlowByLerp = a =>
        {
            if (a > 0f)
            {
                a -= Time.deltaTime;
                Time.timeScale = Mathf.Lerp(1f, 0.08f, a / firstSlowTimeByLerp * 20f);

                if (a <= 0f)
                {
                    StopSlowTimeByLerp();
                    Time.timeScale = 1f;
                }
            }


            return a;
        };

        FuncSlowTimeSomeObjects = a =>
        {
            if (a > 0f)
            {
                a -= Time.deltaTime;

                if (a <= 0f)
                {
                    a = 0f;
                    slowTimeSomeObjects = false;
                }
            }

            return a;
        };

        StopSlowTimeByLerp = () =>
        {
            slowTimeByLerp = 0f;
            firstSlowTimeByLerp = 0f;
        };

        SetFalseSlowTimeSomeObjects = () =>
        {
            slowTimeSomeObjects = false;
        };

        SpawnStages = stageNum =>
        {
            isGameEnd = false;
            SceneManager.LoadScene("StageScene");

            if (stageNum < 0)
            {
                stageNum = 0;
            }

            _currentStageNum = stageNum;
        };

        StageEnd = gameClear =>
        {
            isGameEnd = gameClear;

            if (gameClear)
            {
                // 게임을 클리어했을 때
            }
            else
            {
                // 게임 오버상태일 때
            }
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
        slowTimeByLerp = TimeSlowByLerp(slowTimeByLerp);
        slowTimeSomeObjectsTimer = FuncSlowTimeSomeObjects(slowTimeSomeObjectsTimer);

        // Debug.Log(Camera.main.WorldToViewportPoint(transform.position));
    }
    private void FixedUpdate()
    {
        if (slowTimeSomeObjects)
        {
            currentSlowTimeNum++;
        }
        else
        {
            currentSlowTimeNum = 0;
        }
    }
    public void SpawnStage(int stage) // 버튼에 사용
    {
        SpawnStages(stage);
    }
    public void SetSlowTime(float time)
    {
        slowTime = time;
    }
    public void SetSlowTimeByLerp(float time)
    {
        if (slowTimeByLerp <= 0f)
        {
            slowTimeByLerp = time;
            firstSlowTimeByLerp = time;
        }
    }
    public void StopTime(bool st)
    {
        _stopTime = st;
    }

}
