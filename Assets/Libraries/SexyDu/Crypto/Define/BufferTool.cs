using System;

namespace SexyDu.Crypto
{
    /// <summary>
    /// Buffer 관련 도구
    /// </summary>
    public static class BufferTool
    {
        /// <summary>
        /// 데이터 결합
        /// </summary>
        /// <param name="data1">데이터 1</param>
        /// <param name="data2">데이터 2</param>
        /// <returns>결합 데이터</returns>
        public static byte[] Combine(byte[] data1, byte[] data2)
        {
            byte[] result = new byte[data1.Length + data2.Length];
            Buffer.BlockCopy(data1, 0, result, 0, data1.Length);
            Buffer.BlockCopy(data2, 0, result, data1.Length, data2.Length);
            return result;
        }

        /// <summary>
        /// 데이터 분할
        /// </summary>
        /// <param name="data">데이터</param>
        /// <param name="length">분할 길이</param>
        /// <returns>분할 데이터</returns>
        public static (byte[], byte[]) Split(byte[] data, int length)
        {
            byte[] data1 = new byte[length];
            byte[] data2 = new byte[data.Length - length];
            Buffer.BlockCopy(data, 0, data1, 0, length);
            Buffer.BlockCopy(data, length, data2, 0, data2.Length);
            return (data1, data2);
        }

        /// <summary>
        /// 데이터 비교
        /// </summary>
        /// <param name="data1">데이터 1</param>
        /// <param name="data2">데이터 2</param>
        /// <returns>비교 결과</returns>
        public static bool Compare(byte[] data1, byte[] data2)
        {
            if (data1.Length != data2.Length) return false;
            for (int i = 0; i < data1.Length; i++)
            {
                if (data1[i] != data2[i]) return false;
            }
            return true;
        }

        public static string ToString(byte[] bytes)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(bytes[0].ToString());
            for (int i = 1; i < bytes.Length; i++)
                sb.AppendFormat(", {0}", bytes[i]);
            return sb.ToString();
        }
    }
}