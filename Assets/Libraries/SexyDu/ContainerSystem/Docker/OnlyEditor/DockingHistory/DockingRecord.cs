#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace SexyDu.ContainerSystem.Editor
{
    /// <summary>
    /// 컨테이너 도킹 관련 이력
    /// </summary>
    [Serializable]
    public struct DockingRecord
    {
        // 표시 시간 포맷
        private const string TimeFormat = "yyyy-MM-dd HH:mm:ss";

        // 시점
        [SerializeField] private string dateTime;

        [Header("Method")]
        // 메서드 정보
        [SerializeField][TextArea] private string methodInformation;

        [Header("File")]
        // 스크립트 파일 에셋 경로
        [SerializeField][TextArea] private string scriptAssetPath;
        // 스크립트 줄
        [SerializeField] private int lineInScript;

        public DockingRecord(StackFrame frame)
        {
            this.dateTime = DateTime.Now.ToString(TimeFormat);

            if (frame != null)
            {
                string filePath = frame.GetFileName();
                this.scriptAssetPath = Application.dataPath.Length < filePath.Length ? filePath.Substring(Application.dataPath.Length) : filePath;
                this.lineInScript = frame.GetFileLineNumber();
                if (frame.HasMethod())
                    this.methodInformation = GetMethodInformation(frame.GetMethod());
                else
                    this.methodInformation = string.Empty;
            }
            else
            {
                this.scriptAssetPath = string.Empty;
                this.lineInScript = 0;

                this.methodInformation = string.Empty;
            }
        }

        /// <summary>
        /// MethodBase를 기반으로 Method 정보 문자열 반환
        /// </summary>
        private static string GetMethodInformation(MethodBase methodBase)
        {
            StringBuilder sb = new StringBuilder(methodBase.DeclaringType.Name);
            sb.AppendFormat(".{0} (", methodBase.Name);
            ParameterInfo[] parameters = methodBase.GetParameters();
            if (parameters != null && parameters.Length > 0)
            {
                sb.Append(parameters[0].ParameterType);

                for (int i = 1; i < parameters.Length; i++)
                {
                    sb.AppendFormat(", {0}", parameters[i].ParameterType);
                }
            }
            sb.AppendLine(")");

            sb.AppendFormat("(namespace {0})", methodBase.DeclaringType.Namespace);

            return sb.ToString();
        }
    }
}
#endif