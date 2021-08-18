using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Text_Base : MonoBehaviour
{
    [Serializable]
    protected struct sText
    {
        public string contents;
        public bool isPlayerSay;

    }
}
