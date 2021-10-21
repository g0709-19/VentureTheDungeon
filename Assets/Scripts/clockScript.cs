using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clockScript : MonoBehaviour
{
    public int maxHP;
    public int remainHP;

    public float percent;
    public Texture2D clockBG;
    public Texture2D clockFG;
    public float clockFGMaxWidth;

    // Start is called before the first frame update
    void Start()
    {
        clockFGMaxWidth = clockFG.width;
    }

    // Update is called once per frame
    void Update()
    {
        if (!BossController.cleared)
        {
            SetPercent();
        }
    }

    private void OnGUI()
    {
        float newBarWidth = (percent / 100) * clockFGMaxWidth;
        int gap = 50;

        // 비율에 맞도록 사각형 영역 잡고 텍스쳐 그림
        GUI.BeginGroup(new Rect((Screen.width - clockBG.width) / 2, gap, clockBG.width, clockBG.height));
        GUI.DrawTexture(new Rect(0, 0, clockBG.width, clockBG.height), clockBG);
        GUI.BeginGroup(new Rect(5, 6, newBarWidth, clockFG.height));
        GUI.DrawTexture(new Rect(1, 0, clockFG.width, clockFG.height), clockFG);
        GUI.EndGroup();
        GUI.EndGroup();
    }

    void SetPercent()
    {
        percent = remainHP / (float)maxHP * 100;
        if (remainHP <= 0)
        {
            remainHP = 0;
        }
    }
}
