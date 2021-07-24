using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulleyScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    [SerializeField]
    private Transform pos_L = null;
    [SerializeField]
    private Transform pos_R = null;

    void Start()
    {
        spriteRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (spriteRenderer.flipX)
        {
            transform.position = pos_L.position;
        }
        else
        {
            transform.position = pos_R.position;
        }
    }
}
