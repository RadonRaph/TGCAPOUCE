using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plant
{
    public int branchTex;
    public int branchColor;

    public int leaveTex;
    public int leaveColor;

    public int flowerTex;
    public int flowerColor;

    public Guid plantId;
    public string plantCode;
    public Guid ownerId;
    public string nickname;
    public float creationTimestamp;
    public int exchangesCount;

    public Plant()
    {
        
        plantId = Guid.NewGuid();

        branchTex = 0;
        branchColor = 0;


        leaveTex = 0;
        leaveColor = 0;

        flowerTex = 0;
        flowerColor = 0;

        plantCode = "DEFAULT";

    }

    public Plant(string guid)
    {
        
    }
}
