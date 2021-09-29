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
        public Transform cameraFollowPos; // 해당 대화를 진행할 때 카메라가 보고있을 장소, 딱히 변하지 않으면 그냥 비워둔다.
        public string contents;
        public bool isPlayerSay;
        public bool canNextTalk; // 다른 이벤트들이 없을 시 기본적으로 체크해두고, 다른 이벤트가 따로 있다면, 그 이벤트가 끝났을 때 canNextTalk가 true가 되도록 한다.
    }
}
