using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingByPlayerScript : MonoBehaviour
{
    private GameManager gameManager = null;
    private Image image = null;
    private float posX = 0f;
    [SerializeField]
    private float speed = 0.1f;

    void Start()
    {
        gameManager = GameManager.Instance;

        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!gameManager.SlowTimeSomeObjects)
        {
            posX += gameManager.player.Rigid.velocity.x * speed * Time.deltaTime;

            image.material.mainTextureOffset = new Vector2(posX, 0f);
        }

        // transform.position = (Vector2)Camera.main.gameObject.transform.position;
    }
}