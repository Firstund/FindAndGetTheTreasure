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
        public List<GameObject> doThisTextEvents; // 이 이벤트가 실행될 때 실행될 다른 오브젝트의 동작들, 없으면 비워둔다.
        public string animName; // Play할 애니메이션의 이름, 비워두면 alpha값이 0이 된다
        public bool setCanNextTalkByMoves; // 이 오브젝트가 도착지에 도달했을 때 OnEventEnd함수를 실행할 것인가
        public bool setCanNextAtThisEventEnds; // 이 오브젝트의 OnEventEnd때 다음 대화로 넘어갈 수 있도록 할것인가
        public bool flipX; // isStartAtPlayer가 true면 이 값을 조정하지 않아도 된다.
    }
}
