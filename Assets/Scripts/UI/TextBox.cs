using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBox : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> textBoxes = new List<GameObject>();
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            textBoxes.Add(transform.GetChild(i).gameObject);
        }
    }
    public void SpawnTextBox(int index)
    {
        textBoxes[index].SetActive(true);
    }
    public void DeSpawnTextBex(int index)
    {
        textBoxes[index].SetActive(false);
    }
}
