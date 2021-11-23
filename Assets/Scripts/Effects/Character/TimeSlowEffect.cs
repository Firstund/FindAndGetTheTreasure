using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowEffect : MonoBehaviour

{
    private GameManager gameManager = null;
    [SerializeField]
    private float maxSize = 20f;
    [SerializeField]
    private float fadeInTime = 0.3f;
    private float fadeInTimer = 0f;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    void Update()
    {
        if (fadeInTimer < fadeInTime)
        {
            fadeInTimer += Time.deltaTime;

            if (transform.localScale.x < maxSize || transform.localScale.y < maxSize)
            {
                transform.localScale = new Vector2(
                    Mathf.Lerp(0f, maxSize, fadeInTimer / fadeInTime),
                 Mathf.Lerp(0f, maxSize, fadeInTimer / fadeInTime));
            }
        }
    }
    private void OnEnable()
    {
        transform.localScale = new Vector3(0f, 0f, 1f);
        fadeInTimer = 0f;

        Debug.Log("Aaaa");
        gameManager.SetFalseSlowTimeSomeObjects += () =>
        { 
            Despawn();
        };
    }
    private void OnDisable()
    {
        Debug.Log("bbb");
        gameManager.SetFalseSlowTimeSomeObjects -= () =>
        {
            Despawn();
        };
    }
    private void Despawn()
    {
        try
        {
            gameObject.SetActive(false);
        }
        catch(Exception e)
        {

        }
    }
}
