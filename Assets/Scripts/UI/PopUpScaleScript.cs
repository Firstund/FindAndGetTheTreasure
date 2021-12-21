using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopUpScaleScript : MonoBehaviour
{
    protected GameManager gameManager = null;
    protected MenuManager menuManager = null;

    [SerializeField]
    protected float scaleSpeed = 1f;
    [SerializeField]
    protected float disableVector = 1f;

    protected Vector2 currentScale = Vector2.zero;
    protected Vector2 maxScale = Vector2.zero;

    protected bool onFadeIn = false;
    protected bool onFadeOut = false;

    public virtual void Awake()
    {
        gameManager = GameManager.Instance;
        menuManager = MenuManager.Instance;
    }
    protected void OnEnable() 
    {
        menuManager.OnShowMenu += () =>
        {
            OnShowMenu();
        };

        menuManager.OnHideMenu += () =>
        {
            OnHideMenu();
        };
    }
    public virtual void Start()
    {
        maxScale.x = transform.localScale.x;
        maxScale.y = transform.localScale.y;

        menuManager.SetMenu(gameObject);
    }
    public virtual void Update()
    {
        SetScale();
    }
    protected void OnDisable()
    {
        currentScale = Vector2.zero;
        transform.localScale = Vector2.zero;
        gameManager.StopTime(false);

        onFadeOut = false;

        menuManager.OnShowMenu -= () =>
        {
            OnShowMenu();
        };

        menuManager.OnHideMenu -= () =>
        {
            OnHideMenu();
        };
    }
    protected void OnHideMenu()
    {
        onFadeIn = false;
        onFadeOut = true;
    }

    protected void OnShowMenu()
    {
        Debug.Log(this);
        transform.localScale = Vector2.zero;

        onFadeIn = true;
        onFadeOut = false;
    }
    protected void SetScale()
    {
        currentScale = transform.localScale;

        if (currentScale.x < maxScale.x - disableVector && currentScale.y < maxScale.y - disableVector && onFadeIn && !onFadeOut)
        {
            currentScale = Vector2.Lerp(currentScale, maxScale, scaleSpeed * Time.deltaTime);
        }
        else if (currentScale.x > disableVector && currentScale.y > disableVector && !onFadeIn && onFadeOut)
        {
            currentScale = Vector2.Lerp(currentScale, Vector2.zero, scaleSpeed * Time.deltaTime);
        }
        else if (onFadeIn)
        {
            gameManager.StopTime(true);
        }
        else if (onFadeOut)
        {
            gameObject.SetActive(false);
        }

        transform.localScale = currentScale;
    }
}
