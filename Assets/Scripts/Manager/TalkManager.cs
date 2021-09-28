using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    private static TalkManager instance = null;
    public static TalkManager Instance
    {
        get{
            if(instance == null)
            {
                instance = FindObjectOfType<TalkManager>();

                if(instance == null)
                {
                    Debug.LogError("TalkManager is disappear!");
                }
            }

            return instance;
        }
    }

    private GameManager gameManager = null;
    private DontDestroyOnLoadManager dontDestroyOnLoadManager = null;
    [SerializeField]
    private List<GameObject> textBoxesParent;

    [SerializeField]
    private Queue<TextEventObject> currentEvents = new Queue<TextEventObject>();
    public Queue<TextEventObject> CurrentEvents
    {
        get { return currentEvents; }
    }
    [SerializeField]
    private TextEventObject[] test;

    [SerializeField]
    private Transform eventObjectTrm = null;
    public Transform EventObjectTrm
    {
        get { return eventObjectTrm; }
    }

    private TextBoxesParent _currentTextBoxesParent;
    public TextBoxesParent currentTextBoxesParent
    {
        get { return _currentTextBoxesParent; }
    }
    private TalkableObject currentTalkableObject = null;
    public TalkableObject CurrentTalkableObject
    {
        get { return currentTalkableObject; }
        set { currentTalkableObject = value; }
    }

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        dontDestroyOnLoadManager = DontDestroyOnLoadManager.Instance;

        dontDestroyOnLoadManager.DoNotDestroyOnLoad(gameObject);

        gameManager.SpawnStages += x => SpawnTextBoxes(x - 1);
        gameManager.GameEnd += a => DeSpawnTextBoxes();
    }
    private void Update()
    {
        test = currentEvents.ToArray();
    }
    private void OnDisable()
    {
        gameManager.SpawnStages -= x => SpawnTextBoxes(x - 1);
        gameManager.GameEnd -= a => DeSpawnTextBoxes();
    }
    private void SpawnTextBoxes(int index)
    {
        textBoxesParent[index].SetActive(true); // 메뉴로 돌아왔다가 다시 스테이지 접근에 시도하면 textBoxesParent가 null이 되는 현상 발생

        _currentTextBoxesParent = textBoxesParent[index].GetComponent<TextBoxesParent>();
    }
    public void DeSpawnTextBoxes()
    {
        currentTextBoxesParent.gameObject.SetActive(false);

        _currentTextBoxesParent = null;
    }
}
