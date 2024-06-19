# SexyDu Libraries For Unity
* 개발하며 유용하다 느끼는 기능을 라이브러리화
* 천천히 하나하나 업로드 진행 중...

## ContainerSystem
* 가볍게 사용할 목적으로 개발한 컨테이너 시스템

## MonoHelper
* MonoBehaviour를 사용하지 않고도 Coroutine을 구동시킬 수 있도록 도와주는 툴

## OnFrameContainer (진행 중)
* 매 프레임 수행해야할 작업을 MonoBehaviour 내에서 Update/FixedUpdate 또는 Coroutine을 사용하지 않고 컨테이너에 적재하여 수행 처리
* (적재된 OnFrameTarget이 존재하는 경우) OnFrameContainer에서만 (MonoHelper를 활용해) Coroutine을 구동하고 적재된 타겟이 수행할 수 있도록 구현
* 장점
  - 기능마다 Update/FixedUpdate 또는 Coroutine을 구동할 필요가 없어 비용상 효율적
  - 매 프레임 구동하는 기능을 찾기 용이하여 유지보수에 도움

## Touch / UI 서포트 목적의 추가 기능들
