using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkableObject : MonoBehaviour
{
    private GameManager gameManager = null;
    private TalkManager talkManager = null;

    private float distance = 0f;
    [SerializeField]
    private float talkableDistance = 0.5f;
    [SerializeField]
    private int spawnTextBoxIndex = 0;

    void Start()
    {
        gameManager = GameManager.Instance;
        talkManager = FindObjectOfType<TalkManager>();
    }
    void Update()
    {
        distance = Vector2.Distance(transform.position, gameManager.player.transform.position);

        if(Input.GetButtonDown("Attack") && distance <= talkableDistance)
        {
            talkManager.currentTextBox.SpawnTextBox(spawnTextBoxIndex);
        }
    }
}
