using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingScript : MonoBehaviour
{
    private GameManager gameManager = null;
    private Image image = null;
    [SerializeField]
    private float speed = 0.5f;
    private float totalTime = 0f;

    void Start()
    {
        gameManager = GameManager.Instance;

        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!gameManager.SlowTimeSomeObjects)
        {
            totalTime += Time.deltaTime;

            image.material.mainTextureOffset = new Vector2(totalTime * speed, 0f);
        }

        // transform.position = (Vector2)Camera.main.gameObject.transform.position;
    }
}
