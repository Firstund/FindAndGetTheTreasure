using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulleyBarScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [SerializeField]
    private Transform pos_L = null;
    [SerializeField]
    private Transform pos_R = null;
    private Vector2 pos = Vector2.zero;

    void Start()
    {
        spriteRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
        if (spriteRenderer.flipX)
        {
            pos = pos_L.localPosition;
            transform.localPosition = pos;
        }
        else
        {
            pos = pos_R.localPosition;
            transform.localPosition = pos;
        }
    }
}
