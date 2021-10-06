using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanGoBackScript : MonoBehaviour
{
    private Image image = null;
    void Start()
    {
        image = GetComponent<Image>();

        image.color = new Vector4(1f, 0f, 0f, image.color.a);
    }
    void Update()
    {
        image.color = new Vector4(1f - image.fillAmount, image.fillAmount, 0f, image.color.a);
    }
}
