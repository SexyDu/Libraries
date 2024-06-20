#define USE_CONTAINERDOCKER

using UnityEngine;
using SexyDu.Tool;
#if USE_CONTAINERDOCKER
using SexyDu.ContainerSystem;
#endif

namespace SexyDu.Sample
{
    public class TestOnFrame : MonoBehaviour, IOnFrameTarget
    {
#if USE_CONTAINERDOCKER
        private void Awake()
        {
            ContainerDocker.Dock<IOnFrameContainer>(new OnFrameContainer());
        }

        private void OnDestroy()
        {
            ContainerDocker.Bring<IOnFrameContainer>().Dispose();
            ContainerDocker.Undock<IOnFrameContainer>();
        }
#endif

        private void OnEnable()
        {
#if USE_CONTAINERDOCKER
            ContainerDocker.Bring<IOnFrameContainer>().Subscribe(this);
#else
            OnFrameSingleton.Ins.Subscribe(this);
#endif
        }

        private void OnDisable()
        {
#if USE_CONTAINERDOCKER
            ContainerDocker.Bring<IOnFrameContainer>().Unsubscribe(this);
#else
            OnFrameSingleton.Ins.Unsubscribe(this);
#endif
        }

        public void OnFrame()
        {
            Debug.LogFormat("on frame touch count : {0}", Input.touchCount);
        }
    }
}