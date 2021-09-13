using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReflectTimer : MonoBehaviour
{
    [SerializeField]
    private Image fillImage = null;
    [SerializeField]
    private float time = 3f;
    private float timer = 0f;

    void Start()
    {
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
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        timer = time;
    }
}
