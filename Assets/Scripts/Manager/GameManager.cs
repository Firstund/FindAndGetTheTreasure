using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CharacterMove _player = null;
    public CharacterMove player
    {
        get { return _player; }
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

    [SerializeField]
    private Transform _enemys = null;
    public Transform enemys
    {
        get { return _enemys; }
    }
    private float slowTime = 0f;
    void Start()
    {

    }

    void Update()
    {
        TimeSlow();
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
