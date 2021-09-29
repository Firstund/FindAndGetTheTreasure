using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxesParent : MonoBehaviour
{
    private GameManager gameManager = null;

    [Header("Textbox 오브젝트 네이밍 규칙: Textbox_(이 텍스트박스가 쓰이는 스테이지)_(이 텍스트박스를 부를 때 입력해야하는 index)_(텍스트박스의 이름)")]
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
