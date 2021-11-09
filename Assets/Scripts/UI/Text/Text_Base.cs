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
        public bool cantSkipText; // 이 값이 true면 대화 스킵 불가능
        public bool canNextTalk; // 다른 이벤트들이 없을 시 기본적으로 체크해두고, 다른 이벤트가 따로 있다면, 그 이벤트가 끝났을 때 canNextTalk가 true가 되도록 한다.
        public bool startOtherTextEventOnPlayerSkip; // 이 값이 true면, 플레이어가 해당 대화를 스킵하려 할 때, 다른 텍스트 이벤트를 진행한다.
        public int otherTextEventIndex; // startOtherTextEventOnPlayerSkip가 true일 때, 불러올 텍스트 이벤트의 index, false면 그냥 냅둬도 된다.
        public string func; //해당 대화를 진행할 때 실행할 함수 ex) GameManager.Start -> GameManager 클래스의 Start함수 실행 / Start -> 로컬(Texts)의 Start함수 실행
    }
}
