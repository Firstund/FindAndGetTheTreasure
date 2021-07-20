using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAfterImage : MonoBehaviour
{
    [SerializeField]
    private GameObject afterImageObj = null;
    [SerializeField]
    private Transform afterImageSpawnPosition = null;
    [SerializeField]
    private float _spawnAfterImageDelayMinimum = 0.1f;
    public float spawnAfterImageDelayMinimum
    {
        get{return _spawnAfterImageDelayMinimum;}
    }
    [SerializeField]
    private float _spawnAfterImageDelayMaximum = 0.1f;
    public float spawnAfterImageDelayMaximum
    {
        get{return _spawnAfterImageDelayMaximum;}
    }


    private CharacterMove myMoveScript = null;
    SpriteRenderer spriteRenderer;

    private Stack<AfterImage> afterImageList;

    private void Start()
    {
        myMoveScript = GetComponent<CharacterMove>();
        spriteRenderer = myMoveScript.spriteRenderer;

        afterImageList = new Stack<AfterImage>();
    }

    public void SetAfterImage()
    {
        if (afterImageList.Count <= 0)
        {   
            AfterImage afterImage = Instantiate(afterImageObj, afterImageSpawnPosition).GetComponent<AfterImage>();

            afterImage.SetSprite(spriteRenderer.sprite, spriteRenderer.flipX, myMoveScript.currentPosition);
            
            afterImageList.Push(afterImage);
            
        }
        else
        {
            AfterImage _afterImage;

            _afterImage = afterImageList.Pop();
            _afterImage.SetSprite(spriteRenderer.sprite, spriteRenderer.flipX, myMoveScript.currentPosition);

            _afterImage.gameObject.SetActive(true);
        }
    }
}
