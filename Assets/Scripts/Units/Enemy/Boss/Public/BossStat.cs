using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    private StageManager stageManager = null;

    private Animator anim = null;
    private SpriteRenderer spriteRenderer = null;

    private List<IWhenBossDead> whenBossDeads = new List<IWhenBossDead>();

    [SerializeField]
    private float hp = 10f;
    public float Hp
    {
        get { return hp; }
        set { hp = value; }
    }
    private float firstHp = 0f;

    // 보스는 스킬기반으로 움직이기에 따로 기본 공격력을 두지 않는다.

    [SerializeField]
    private float dp = 0f;
    public float Dp
    {
        get { return dp; }
        set { dp = value; }
    }
    private float firstDp = 0f;

    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    private bool isNothurtMode = false;
    public bool IsNothurtMode
    {
        get { return isNothurtMode; }
        set { isNothurtMode = value; }
    }

    private bool alreadyDead = false;

    [SerializeField]
    private string deadAnimTrigger = "";

    private Vector2 currentPosition = Vector2.zero;
    public Vector2 CurrentPosition
    {
        get { return currentPosition; }
    }

    private void Awake()
    {
        stageManager = StageManager.Instance;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        whenBossDeads = GetComponents<IWhenBossDead>().ToList();

        firstHp = hp;
        firstDp = dp;

        EventManager.StartListening("WhenBossIsDead", () =>
        {
            foreach (var item in whenBossDeads)
            {
                item.DoWhenBossDead();
            }

            anim.SetTrigger(deadAnimTrigger);
        });
    }
    private void Start()
    {
        stageManager.Enemys.Add(gameObject);
    }
    private void OnEnable()
    {
        isDead = false;
        alreadyDead = false;

        hp = firstHp;
        dp = firstDp;
    }
    void Update()
    {
        currentPosition = transform.position;

        if (isDead && !alreadyDead)
        {
            alreadyDead = true;
            EventManager.TriggerEvent("WhenBossIsDead");
        }

        transform.position = currentPosition;
    }
    public void Hurt(float damage)
    {
        if (!isDead && !isNothurtMode)
        {
            float dm = (damage - dp);

            if (dm <= 0f)
            {
                dm = 0.5f;
            }

            hp -= dm;

            if (hp <= 0f)
            {
                isDead = true;
            }
            else
            {
                StartCoroutine(hurt());
            }
        }
    }
    private IEnumerator hurt()
    {
        Color color = new Color(1f, 0f, 1f, 0.5f);
        Color color_origin = new Color(1f, 1f, 1f, 1f);

        spriteRenderer.color = color;

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = color_origin;
    }
    private void Destroye()
    {
        stageManager.DespawnEnemy(gameObject);
    }
}
