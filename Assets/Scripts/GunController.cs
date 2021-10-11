using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public Transform sPoint;
    public float timeBetweenShots;

    public AudioSource audioSource;
    public AudioClip[] shootSounds;

    private float shotTime;

    void Update()
    {
        //마우스 왼쪽버튼을 눌렀을때
        if (Input.GetMouseButton(0))
        {
            if (Time.time >= shotTime)
            {
                ShootBullet();
                PlayRandomShootSound();
                CameraController.Shake();
            }
        }
    }

    void ShootBullet()
    {
        float angle = GetAngleToMouse();
        //총알을 생성한다
        Instantiate(bullet, sPoint.position, Quaternion.AngleAxis(angle - 90.0f, Vector3.forward));
        //재장전 총알 딜레이 
        shotTime = Time.time + timeBetweenShots;
    }

    float GetAngleToMouse()
    {
        //카메라 스크린의 마우스 거리와 총과의 방향 
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //마우스 거리로 부터 각도 계산
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void PlayRandomShootSound()
    {
        RandomlySelectShootSound();
        audioSource.Play();
    }

    void RandomlySelectShootSound()
    {
        int r = Random.Range(0, shootSounds.Length);
        audioSource.clip = shootSounds[r];
    }
}
