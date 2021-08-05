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
    private Vector2[] positions = new Vector2[3];
    [SerializeField]
    private float timeWarpDoTime = 1f;
    [SerializeField]
    private float timeWarpDelay = 5f;
    private float totalTime = 0f;
    private int pasteI_TotalTime = 0;
    private bool isTimeWarp = false;
    private bool canTimeWarp = true;
    private bool canSpawnAfterImage = true;
    void Start()
    {
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

            int i_totalTime = (int)(totalTime % 3f);

            bool isDifferent = false;
            isDifferent = (i_totalTime != pasteI_TotalTime);

            if (isDifferent)
            {
                if (i_totalTime >= 2)
                {
                    positions[2] = positions[1];
                    positions[1] = positions[0];
                    positions[0] = transform.position;
                }
                else if (i_totalTime >= 1)
                {
                    positions[1] = positions[0];
                    positions[0] = transform.position;
                }
                else if (i_totalTime >= 0)
                {
                    positions[0] = transform.position;
                }
            }

            pasteI_TotalTime = i_totalTime;
        }
    }
    public void TimeWarp()
    {
        if (isTimeWarp)
        {
            canTimeWarp = false;

            transform.DOMove(positions[2], timeWarpDoTime).SetEase(Ease.InQuad);

            float distance = Vector2.Distance(transform.position, positions[2]);

            rigid.velocity = new Vector2(rigid.velocity.x, 0f);

            if (distance <= 0.5f)
            {
                isTimeWarp = false;
                Invoke("CanTimeWarpSet", timeWarpDelay);
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
