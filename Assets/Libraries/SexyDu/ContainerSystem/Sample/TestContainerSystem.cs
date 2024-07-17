/// ContainerDocker 클래스 사용 여부 플래그
/// * 사용하지 않는 경우 singleton 독자로 동작한다
#define USE_CONTAINERDOCKER

using UnityEngine;
using SexyDu.ContainerSystem;

namespace SexyDu.Sample
{
    public class TestContainerSystem : MonoBehaviour
    {
        // 
#if USE_CONTAINERDOCKER
        private ISingleContainer singleContainer
            => ContainerDocker.LazyBring<ISingleContainer>();
        private IConvenientContainer convenientContainer
            => ContainerDocker.LazyBring<IConvenientContainer>();
#else
        private ISingleContainer singleContainer => SingleContainerSingleton.Ins;
        private IConvenientContainer convenientContainer => ConvenientContainerSingleton.Ins;
#endif

        [SerializeField] private bool onStartTesting = false;
        private void Start()
        {
            if (onStartTesting)
            {
                InitializeContainers();

                TestContainer();
            }
        }

        private void InitializeContainers()
        {
            // 컨테이너 도커 초기설정
            ContainerDocker.Initialize();

            // 아래와 같이 도커에 컨테이너를 연결한다.
            ContainerDocker.Dock<ISingleContainer>(new SingleContainer());
            ContainerDocker.Dock<IConvenientContainer>(new ConvenientContainer());
        }

        private void TestContainer()
        {
            /// ISingleContainer
            // 컨테이너에 데이터가 적재되기 전 로그 출력
            LogSampleSingleBaggage(singleContainer.Get<SampleSingleBaggage>());
            // 컨테이너에 데이터 적재
            singleContainer.Bind(new SampleSingleBaggage());
            // 컨테이너에 데이터가 적재되기 전 로그 출력
            LogSampleSingleBaggage(singleContainer.Get<SampleSingleBaggage>());

            /// IConvenientContainer
            // 컨테이너에 데이터가 적재되기 전 로그 출력
            LogBaggage(convenientContainer.Get<int>());
            // 컨테이너에 데이터 적재 (int형)
            convenientContainer.Bind(3);
            // 컨테이너에 데이터가 적재되기 전 로그 출력
            LogBaggage(convenientContainer.Get<int>());
        }

        [SerializeField] private int integerBaggage;
        private void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 100f, 100f), "Bind SampleSingleBaggage"))
            {
                singleContainer.Bind(new SampleSingleBaggage());
            }

            if (GUI.Button(new Rect(100f, 0f, 100f, 100f), "Unbind SampleSingleBaggage"))
            {
                singleContainer.Unbind<SampleSingleBaggage>();
            }

            if (GUI.Button(new Rect(200f, 0f, 100f, 100f), "Print SampleSingleBaggage"))
            {
                LogSampleSingleBaggage(singleContainer.Get<SampleSingleBaggage>());
            }

            if (GUI.Button(new Rect(0f, 100f, 100f, 100f), "Bind int"))
            {
                convenientContainer.Bind(integerBaggage);
            }

            if (GUI.Button(new Rect(100f, 100f, 100f, 100f), "Unbind int"))
            {
                convenientContainer.Unbind<int>();
            }

            if (GUI.Button(new Rect(200f, 100f, 100f, 100f), "Print int"))
            {
                LogBaggage(convenientContainer.Get<int>());
            }

            if (GUI.Button(new Rect(0f, 200f, 100f, 100f), ""))
            {
                InitializeContainers();
            }

            if (GUI.Button(new Rect(100f, 200f, 100f, 100f), ""))
            {
                TestContainer();
            }

            if (GUI.Button(new Rect(), ""))
            {

            }
        }

        private void LogSampleSingleBaggage(SampleSingleBaggage baggage)
        {
            if (baggage == null)
                Debug.Log("SampleSingleBaggage is null");
            else
                Debug.LogFormat("SampleSingleBaggage.A : {0}, .B : {1}",
                    baggage.A, baggage.B);
        }

        public void LogBaggage(object obj)
        {
            if (obj == null)
            {
                Debug.Log("Baggage is null");
            }
            else
            {
                Debug.LogFormat("Baggage<{0}> : {1}", obj.GetType(), obj);
            }
        }
    }
}