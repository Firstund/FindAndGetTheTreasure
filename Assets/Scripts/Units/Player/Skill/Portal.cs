using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private int thisPortalNum = 0;

    private PortalManager portalManager = null;
    private Animator anim = null;
    [SerializeField]
    private Transform _otherPortalPosition = null;
    public Transform otherPortalPosition
    {
        get { return _otherPortalPosition; }
        set { _otherPortalPosition = value; }
    }

    public Vector2 targetPosition
    {
        get { return _otherPortalPosition.position; }
    }

    void Start()
    {
        portalManager = FindObjectOfType<PortalManager>();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (thisPortalNum % 2 == 0)
        {
            anim.Play("Portal_B");
        }
        else
        {
            anim.Play("Portal_R");
        }
    }
}
