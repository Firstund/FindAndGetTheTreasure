using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEventTrigger : MonoBehaviour, IEventTrigger
{
    private GameManager gameManager = null;
    private TalkManager talkManager = null;

    [Header("Textbox 오브젝트 네이밍 규칙: Textbox_(이 텍스트박스가 쓰이는 스테이지)_(이 텍스트박스를 부를 때 입력해야하는 index)_(텍스트박스의 이름)")]
    [SerializeField]
    private int spawnTextBoxIndex = 0;
    
    private void Awake()
    {
        gameManager = GameManager.Instance;
        talkManager = TalkManager.Instance;
    }

    public void DoEvent()
    {
        if (!gameManager.IsGameEnd)
        {
            talkManager.currentTextBoxesParent.SpawnTextBox(spawnTextBoxIndex);
        }
    }
}
