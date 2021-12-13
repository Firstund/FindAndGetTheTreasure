using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarp : BossSkillBase
{
    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private bool isWarpToPlayer = false;

    [Header("isWarpToPlayer와 함꼐 true인 상태라면, 무조건 플레이어한테 워프한다.")]
    [SerializeField]
    private bool isWarpRandom = false;
    [SerializeField]
    private Vector2 randWarpMin = Vector2.zero;
    [SerializeField]
    private Vector2 randWarpMax = Vector2.zero;

    [Header("isWarpToPlayer와 isWarpRandom이 둘 다 false일 때 이 보스 캐릭터가 워프하는 값을 나타낸다.")]
    [SerializeField]
    private Vector2 targetPos = Vector2.zero;

    [SerializeField]
    private LayerMask whatIsWall;

    [SerializeField]
    private float fadeOutSpeed = 0f;
    [SerializeField]
    private float fadeInSpeed = 0f;

    [Header("이 유닛이 사라진 후 Warp한다음 다시 FadeIn할 때 실행할 애니메이션의 트리거, 없으면 비워둔다.")]
    [SerializeField]
    private string fadeInTrigger = "";

    private bool fadeOut = false;
    private bool fadeIn = false;


    private void Awake()
    {
        DoAwake();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (fadeIn && spriteRenderer.color.a < 1f)
        {
            spriteRenderer.color = Vector4.Lerp(spriteRenderer.color, new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f), Time.deltaTime * fadeInSpeed);

            if (spriteRenderer.color.a >= 1f)
            {
                fadeIn = false;

                spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

                if (fadeInTrigger == "")
                {
                    doThisSkill = false;

                    bossStatus.DoCurrentSkillSuccess();
                }
            }
        }

        if (fadeOut && spriteRenderer.color.a > 0f)
        {
            spriteRenderer.color = Vector4.Lerp(spriteRenderer.color, new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f), Time.deltaTime * fadeOutSpeed);

            if (spriteRenderer.color.a <= 0f)
            {
                fadeOut = false;

                spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
                DoWarp();
            }
        }
    }
    public override void DoSkill()
    {
        if (doThisSkill)
        {
            base.DoSkill();
            PlaySound();
            
            StartFadeOut();
        }
    }
    public override string GetSkillScriptName()
    {
        return "BossWarp";
    }
    private void StartFadeOut()
    {
        bossStat.IsNothurtMode = true;
        fadeIn = false;
        fadeOut = true;
    }
    private void StartFadeIn()
    {
        fadeOut = false;
        fadeIn = true;

        if (fadeInTrigger != "")
        {
            bossStatus.Anim.SetTrigger(fadeInTrigger);
        }
    }
    private void FadeInSuccess()
    {
        bossStat.IsNothurtMode = false;
        fadeOut = false;
        fadeIn = false;

        doThisSkill = false;

        bossStatus.DoCurrentSkillSuccess();

        bossStatus.Anim.SetTrigger("Idle");
    }
    private void DoWarp()
    {
        if (isWarpToPlayer)
        {
            transform.position = gameManager.player.transform.position;

            StartFadeIn();

            return;
        }
        else if (isWarpRandom)
        {
            Ray2D ray = new Ray2D();
            RaycastHit2D hit = new RaycastHit2D();
            float distance = 0f;

            targetPos.x = Random.Range(randWarpMin.x, randWarpMax.x);

            if (bossStatus.IsAirUnit)
            {
                targetPos.y = Random.Range(randWarpMin.y, randWarpMax.y);
            }
            else
            {
                targetPos.y = 0f;
            }

            ray.origin = bossStat.CurrentPosition;
            ray.direction = targetPos.normalized;

            distance = Vector2.Distance(bossStat.CurrentPosition, targetPos);

            // hit = Physics2D.Raycast(ray.origin, ray.direction, randWarpMax.x + randWarpMax.y, whatIsWall);
            if (randWarpMax.x > randWarpMax.y)
            {
                hit = Physics2D.CircleCast(ray.origin, 0.5f, ray.direction, randWarpMax.x, whatIsWall);
            }
            else
            {
                hit = Physics2D.CircleCast(ray.origin, 0.5f, ray.direction, randWarpMax.y, whatIsWall);
            }

            if (hit)
            {
                transform.position = hit.point;
            }
            else
            {
                transform.position += (Vector3)targetPos;
            }
        }

        StartFadeIn();
    }
}
