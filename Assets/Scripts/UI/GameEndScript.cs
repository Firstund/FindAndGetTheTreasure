using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameEndScript : MonoBehaviour
{
    private GameManager gameManager = null;
    [SerializeField]
    private Sprite backgroundSprite;
    private Image backgroundImg = null;
    [SerializeField]
    private float gotoMainTime = 10f;
    private float gotoMainTimer = 0f;

    void Awake()
    {
        gameManager = GameManager.Instance;
    }
    private void OnEnable()
    {
        EventManager.StartListening_Bool("StageEnd", WhenGameEnd);
    }
    private void OnDisable()
    {
        EventManager.StopListening_Bool("StageEnd", WhenGameEnd);
    }

    private void WhenGameEnd(bool a)
    {
        gameManager.StopTime(true);
        gotoMainTimer = gotoMainTime;
        backgroundImg.DOFade(1f, 5f);
    }

    void Start()
    {
        backgroundImg = GetComponent<Image>();

        backgroundImg.color = new Vector4(1f, 1f, 1f, 0f);

        backgroundImg.sprite = backgroundSprite;
    }
    void Update()
    {
        if (gotoMainTimer > 0f)
        {
            gotoMainTimer -= Time.deltaTime;

            if (gotoMainTimer <= 0f || Input.GetKeyUp(KeyCode.Space))
            {
                GetOutStage();
            }
        }
    }
    private void GetOutStage()
    {
        gameManager.StopTime(false);
        EventManager.TriggerEvent("WhenGoToStageSelectMenu");
        SceneManager.LoadScene("StageSelectScene");
    }
}
