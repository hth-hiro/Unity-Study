using UnityEngine;

public class SimpleShooter : MonoBehaviour
{
    [Header("Bullet Settings")]
    [Tooltip("총알로 사용할 프리팹을 할당하세요.")]
    public GameObject bulletPrefab;

    [Tooltip("총알이 생성될 위치 (선택). 할당하지 않으면 기본 계산 위치를 사용합니다.")]
    public Transform firePoint;

    [Tooltip("총알의 발사 속도")]
    public float bulletSpeed = 20f;

    [Tooltip("총알이 파괴되기까지의 시간 (초)")]
    public float bulletLifetime = 3f;

    [Tooltip("발사 쿨다운 (초)")]
    public float fireCooldown = 0.2f;

    private float _nextFireTime = 0f;

    void Update()
    {
        // Space 키 입력 및 쿨다운 체크
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= _nextFireTime)
        {
            Fire();
            _nextFireTime = Time.time + fireCooldown;
        }
    }

    private void Fire()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("SimpleShooter: Bullet Prefab이 할당되지 않았습니다!");
            return;
        }

        // 총알 생성 위치 결정
        Vector3 spawnPos = firePoint != null 
            ? firePoint.position 
            : transform.position + transform.forward * 1.2f + Vector3.up * 0.5f;

        // 총알 생성 회전 결정 (큐브의 정면 방향)
        Quaternion spawnRot = transform.rotation;

        // 총알 인스턴스화
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, spawnRot);

        // Rigidbody를 가져와서 정면 방향(transform.forward)으로 속도 적용
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("SimpleShooter: Bullet Prefab에 Rigidbody가 없습니다!");
        }

        // 일정 시간 후 총알 오브젝트 파괴
        Destroy(bullet, bulletLifetime);
    }
}
