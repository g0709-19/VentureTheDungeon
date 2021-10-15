using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSpeed : MonoBehaviour
{

    public Image image;
    public PlayerController player;

    public static BulletSpeed bulletSpeed;
    private void Start()
    {
        if (bulletSpeed == null)
            bulletSpeed = this;
        else
            Destroy(this);
    }

    public void affect()
    {
        ShowIcon();
        player.BuffBulletSpeed();
    }

    void ShowIcon()
    {
        image.gameObject.SetActive(true);
    }
}
