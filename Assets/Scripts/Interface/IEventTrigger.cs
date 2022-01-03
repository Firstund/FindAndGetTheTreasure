public interface IEventTrigger
{
    void DoEvent();
    void DoEventWhenDoEventFalse(); // EventTriggerArea 스크립트의 DoEvent변수가 False가 됐을 때 한번 실행하는 함수
}
