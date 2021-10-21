using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject bullet;
    public Transform sPoint;
    public SpriteRenderer reloadUI;
    public float timeBetweenShots;
    public float reloadTime = 2f;


    public int maxAmmo = 10;
    private int ammo;

    public AudioSource audioSource;
    public AudioClip[] shootSounds;

    private float shotTime;

    private void Start()
    {
        ammo = maxAmmo;
    }

    void Update()
    {
        //마우스 왼쪽버튼을 눌렀을때
        if (Input.GetMouseButton(0))
        {
            if (BossController.cleared)
            {
                ExitGame();
            }
            else if (Time.time >= shotTime)
            {
                ShootBullet();
            }
        }
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    float buffSpeedPercent = 1.0f;

    public void MakeFasterBulletSpeed()
    {
        buffSpeedPercent = 1.5f;
    }

    void ShootBullet()
    {
        if (!hasAmmo()) return;
        float angle = GetAngleToMouse();
        //총알을 생성한다
        BulletController bulletController = Instantiate(bullet, sPoint.position,
            Quaternion.AngleAxis(angle - 90.0f, Vector3.forward)).GetComponent<BulletController>();
        bulletController.BuffSpeed(buffSpeedPercent);
        //재장전 총알 딜레이 
        shotTime = Time.time + timeBetweenShots;
        UseAmmo();
        PlayRandomShootSound();
        CameraController.Shake();
    }

    bool hasAmmo()
    {
        return ammo > 0;
    }

    float GetAngleToMouse()
    {
        //카메라 스크린의 마우스 거리와 총과의 방향 
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //마우스 거리로 부터 각도 계산
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void UseAmmo()
    {
        --ammo;
        if (ammo <= 0)
        {
            reloadUI.gameObject.SetActive(true);
            StartCoroutine(ReloadAmmo());
        }
    }

    IEnumerator ReloadAmmo()
    {
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        reloadUI.gameObject.SetActive(false);
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

    public void MakeFasterShootSpeed()
    {
        timeBetweenShots -= 0.1f;
    }
}
