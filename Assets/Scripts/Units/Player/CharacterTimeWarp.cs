using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterTimeWarp : MonoBehaviour
{
    private PlayerInput playerInput = null;
    private SpawnAfterImage spawnAfterImage = null;

    private Rigidbody2D rigid = null;

    [SerializeField]
    private Vector2[] positions = new Vector2[12];
    [SerializeField]
    private float timeWarpDoTime = 1f;
    [SerializeField]
    private float timeWarpDelay = 5f;
    private float totalTime = 0f;
    [SerializeField]
    private int moveNum = 12;
    private int pasteI_TotalTime = 0;
    private int currentMovePositionNum = 0;
    private int iTotalTime = 0;
    private bool isTimeWarp = false;
    private bool canTimeWarp = true;
    private bool canSpawnAfterImage = true;
    void Start()
    {
        positions = new Vector2[moveNum];
        
        playerInput = GetComponent<PlayerInput>();
        spawnAfterImage = GetComponent<SpawnAfterImage>();
        rigid = GetComponent<Rigidbody2D>();

        for (int i = 0; i < 3; i++)
        {
            positions[i] = transform.position;
        }
    }
    void Update()
    {
        if (playerInput.timeWarp && canTimeWarp)
        {
            isTimeWarp = playerInput.timeWarp;
        }
    }

    void FixedUpdate()
    {
        SetPositions();
        SpawnAfterImage();

        TimeWarp();

    }

    private void SetPositions()
    {
        if (!isTimeWarp)
        {
            totalTime += Time.fixedDeltaTime;

            iTotalTime = (int)((totalTime * 12f) % moveNum);

            bool isDifferent = false;
            isDifferent = (iTotalTime != pasteI_TotalTime);

            if (isDifferent)
            {
                for (int i = iTotalTime; i >= 0 ; i--)
                {
                    if (i > 0)
                    {
                        positions[i] = positions[i - 1];
                    }
                }

                positions[0] = transform.position;

                // if (iTotalTime >= 2)
                // {
                //     positions[2] = positions[1];
                //     positions[1] = positions[0];
                //     positions[0] = transform.position;
                // }
                // else if (iTotalTime >= 1)
                // {
                //     positions[1] = positions[0];
                //     positions[0] = transform.position;
                // }
                // else if (iTotalTime >= 0)
                // {
                //     positions[0] = transform.position;
                // }
            }

            pasteI_TotalTime = iTotalTime;
        }
    }
    public void TimeWarp()
    {
        if (isTimeWarp)
        {
            canTimeWarp = false;

            transform.DOMove(positions[currentMovePositionNum], timeWarpDoTime / moveNum).SetEase(Ease.InQuad);

            float distance = Vector2.Distance(transform.position, positions[currentMovePositionNum]);

            rigid.velocity = new Vector2(rigid.velocity.x, 0f);

            if (distance <= 0.5f && currentMovePositionNum >= moveNum - 1)
            {
                Debug.Log("aa");
                currentMovePositionNum = 0;
                isTimeWarp = false;
                Invoke("CanTimeWarpSet", timeWarpDelay);
            }
            else if (distance <= 0.5f && currentMovePositionNum < moveNum - 1)
            {
                Debug.Log("bb");
                currentMovePositionNum++;
            }
        }
    }
    private void CanTimeWarpSet()
    {
        canTimeWarp = true;
    }
    private void SpawnAfterImage()
    {
        if (isTimeWarp && canSpawnAfterImage)
        {
            float spawnAfterImageDelay = Random.Range(spawnAfterImage.spawnAfterImageDelayMinimum, spawnAfterImage.spawnAfterImageDelayMaximum);
            spawnAfterImage.SetAfterImage();
            canSpawnAfterImage = false;

            Invoke("SpawnAfterImageByDashRe", spawnAfterImageDelay);
        }
    }
    private void SpawnAfterImageByDashRe()
    {
        canSpawnAfterImage = true;
    }
}
