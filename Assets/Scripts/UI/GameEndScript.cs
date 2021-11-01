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
        gameManager.StageEnd += gameClear =>
        {
            WhenGameEnd();
        };
    }
    private void OnDisable() 
    {
        gameManager.StageEnd -= gameClear =>
        {
            WhenGameEnd();
        };
    }

    private void WhenGameEnd()
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
        if(gotoMainTimer > 0f)
        {
            gotoMainTimer -= Time.deltaTime;
            
            if(gotoMainTimer <= 0f || Input.GetKeyUp(KeyCode.Space))
            {
                gameManager.StopTime(false);
                SceneManager.LoadScene("MenuScene");
            }
        }

    }
}
