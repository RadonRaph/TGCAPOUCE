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
    
    public BranchTextureInfo[] branchTexs;
    public TextureInfo[] leavesTexs;
    public TextureInfo[] flowerTexs;

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

        for (int i = 0; i < branchTexs.Length; i++)
        {
            if (!branchTexs[i].baked)
            {
                branchTexs[i].BakePoints();
            }
        }
    }

    
    public GameObject BuildPlant(Plant plant, Vector3 localPosition, Transform parent)
    {
        GameObject obj = new GameObject(plant.plantCode);
        obj.transform.SetParent(parent);
        obj.transform.localPosition = localPosition;

        GameObject branchPlane = Instantiate(_quadPrefab, obj.transform);
        BranchTextureInfo branchTexInfo = branchTexs[plant.branchTex];
        float scale = Random.Range(branchTexInfo.scaleMinMax.x, branchTexInfo.scaleMinMax.y);
        branchPlane.transform.localScale = new Vector3(scale, scale, 1);
        branchPlane.transform.localPosition = new Vector3(0, 0.5f * scale, 0);
        MeshRenderer branchRenderer = branchPlane.GetComponent<MeshRenderer>();
        branchRenderer.material.SetTexture(MainTex ,branchTexInfo.tex);

        return obj;
    }
}

[System.Serializable]
public class TextureInfo 
{
    [SerializeField]
    public Texture2D tex;
    [SerializeField]
    public Vector2 scaleMinMax = Vector2.one;


}

[System.Serializable]
public class BranchTextureInfo : TextureInfo
{
    public bool baked = false;
    
    [ReadOnly]
    [SerializeField] public Ray[] leavesPoints;
    [ReadOnly]
    [SerializeField] public Vector3[] flowerPoints;

    [Button("Bake Points")]
    public void BakePoints()
    {
        if (tex == null)
        {
           Debug.LogError("Texture should not be empty !"); 
        }
        
        int width = tex.width;
        int height = tex.height;

        List<Ray> leaves = new List<Ray>();
        List<Vector3> flowers = new List<Vector3>();

        Color flowerColor = new Color(16 / 255f, 252 / 255f, 236 / 255f);
        Color leavePosColor = new Color(66 / 255f, 1, 22 / 255f);
        Color leaveDirColor = new Color(252 / 255f, 24 / 255f, 16 / 255f);

        Color[] pixels = tex.GetPixels();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int i = ArrayExtends.ToLinearIndex(x, y, width);
                
                Color pix = pixels[i];

                float xPos = x / (float) width;
                float yPos = y / (float) height;

                xPos = MathExtends.Remap(xPos, 0, 1, -0.5f, 0.5f);
                yPos = MathExtends.Remap(yPos, 0, 1, -0.5f, 0.5f);

                if (CompareColor(pix, flowerColor))
                {
                    flowers.Add(new Vector3(xPos, yPos,0));
                }else if (CompareColor(pix, leavePosColor))
                {
                    Vector3 pos = new Vector3(xPos, yPos, 0);
                    Vector3 dir = Vector3.zero;

                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int tx = Mathf.Clamp(x + dx, 0, width - 1);
                            int ty = Mathf.Clamp(y + dy, 0, height - 1);

                            int ti = ArrayExtends.ToLinearIndex(tx, ty, width);

                            if (CompareColor(pixels[ti], leaveDirColor))
                            {
                                dir = (new Vector3(tx, ty, 0) - new Vector3(x, y, 0)).normalized;
                                break;
                            }
                        }
                    }


                    Ray r = new Ray(pos, dir);
                    leaves.Add(r);
                }
            }
        }

        leavesPoints = leaves.ToArray();
        flowerPoints = flowers.ToArray();
        baked = true;
    }

    bool CompareColor(Color A, Color B)
    {
        float diff = Mathf.Abs(A.r - B.r) + Mathf.Abs(A.g - B.g) + Mathf.Abs(A.b - B.b);

        return diff < 0.1f;
    }
}
