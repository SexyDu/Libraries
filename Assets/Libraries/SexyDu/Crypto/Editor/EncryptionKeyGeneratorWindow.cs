using System;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

namespace SexyDu.OnEditor.LocalLibraryImporter
{
    public class EncryptionKeyGeneratorWindow : EditorWindow
    {
        [MenuItem("SexyDu/EncryptionKeyGenerator")]
        static void Open()
        {
            EncryptionKeyGeneratorWindow window = GetWindow<EncryptionKeyGeneratorWindow>();
            window.titleContent = new GUIContent("EncryptionKeyGenerator");
            window.minSize = new Vector2(100f, 100f);

            window.Show(true);
        }

        private int length = 32;

        private int currentLength = 32;
        private string key = string.Empty;
        private string declaration = string.Empty;

        private void OnGUI()
        {
            GUILayout.Box("Base64 기반 키 생성기입니다.\n원하는 길이를 입력하고 생성버튼을 눌러 확인하세요.", GUILayout.ExpandWidth(true));
            EditorGUILayout.Space(10);
            length = EditorGUILayout.IntField("길이", length);

            if (GUILayout.Button("생성"))
            {
                key = GenerateBase64Key(length);
                currentLength = length;
                declaration = GetDeclaration(key);
            }

            if (!string.IsNullOrEmpty(key))
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField($"Key ({currentLength} bytes)", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(key, GUILayout.ExpandWidth(true), GUILayout.Height(18f));

                EditorGUILayout.LabelField("Declaration (char [])", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(declaration, GUILayout.ExpandWidth(true), GUILayout.Height(18f));
            }
        }

        private string GenerateBase64Key(int size)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[size];
                rng.GetBytes(key);
                return Convert.ToBase64String(key);
            }
        }

        private string GetDeclaration(string key)
        {
            return $"new char[{key.Length}] {{ {string.Join(", ", key.Select(b => $"'{b}'"))} }};";
        }
    }
}
