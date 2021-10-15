using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootDelay : MonoBehaviour
{

    public Image image;
    public PlayerController player;

    public static ShootDelay shootDelay;

    private void Start()
    {
        if (shootDelay == null)
            shootDelay = this;
        else
            Destroy(this);
    }

    public void affect()
    {
        ShowIcon();
        player.BuffShootDelay();
    }

    void ShowIcon()
    {
        image.gameObject.SetActive(true);
    }
}
