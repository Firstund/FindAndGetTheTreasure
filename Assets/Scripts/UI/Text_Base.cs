using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Text_Base : MonoBehaviour
{
    [Serializable]
    protected struct SText
    {
        public Sprite LSprite;
        public Sprite RSprite;
        public string contents;
        public bool isPlayerSay;
        public bool gameClearAtEnd; // 대화가 끝났을 때 게임클리어 처리를 할것인가.
        public bool gameOverAtEnd; // 대화가 끝났을 때 게임오버 처리를 할것인가.

    }
}
