using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TextEventObject_Base : MonoBehaviour
{
    [Serializable]
    protected struct SEventObjData
    {
        public float moveSpeed;
        public Vector2 moveTargetPos; // 이동하지 않는다면, 모두 0으로 설정해둔다.
        public string animName; // Play할 애니메이션의 이름, 없으면 비워둔다.
        public bool setCanNextTalkByMoves; // 이 오브젝트가 도착지에 도달했을 때 OnEventEnd함수를 실행할 것인가
        public bool setCanNextAtThisEventEnds; // 이 오브젝트의 OnEventEnd때 다음 대화로 넘어갈 수 있도록 할것인가
        public bool flipX;
    }

}
