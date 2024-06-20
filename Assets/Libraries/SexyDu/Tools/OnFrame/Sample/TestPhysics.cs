using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SexyDu.Sample
{
    public class TestPhysics : MonoBehaviour
    {
        public float damageRadius = 5f; // 범위 반경
        public int damageAmount = 10;   // 줄 데미지 양
        public LayerMask enemyLayer;    // 적이 속한 레이어

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DealDamage();
            }
        }

        void DealDamage()
        {
            // 현재 위치를 중심으로 damageRadius 내에 있는 모든 콜라이더를 감지
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, enemyLayer);

            foreach (var hitCollider in hitColliders)
            {
                // 감지된 콜라이더가 적 오브젝트인지 확인
                Enemy enemy = hitCollider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount);
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            // 기즈모를 사용하여 범위를 시각화
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }

        [SerializeField] private Transform target;
        [SerializeField] private float radius = 3;
        [SerializeField] public float angleRange = 30f;
        private void OnGUI()
        {
            if (GUI.Button(new Rect(0f, 0f, 100f, 100f), ""))
            {
                bool isCollision = false;
                Transform transformCache = transform; 

                Vector2 interV = target.position - transformCache.position;
                Vector2 interVnormalized = interV.normalized;
                Vector2 way = transformCache.up;
                Debug.LogFormat("interV [{0}, {1}, {2}], normalized [{3}, {4}, {5}]",
                    interV.x, interV.y, "none", interVnormalized.x, interVnormalized.y, "none");
                Debug.LogFormat("way [{0}, {1}, {2}]", way.x, way.y, "none");

                // target과 나 사이의 거리가 radius 보다 작다면
                if (interV.magnitude <= radius)
                {
                    // '타겟-나 벡터'와 '내 정면 벡터'를 내적
                    float dot = Vector2.Dot(interVnormalized, way);
                    // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
                    float theta = Mathf.Acos(dot);
                    // angleRange와 비교하기 위해 degree로 변환
                    float degree = Mathf.Rad2Deg * theta;

                    // 시야각 판별
                    if (degree <= angleRange / 2f)
                        isCollision = true;
                }

                Debug.LogFormat("isCollision : {0}", isCollision);
            }
        }
    }

    // Enemy 스크립트 예제
    public class Enemy : MonoBehaviour
    {
        public int health = 100;

        public void TakeDamage(int amount)
        {
            health -= amount;
            if (health <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            // 적 사망 로직
            Destroy(gameObject);
        }
    }
}