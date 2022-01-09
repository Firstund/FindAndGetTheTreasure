using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private GameObject menuObj = null; // 씬을 로드했을 때 그곳의 Menu Object가 Start함수에서 SetMenu함수를 이용해 이곳에 자기 자신을 넣는다.
    private static MenuManager instance = null;
    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();

                if (instance == null)
                {
                    GameObject temp = new GameObject("MenuManager");
                    instance = temp.AddComponent<MenuManager>();
                }
            }

            return instance;
        }
    }

    [SerializeField]
    private KeyCode spawnMenuKey;

    private bool menuWasShow = false;
    private void Update()
    {
        if (Input.GetKeyUp(spawnMenuKey))
        {
            if (menuWasShow)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }
        }
    }
    public void SetMenu(GameObject menu)
    {
        if (menu != null)
        {
            menuObj = menu;
        }
    }
    public void ShowMenu()
    {
        menuObj.SetActive(true);

        EventManager.TriggerEvent("OnShowMenu");

        menuWasShow = true;
    }
    public void HideMenu()
    {
        //Hide할 땐 menuObj에서 직접 SetActive(false)를 실행한다.
        EventManager.TriggerEvent("OnHideMenu");

        menuWasShow = false;
    }
}
