using UnityEngine;
using SexyDu.ContainerSystem;

namespace SexyDu.Sample
{
    public class TestContainerSystem : MonoBehaviour
    {
        private void Start()
        {
            // 아래와 같이 도커에 컨테이너를 연결한다.
            ContainerDocker.Dock<ISingleContainer>(new SingleContainer());
            ContainerDocker.Dock<IConvenientContainer>(new ConvenientContainer());

            /// ISingleContainer
            // 아래와 같이 컨테이너를 가져올 수 있다.
            ISingleContainer main = ContainerDocker.Bring<ISingleContainer>();
            // 컨테이너에 데이터가 적재되기 전 로그 출력
            LogSampleSingleBaggage(main.Get<SampleSingleBaggage>());
            // 컨테이너에 데이터 적재
            main.Bind(new SampleSingleBaggage());
            // 컨테이너에 데이터가 적재되기 전 로그 출력
            LogSampleSingleBaggage(main.Get<SampleSingleBaggage>());

            /// IConvenientContainer
            // 아래와 같이 컨테이너를 가져올 수 있다.
            IConvenientContainer conv = ContainerDocker.Bring<IConvenientContainer>();
            // 컨테이너에 데이터가 적재되기 전 로그 출력
            LogBaggage(conv.Get<int>());
            // 컨테이너에 데이터 적재 (int형)
            conv.Bind(3);
            // 컨테이너에 데이터가 적재되기 전 로그 출력
            LogBaggage(conv.Get<int>());
        }

        [SerializeField] private int integerBaggage;
        private void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 100f, 100f), "Bind SampleSingleBaggage"))
            {
                ContainerDocker.Bring<ISingleContainer>().Bind(new SampleSingleBaggage());
            }

            if (GUI.Button(new Rect(100f, 0f, 100f, 100f), "Unbind SampleSingleBaggage"))
            {
                ContainerDocker.Bring<ISingleContainer>().Unbind<SampleSingleBaggage>();
            }

            if (GUI.Button(new Rect(200f, 0f, 100f, 100f), "Print SampleSingleBaggage"))
            {
                LogSampleSingleBaggage(ContainerDocker.Bring<ISingleContainer>().Get<SampleSingleBaggage>());
            }

            if (GUI.Button(new Rect(0f, 100f, 100f, 100f), "Bind int"))
            {
                ContainerDocker.Bring<IConvenientContainer>().Bind(integerBaggage);
            }

            if (GUI.Button(new Rect(100f, 100f, 100f, 100f), "Unbind int"))
            {
                ContainerDocker.Bring<IConvenientContainer>().Unbind<int>();
            }

            if (GUI.Button(new Rect(200f, 100f, 100f, 100f), "Print int"))
            {
                LogBaggage(ContainerDocker.Bring<IConvenientContainer>().Get<int>());
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
