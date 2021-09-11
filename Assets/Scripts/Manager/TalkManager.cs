using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    private GameManager gameManager = null;
    private DontDestroyOnLoadManager dontDestroyOnLoadManager = null;
    [SerializeField]
    private List<GameObject> textBoxesParent;

    private TextBoxesParent _currentTextBoxesParent;
    public TextBoxesParent currentTextBoxesParent
    {
        get { return _currentTextBoxesParent; }
    }
    private void Awake()
    {
        gameManager = GameManager.Instance;

        
    }
    void Start()
    {
        dontDestroyOnLoadManager = DontDestroyOnLoadManager.Instance;


        dontDestroyOnLoadManager.DoNotDestroyOnLoad(gameObject);
    }
    private void OnEnable() 
    {
        gameManager.SpawnStages += x => SpawnTextBoxes(x - 1);
        gameManager.GameEnd += a => DeSpawnTextBoxes();
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
