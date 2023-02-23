using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Raccoonlabs;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "PlantInfo", menuName = "PlantInfo")]
public class PlantInfo : ScriptableObject
{
    public BranchTextureInfo[] branchTexs;
    public DecorationTextureInfo[] leavesTexs;
    public DecorationTextureInfo[] flowerTexs;
}

[System.Serializable]
public class TextureInfo 
{
    [SerializeField]
    public Texture2D tex;
    [SerializeField]
    public Vector2 scaleMinMax = Vector2.one;

    [SerializeField] public bool baked = false;
    [SerializeField] public Color[] palette;

    [HideInInspector]
    public Color[] pixels;

    [HideInInspector] public int width;
    [HideInInspector] public int height;
    
    public bool CompareColor(Color A, Color B)
    {
        float diff = Mathf.Abs(A.r - B.r) + Mathf.Abs(A.g - B.g) + Mathf.Abs(A.b - B.b);

        return diff < 0.8f;
    }

    
    public virtual void Bake()
    {

        if (tex == null)
        {
            Debug.LogError("Texture should not be empty !");
            return;
        }
        width = tex.width;
        height = tex.height;
        
        pixels = tex.GetPixels();
        

        List<Color> colors = new List<Color>();
        for (int x = 0; x < width; x++)
        {
            int i = ArrayExtends.ToLinearIndex(x, height-1, width);

            Color c = pixels[i];
            if (c.a > 0.5f)
                colors.Add(c);
        }

        palette = colors.ToArray();
        


        baked = true;
    }


}

[System.Serializable]
public class DecorationTextureInfo : TextureInfo
{

    public Vector3 offset;

    [Button("Bake")]
    public override void Bake()
    {
        base.Bake();
        
        
        Color flowerColor = new Color(16 / 255f, 252 / 255f, 236 / 255f);

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
                    offset = new Vector3(xPos, -yPos,0);
                    return;
                }
            }
        }

        baked = true;

    }
}

[System.Serializable]
public class LeafSpot
{
    [SerializeField]
    public Vector3 origin;
    [SerializeField]
    public Vector3 direction;

    public LeafSpot(Vector3 or, Vector3 d)
    {
        origin = or;
        direction = d;
    }
}

[System.Serializable]
public class BranchTextureInfo : TextureInfo
{
    public bool baked = false;
    

    [SerializeField] public LeafSpot[] leavesPoints;
    [SerializeField] public Vector3[] flowerPoints;

    [Button("Bake")]
    public override void Bake()
    {
        base.Bake();
        
        List<LeafSpot> leaves = new List<LeafSpot>();
        List<Vector3> flowers = new List<Vector3>();

        Color flowerColor = new Color(16 / 255f, 252 / 255f, 236 / 255f);
        Color leavePosColor = new Color(66 / 255f, 1f, 22 / 255f);
        Color leaveDirColor = new Color(252 / 255f, 24 / 255f, 16 / 255f);


        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int i = ArrayExtends.ToLinearIndex(x, y, width);
                
                Color pix = pixels[i];
                if (pix.a < 0.1f)
                    continue;

                float xPos = x / (float) width;
                float yPos = y / (float) height;

                xPos = MathExtends.Remap(xPos, 0, 1, -0.5f, 0.5f);
                yPos = MathExtends.Remap(yPos, 0, 1, -0.5f, 0.5f);

                if (CompareColor(pix, flowerColor))
                {
                    flowers.Add(new Vector3(xPos, yPos,0));
                }else if (CompareColor(pix, leavePosColor))
                {
                    Debug.Log("Found Leaf !");
                    Vector3 pos = new Vector3(xPos, yPos, 0);
                    Vector3 dir = GetDir(x,y,width,height, pixels, leaveDirColor);
                    
                    LeafSpot r = new LeafSpot(pos, dir);
                    leaves.Add(r);
                }
            }
        }

        leavesPoints = leaves.ToArray();
        flowerPoints = flowers.ToArray();
        baked = true;
    }

    Vector3 GetDir(int x, int y, int width, int height, Color[] pixels, Color leaveDirColor)
    {
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
                    return dir;
                }
            }
        }

        return dir;
    }


}

