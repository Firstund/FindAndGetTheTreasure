using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReflectTimer : MonoBehaviour
{
    private GameManager gameManager  = null;
    private Reflect reflect = null;
    [SerializeField]
    private Image fillImage = null;
    [SerializeField]
    private float time = 3f;
    private float timer = 0f;

    void Start()
    {
        gameManager = GameManager.Instance;

        reflect = FindObjectOfType<Reflect>();

        timer = time;
    }

    void Update()
    {
        if(timer > 0f)
        {
            timer -= Time.deltaTime;

            fillImage.fillAmount = timer / time;
        }
        else
        {
            reflect.canShoot = false;
            reflect.canSettingAngle = false;
            gameManager.SlowTimeSomeObjects = false;

            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        timer = time;
    }
}
