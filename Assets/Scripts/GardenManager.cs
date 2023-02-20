using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenManager : MonoBehaviour
{

    public int width = 5;
    public int height = 5;

    public Slot[,] slots;

    public string playerCode;

    private GameObject _slotPrefab;
    private Plant _dummyPlant;
    
    // Start is called before the first frame update
    void Start()
    {
        _slotPrefab = Resources.Load<GameObject>("Prefabs/Slot");
        _dummyPlant = new Plant();
        

        //Load slots data from server
    }


    void LoadData()
    {
        width = 5;//ToLoad
        height = 5;//ToLoad

        slots = new Slot[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject obj = Instantiate(_slotPrefab, transform);
                obj.transform.localPosition = new Vector3((int) (-width / 2f + x), 0, (int) (-height / 2f + y));

                slots[x, y] = obj.GetComponent<Slot>();
                slots[x,y].plant = 
            }
        }

    }


}
