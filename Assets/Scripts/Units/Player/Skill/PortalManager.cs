using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public int portalNum { get; private set; }
    
    private bool _portal0Spawned = false;
    public bool portal0Spawned
    {
        get { return _portal0Spawned; }
        set { _portal0Spawned = value; }
    }

    private bool _portal1Spawned = false;
    public bool portal1Spawned
    {
        get { return _portal1Spawned; }
        set { _portal1Spawned = value; }
    }

    public void portalNumPlus()
    {
        portalNum++;
    }

    public void portalNumMinus()
    {
        portalNum--;
    }
}
