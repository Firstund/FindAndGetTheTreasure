using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutCheck : MonoBehaviour
{
    [SerializeField]
    private float isOutTime = 1f;
    private float isOutTimer = 0f;

    private bool isOutCamera = false;
    public bool IsOutCamera
    {
        get { return isOutCamera; }
    }
    private void Update()
    {
        OutCheck();
    }
    private void OutCheck()
    {
        Vector3 viewPortPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPortPos.x < 0f || viewPortPos.x > 1f || viewPortPos.y < 0f || viewPortPos.y > 1f)
        {
            if (isOutTimer > 0f)
            {
                isOutTimer -= Time.deltaTime;

                if(isOutTimer <= 0f)
                {
                    isOutCamera = true;
                }
            }
            else
            {
                OnOutOfCamera();
            }
        }
        else
        {
            OnInOfCamera();
        }
    }
    private void OnInOfCamera()
    {
        isOutCamera = false;
        isOutTimer = 0f;
    }
    private void OnOutOfCamera()
    {
        // isOutCamera = true;
        isOutTimer = isOutTime;
    }
}
