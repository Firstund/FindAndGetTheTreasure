using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShowSpriteWhenPlayerClose : MonoBehaviour
{
    private GameManager gameManager = null;

    private GameObject showIt = null;

    [SerializeField]
    private float showDistance = 0.5f;
    private float curDistance = 0f;

    private void Awake() 
    {
        gameManager = GameManager.Instance;
    }
    void Start()
    {
        showIt = transform.Find("ShowIt").gameObject;
        showIt.SetActive(false);
    }

    void Update()
    {
        ShowCheck();
    }

    private void ShowCheck()
    {
        curDistance = Vector2.Distance(transform.position, gameManager.player.currentPosition);

        if (curDistance <= showDistance)
        {
            showIt.SetActive(true);
        }
        else
        {
            showIt.SetActive(false);
        }
    }
}
