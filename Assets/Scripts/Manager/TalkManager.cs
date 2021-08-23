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
    void Start()
    {
        gameManager = GameManager.Instance;
        dontDestroyOnLoadManager = DontDestroyOnLoadManager.Instance;

        gameManager.SpawnStages += x => SpawnTextBoxes(x - 1);

        dontDestroyOnLoadManager.DoNotDestroyOnLoad(gameObject);
    }
    private void SpawnTextBoxes(int index)
    {
        textBoxesParent[index].SetActive(true);

        _currentTextBoxesParent = textBoxesParent[index].GetComponent<TextBoxesParent>();
    }
    public void DeSpawnTextBoxes(int index)
    {
        textBoxesParent[index].SetActive(false);
    }
}
