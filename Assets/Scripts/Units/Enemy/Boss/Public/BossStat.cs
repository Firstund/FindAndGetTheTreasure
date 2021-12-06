using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStat : MonoBehaviour
{
    private StageManager stageManager = null;

    private Animator anim = null;
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private float hp = 10f;
    public float Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    // 보스는 스킬기반으로 움직이기에 따로 기본 공격력을 두지 않는다.

    [SerializeField]
    private float dp = 0f;
    public float Dp
    {
        get { return dp; }
        set { dp = value; }
    }

    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    private bool alreadyDead = false;

    [SerializeField]
    private string deadAnimTrigger = "";

    public event Action WhenIsDead;

    private void Awake()
    {
        stageManager = StageManager.Instance;

        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        WhenIsDead = () =>
        {
            anim.SetTrigger(deadAnimTrigger);
        };
    }
    private void Start() 
    {
        stageManager.Enemys.Add(gameObject);
    }
    void Update()
    {
        if (isDead && !alreadyDead)
        {
            alreadyDead = true;

            WhenIsDead();
        }
    }
    public void Hurt(float damage)
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
        Destroy(gameObject);
    }
}
