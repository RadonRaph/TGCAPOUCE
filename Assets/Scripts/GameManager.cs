using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool isContructionMode = false;
    public LayerMask interactionMask;

    private Plant _selectedPlant;

    private GameObject _lastHoveredGameobject;
    
    // Start is called before the first frame update
    void Start()
    {
        _selectedPlant = new Plant();
    }

    // Update is called once per frame
    void Update()
    {
        Ray inputRay;
        GetInput(out inputRay);
        if (true)
        {
            RaycastHit hit;
            Physics.Raycast(inputRay, out hit, 100, interactionMask);

            if (hit.transform != null)
            {
                if (_lastHoveredGameobject!=null && hit.transform.gameObject != _lastHoveredGameobject)
                {
                    _lastHoveredGameobject.SendMessage("EndHover");
                    _lastHoveredGameobject = hit.transform.gameObject;
                    _lastHoveredGameobject.SendMessage("BeginHover");
                }
                
                
                if (isContructionMode)
                {
                    if (hit.transform.gameObject.CompareTag("Slot"))
                    {
                        Slot slot = hit.transform.gameObject.GetComponentInParent<Slot>();
                        
                        slot.ChangePlant(_selectedPlant);
                    }
                }
                
                
            }

        }
        
        
    }

    bool GetInput(out Ray ray)
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return true;
        }else if (Input.touchCount == 1)
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            return true;
        }

        ray = new Ray(Vector3.zero, Vector3.zero);
        return false;
    }

    public void ToggleConstructionMode()
    {
        if (isContructionMode)
        {
            BroadcastMessage("EndConstructionMode");
            isContructionMode = false;
        }
        else
        {
            BroadcastMessage("BeginConstructionMode");
            isContructionMode = true;
        }
    }
}
