using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxesParent : MonoBehaviour
{
    private GameManager gameManager = null;
    [SerializeField]
    private List<GameObject> textBoxes = new List<GameObject>();
    private GameObject currentTextBox = null;
    void Start()
    {
        gameManager = GameManager.Instance;

        for (int i = 0; i < transform.childCount; i++)
        {
            textBoxes.Add(transform.GetChild(i).gameObject);
        }
    }
    public void SpawnTextBox(int index)
    {
        currentTextBox = textBoxes[index];
        currentTextBox.SetActive(true);
        
        gameManager.StopTime(true);
    }
    public void DeSpawnTextBox()
    {
        currentTextBox.SetActive(false);

        gameManager.StopTime(false);
    }
}
