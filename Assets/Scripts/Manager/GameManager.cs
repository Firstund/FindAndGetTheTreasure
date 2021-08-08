using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        TimeSlow();
    }
    public void SpawnStages(int stageNum)
    {
        SceneManager.LoadScene("StageScene");
        if (stageNum < 1)
        {
            stageNum = 1;
        }

        _currentStage = stageNum;
    }
    public void SetSlowTime(float time)
    {
        slowTime = time;
    }
    private void TimeSlow()
    {
        if (slowTime > 0f)
        {
            slowTime -= Time.deltaTime;
            Time.timeScale = 0.4f;
        }
        else
        {
            slowTime = 0f;
            Time.timeScale = 1f;
        }
    }
}
