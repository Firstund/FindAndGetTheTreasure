using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkableObject : MonoBehaviour
{
    private GameManager gameManager = null;
    private TalkManager talkManager = null;

    private SpriteRenderer spriteRenderer = null;

    private float distance = 0f;
    [SerializeField]
    private float talkableDistance = 0.5f;

    [SerializeField]
    private float fadeInTime = 5f;
    private float fadeInTimer = 0f;

    [Header("Textbox 오브젝트 네이밍 규칙: Textbox_(이 텍스트박스가 쓰이는 스테이지)_(이 텍스트박스를 부를 때 입력해야하는 index)_(텍스트박스의 이름)")]
    [SerializeField]
    private int spawnTextBoxIndex = 0;

    [SerializeField]
    private bool despawnWhenTextEvent = false;
    private bool fadeInStarted = false;

    void Start()
    {
        gameManager = GameManager.Instance;
        talkManager = TalkManager.Instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        distance = Vector2.Distance(transform.position, gameManager.player.transform.position);

        CheckTalk();
        FadeIn();
    }

    private void CheckTalk()
    {
        if (Input.GetButtonDown("TalkWithTalkable") && distance <= talkableDistance && !gameManager.IsGameEnd)
        {
            spriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);

            talkManager.currentTextBoxesParent.SpawnTextBox(spawnTextBoxIndex); // 이 오브젝트도 대화창 이벤트시 SetActive(false)로 한다.
            talkManager.CurrentTalkableObject = this;
            
            gameObject.SetActive(!despawnWhenTextEvent);
        }
    }
    public void StartFadeIn()
    {
        fadeInTimer = 0f;
        fadeInStarted = true;
    }
    private void FadeIn()
    {
        if(fadeInTimer < fadeInTime && fadeInStarted)
        {
            fadeInTimer += Time.deltaTime;

            spriteRenderer.color = new Vector4(1f, 1f, 1f, fadeInTimer / fadeInTime);
        }
        else
        {
            fadeInStarted = false;
            spriteRenderer.color = new Vector4(1f, 1f, 1f, 1f);
        }
    }
}
