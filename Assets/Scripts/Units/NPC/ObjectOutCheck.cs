using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutCheck : MonoBehaviour
{
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
            OnOutOfCamera();
        }
        else
        {
            OnInOfCamera();
        }
    }
    private void OnInOfCamera()
    {
        isOutCamera = false;
    }
    private void OnOutOfCamera()
    {
        isOutCamera = true;
    }
}
