using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AfterImage : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTime = 0.7f;
    private SpriteRenderer spriteRenderer = null;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetSprite(Sprite sprite, bool flip, Vector3 position)
    {
        transform.position = position;
        spriteRenderer.flipX = flip;
        spriteRenderer.color = new Color(10f,1f,1f,1f);
        spriteRenderer.sprite = sprite;

        spriteRenderer.DOFade(0, fadeOutTime).OnComplete(()=>{
            gameObject.SetActive(false);
        });
    }
}
