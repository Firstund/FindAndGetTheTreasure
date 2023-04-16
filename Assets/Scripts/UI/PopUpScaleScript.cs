using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class PopUpScaleScript : MonoBehaviour
{
    protected GameManager gameManager = null;
    protected MenuManager menuManager = null;
    protected CanvasGroup canvasGroup = null;

    protected float fadeTime = 2.3f;
    protected float fadeStartTime = 0f;
    protected float scaleSpeed = 5f;

    [SerializeField]
    protected bool isMenu = false;

    protected Vector2 currentScale = Vector2.zero;
    protected Vector2 targetScale = Vector2.zero;

    protected bool fadeStart = false;
    protected bool onFadeIn = false;
    protected bool onFadeOut = false;

    public virtual void Awake()
    {
        gameManager = GameManager.Instance;
        menuManager = MenuManager.Instance;

        canvasGroup = GetComponent<CanvasGroup>();
    }
    public virtual void Start()
    {
        targetScale.x = transform.localScale.x;
        targetScale.y = transform.localScale.y;

        if (isMenu)
        {
            menuManager.SetMenu(gameObject);
        }
    }
    public virtual void Update()
    {
        CheckTime();

        if (fadeStart)
        {
            SetScale();
            SetAlpha();
        }

    }
    public virtual void OnDisable()
    {
        currentScale = Vector2.zero;
        transform.localScale = Vector2.zero;

        onFadeOut = false;
    }
    public void OnHidePopUp()
    {
        onFadeIn = false;
        onFadeOut = true;

        fadeStart = true;
        UtilTimer.Instance.AddAndStartTimerDict(gameObject, 0f, fadeTime, scaleSpeed, () =>
        {
            fadeStart = false;
            gameObject.SetActive(false);
            if (!gameManager.TextEventPlaying)
            {
                gameManager.StopTime(false);
            }
        }, fadeStartTime);
    }

    public void OnShowPopUp()
    {
        transform.localScale = Vector2.zero;

        onFadeIn = true;
        onFadeOut = false;

        fadeStart = true;

        gameManager.StopTime(true);
        UtilTimer.Instance.AddAndStartTimerDict(gameObject, 0f, fadeTime, scaleSpeed, () =>
        {
            transform.localScale = targetScale;
            canvasGroup.alpha = 1f;

            fadeStart = false;
        }, fadeStartTime);
    }
    public void OnClickCloseBtn()
    {
        if (isMenu)
        {
            menuManager.HideMenu();
        }
        else
        {
            OnHidePopUp();
        }
    }
    private void CheckTime()
    {

    }
    protected void SetScale()
    {
        currentScale = transform.localScale;

        if (onFadeIn && !onFadeOut)
        {
            currentScale = targetScale * EffectGraph(UtilTimer.Instance.TimerDict[gameObject].currentTime);
        }
        else if (!onFadeIn && onFadeOut)
        {
            currentScale = targetScale * EffectGraph(fadeTime - (UtilTimer.Instance.TimerDict[gameObject].currentTime));
        }

        transform.localScale = currentScale;
    }
    protected void SetAlpha()
    {
        if (onFadeIn && !onFadeOut)
        {
            canvasGroup.alpha = EffectGraph(UtilTimer.Instance.TimerDict[gameObject].currentTime);
        }
        else if (!onFadeIn && onFadeOut)
        {
            canvasGroup.alpha = EffectGraph(fadeTime - (UtilTimer.Instance.TimerDict[gameObject].currentTime));
        }
    }
    private float EffectGraph(float x)
    {
        // x += 0.7f; // 그래프 x값 조정용
        // 이펙트에 쓰일 그래프
        return -0.25f * Mathf.Pow(x, 4) + 0.7f * Mathf.Pow(x, 3) - 0.69f * x + 1.05f;
    }
}
