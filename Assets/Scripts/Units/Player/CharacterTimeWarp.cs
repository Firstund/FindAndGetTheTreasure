using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterTimeWarp : MonoBehaviour
{
    private GameManager gameManager = null;
    private SoundManager soundManager = null;
    private PlayerInput playerInput = null;
    private CharacterMove characterMove = null;
    private CharacterStat characterStat = null;
    private SpawnAfterImage spawnAfterImage = null;
    private SpriteRenderer spriteRenderer = null;
    private Rigidbody2D rigid = null;

    [SerializeField]
    private GameObject timeWarpPositionEffect = null;
    [SerializeField]
    private Image canGoBackTime = null;

    [SerializeField]
    private Vector2[] positions;
    private Vector2 timeWarpPosition = Vector2.zero;
    private Sprite[] sprites;
    private bool[] flipXes;

    [SerializeField]
    private float timeWarpDoTime = 1f;
    [SerializeField]
    private float timeWarpDelay = 5f;
    private float totalTime = 0f;
    private float timeWarpTimer = 0f;

    [Header("CanUseTimeWarpTime * 초당 저장하는 위치의 수")]
    [SerializeField]
    private int moveNum = 12;
    [Header("타임워프를 대쉬어택 후 몇초안에 사용해야 하는가")]
    [SerializeField]
    private int canUseTimeWarpTime = 3;
    private float moveBackTime = 0f;//
    private int pasteI_TotalTime = 0;
    private int currentMovePositionNum = 0;//
    private int totalMoveByTimeWarp = 0;//
    private int iTotalTime = 0;

    private bool _isTimeWarp = false;
    public bool isTimeWarp
    {
        get { return _isTimeWarp; }
    }
    private bool canTimeWarp = false;
    private bool canTimeWaprPositionSet = true;
    private bool canSpawnAfterImage = true;


    private void Awake()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;

        playerInput = GetComponent<PlayerInput>();
        characterMove = GetComponent<CharacterMove>();
        characterStat = GetComponent<CharacterStat>();
        spawnAfterImage = GetComponent<SpawnAfterImage>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        OriginSet();
    }
    private void OnEnable()
    {
        gameManager.player.WhenPlayerDead += CancelTimeWarp;
    }
    

    private void OriginSet()
    {
        positions = new Vector2[moveNum];
        sprites = new Sprite[moveNum];
        flipXes = new bool[moveNum];

        for (int i = 0; i < moveNum; i++)
        {
            positions[i] = transform.position;
            sprites[i] = spriteRenderer.sprite;
        }

        timeWarpPositionEffect.transform.position = positions[0];

        totalTime = 0f;
    }

    void Update()
    {
        if (characterMove.DashAttacking && canTimeWaprPositionSet)
        {
            OriginSet();

            timeWarpTimer = canUseTimeWarpTime;

            canTimeWaprPositionSet = false;
            canTimeWarp = true;

            positions[0] = transform.position;
            sprites[0] = spriteRenderer.sprite;
            flipXes[0] = spriteRenderer.flipX;
        }

        if (playerInput.timeWarp && canTimeWarp && !canTimeWaprPositionSet && characterMove.SkillUseSpValue.timeWarp < characterStat.sp)
        {
            characterStat.sp -= characterMove.SkillUseSpValue.timeWarp;
            canTimeWarp = false;
            _isTimeWarp = true;

            soundManager.SetMainBGMPitchByLerp(1, -7, timeWarpDoTime);
        }

        timeWarpPositionEffect.SetActive(canTimeWarp);
        canGoBackTime.gameObject.SetActive(canTimeWarp);
    }

    void FixedUpdate()
    {
        if (timeWarpTimer > 0f)
        {
            if (!isTimeWarp)
            {
                timeWarpTimer -= Time.fixedDeltaTime;

                moveBackTime += Time.fixedDeltaTime * (moveNum / canUseTimeWarpTime);
            }

            SetPositions();

            TimeWarp();

            SpawnAfterImage();
        }
        else
        {
            CancelTimeWarp();
        }
    }

    private void CancelTimeWarp()
    {
        canTimeWaprPositionSet = true;
        canTimeWarp = false;
        moveBackTime = 0f;
    }

    private void SetPositions()
    {
        if (!isTimeWarp)
        {
            totalTime += Time.fixedDeltaTime * (moveNum / canUseTimeWarpTime);

            iTotalTime = (int)((totalTime) % moveNum);

            bool isDifferent = false;
            isDifferent = (iTotalTime != pasteI_TotalTime);

            if (isDifferent)
            {
                for (int i = iTotalTime; i >= 0; i--)
                {
                    if (i > 0)
                    {
                        positions[i] = positions[i - 1];
                        sprites[i] = sprites[i - 1];
                        flipXes[i] = flipXes[i - 1];
                    }
                }

                positions[0] = transform.position;
                sprites[0] = spriteRenderer.sprite;
                flipXes[0] = spriteRenderer.flipX;
            }

            pasteI_TotalTime = iTotalTime;

            canGoBackTime.fillAmount = 1 - totalTime / moveNum;
        }
    }
    public void TimeWarp()
    {
        if (isTimeWarp)
        {
            transform.DOMove(positions[currentMovePositionNum], timeWarpDoTime / moveNum * 2).SetEase(Ease.InQuad);
            spriteRenderer.sprite = sprites[currentMovePositionNum];
            spriteRenderer.flipX = flipXes[currentMovePositionNum];

            float distance = Vector2.Distance(transform.position, positions[currentMovePositionNum]);

            rigid.velocity = new Vector2(rigid.velocity.x, 0f);

            if (distance <= 0.5f && currentMovePositionNum > iTotalTime - 1)
            {
                currentMovePositionNum = 0;
                totalMoveByTimeWarp = 0;

                _isTimeWarp = false;

                soundManager.SetMainBGMPitch(1f);

                Invoke("CanTimeWarpSet", timeWarpDelay);
            }
            else if (distance <= 0.5f && currentMovePositionNum <= iTotalTime - 1)
            {
                currentMovePositionNum++;
                totalMoveByTimeWarp++;
            }
        }
    }
    private void CanTimeWarpSet()
    {
        moveBackTime = 0f;
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
