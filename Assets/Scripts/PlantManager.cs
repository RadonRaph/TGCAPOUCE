using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Raccoonlabs;
using Random = UnityEngine.Random;


public class PlantManager : MonoBehaviour
{

    public static PlantManager instance;

    public PlantInfo info;

    public Color[] palette;

    private GameObject _planePrefab;
    private GameObject _quadPrefab;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");

    private void Start()
    {
        if (PlantManager.instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            PlantManager.instance = this;
        }
        
        
        //MakeTextureArray and sent it a global property

        _quadPrefab = Resources.Load<GameObject>("Prefabs/Quad");

        for (int i = 0; i < info.branchTexs.Length; i++)
        {
            if (!info.branchTexs[i].baked)
            {
                info.branchTexs[i].Bake();
            }
        }
    }

    public Plant CreateBasePlant()
    {
        Plant p = new Plant();

        p.nickname = "Unnamed";
        p.branchTex = Random.Range(0, info.branchTexs.Length);
        p.branchColor = Random.Range(0, info.branchTexs[p.branchTex].palette.Length);
        
        p.leaveTex = Random.Range(0, info.leavesTexs.Length);
        p.leaveColor = Random.Range(0, info.leavesTexs[p.leaveTex].palette.Length);
        
        p.flowerTex = Random.Range(0, info.flowerTexs.Length);
        p.flowerColor = Random.Range(0, info.flowerTexs[p.flowerTex].palette.Length);

        p.plantCode = "!" + p.branchTex + "-" + p.branchColor + "!" + p.leaveTex + "-" + p.leaveColor + "!" +
                      p.flowerTex + "-" + p.flowerColor;

        return p;
    }

    
    public GameObject BuildPlant(Plant plant, Vector3 localPosition, Transform parent)
    {
        GameObject obj = new GameObject(plant.plantCode);
        obj.transform.SetParent(parent);
        obj.transform.localPosition = localPosition;

        GameObject branchPlane = Instantiate(_quadPrefab, obj.transform);
        BranchTextureInfo branchTexInfo = info.branchTexs[plant.branchTex];
        float scale = Random.Range(branchTexInfo.scaleMinMax.x, branchTexInfo.scaleMinMax.y);
        branchPlane.transform.localScale = new Vector3(scale, scale, 1);
        branchPlane.transform.localPosition = new Vector3(0, 0.5f * scale, 0);
        MeshRenderer branchRenderer = branchPlane.GetComponent<MeshRenderer>();
        branchRenderer.material.SetTexture(MainTex ,branchTexInfo.tex);


        DecorationTextureInfo leaveTexInfo = info.leavesTexs[plant.leaveTex];
        Material leaveMat = new Material(Shader.Find("Main/Plant"));
        leaveMat.SetTexture(MainTex, leaveTexInfo.tex);
        float leaveScale = Random.Range(leaveTexInfo.scaleMinMax.x, leaveTexInfo.scaleMinMax.y);
        for (int i = 0; i < branchTexInfo.leavesPoints.Length; i++)
        {
            
            
            LeafSpot leaveRay = branchTexInfo.leavesPoints[i];

            GameObject leave = Instantiate(_quadPrefab, branchPlane.transform);
            leave.transform.localScale = new Vector3(leaveScale, leaveScale, leaveScale);
            leave.transform.localPosition = leaveRay.origin * scale + leaveTexInfo.offset * leaveScale - Vector3.forward*0.01f*(i+1);
            //ROTATION Glitch Add a custom pivot to rotate around
           // leave.transform.up = leaveRay.direction;

            MeshRenderer leaveRenderer = leave.GetComponent<MeshRenderer>();
            leaveRenderer.material = leaveMat;
        }
        
        DecorationTextureInfo flowerTexInfo = info.flowerTexs[plant.flowerTex];
        Material flowerMat = new Material(Shader.Find("Main/Plant"));
        flowerMat.SetTexture(MainTex, flowerTexInfo.tex);
        float flowerScale = Random.Range(flowerTexInfo.scaleMinMax.x, flowerTexInfo.scaleMinMax.y);
        for (int i = 0; i < branchTexInfo.flowerPoints.Length; i++)
        {

            Vector3 flowerPos = branchTexInfo.flowerPoints[i];

            GameObject flower = Instantiate(_quadPrefab, branchPlane.transform);
            flower.transform.localScale = new Vector3(flowerScale, flowerScale, flowerScale);
            flower.transform.localPosition = flowerPos * scale + flowerTexInfo.offset * flowerScale + Vector3.forward*0.01f*(i+1) ;
            //ROTATION Glitch Add a custom pivot to rotate around
            // leave.transform.up = leaveRay.direction;

            MeshRenderer flowerRenderer = flower.GetComponent<MeshRenderer>();
            flowerRenderer.material = flowerMat;
        }

        return obj;
    }
}

