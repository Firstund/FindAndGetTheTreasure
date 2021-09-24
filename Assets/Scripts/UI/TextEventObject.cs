using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEventObject : MonoBehaviour
{
    private Texts textsScript = null;

    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private Transform moveTargetPos = null;
    [SerializeField]
    private bool setCanNextAtThisEventEnd = false;

    private void Start()
    {
        textsScript = transform.parent.transform.parent.transform.parent.GetComponentInChildren<Texts>();
    }
    // 오브젝트가 이동하는 코드
}
