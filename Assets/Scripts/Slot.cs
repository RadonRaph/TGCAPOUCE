using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Slot : MonoBehaviour
{

    public Plant plant;
    public GameObject button;


    private MeshRenderer _buttonRenderer;
    private static readonly int BtnColor = Shader.PropertyToID("_Color");
    private bool _isHovered = false;

    private GameObject _currentPlantObjet;

    // Start is called before the first frame update
    void Start()
    {
        _buttonRenderer = button.GetComponent<MeshRenderer>();
    }

    public void Initialize()
    {
        //Instantiate Plant Prefab
    }

    [Button("BuildPlant")]
    public void ConstructPlant()
    {
        _currentPlantObjet = PlantManager.instance.BuildPlant(plant, Vector3.zero, transform);
    }

    public void ChangePlant(Plant newPlant)
    {
        if (plant == null)
        {
            //DestroyPlantPrefab
            plant = newPlant;
            //Instantiate New Plant prefab;
        }
        else
        {
            //DestroyPlantPrefab
            plant = null;
        }
        
        UpdateButtonColor();
        
    }

    public void BeginHover()
    {
        _isHovered = true;
        UpdateButtonColor();
    }

    public void EndHover()
    {
        _isHovered = false;
        UpdateButtonColor();
    }

    void UpdateButtonColor()
    {
        Color c = Color.black;

        if (_isHovered)
        {
            c = Color.yellow;
        }
        else
        {
            c = plant == null ? Color.green : Color.red;
        }
        _buttonRenderer.material.SetColor(BtnColor, c);
    }

    public void BeginConstructionMode()
    {
        
        UpdateButtonColor();
        button.transform.DOLocalMoveY(0.5f, 0.5f);
    }

    public void EndConstructionMode()
    {
        button.transform.DOLocalMoveY(-1, 0.5f);
    }

}
