using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    private GameManager gameManager = null;
    [SerializeField]
    private List<GameObject> textBoxes;

    private TextBox _currentTextBox;
    public TextBox currentTextBox
    {
        get { return _currentTextBox; }
    }
    void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.SpawnStages += x => SpawnTextBoxes(x - 1);

        DontDestroyOnLoad(this);
    }
    public void SpawnTextBoxes(int index)
    {
        textBoxes[index].SetActive(true);

        _currentTextBox = textBoxes[index].GetComponent<TextBox>();
    }
    public void DeSpawnTextBoxes(int index)
    {
        textBoxes[index].SetActive(false);
    }
}
