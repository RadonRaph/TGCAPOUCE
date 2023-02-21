using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plant
{
    public int branchTex;
    public Color branchColor;

    public int leaveTex;
    public Color leaveColor;

    public int flowerTex;
    public Color flowerColor;

    public string plantCode;

    public Plant()
    {
        branchTex = 0;
        branchColor = new Color(99 / 255f, 62 / 255f, 24 / 255f);


        leaveTex = 0;
        leaveColor = new Color(24 / 255f, 99 / 255f, 42 / 255f);

        flowerTex = 0;
        flowerColor = new Color(211 / 255f, 217 / 255f, 100 / 255f);

        plantCode = "DEFAULT";

    }
}
