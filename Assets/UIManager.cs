using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public InputField loginNicknameInput;
    public InputField loginPassInput;
    
    
    public InputField registerNicknameInput;
    public InputField registerPassInput;
    public InputField registerPlantName;


    public WebApi webApi;
    public PlantManager plantManager;

    private Plant[] generatedPlants;
    private int _selectedPlant = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        webApi = FindObjectOfType<WebApi>();
        plantManager = FindObjectOfType<PlantManager>();
        
        if (!PlayerPrefs.HasKey("PlayerGUID"))
        {
            OpenLogin();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OpenLogin()
    {
        
    }

    public void TryLogin()
    {
        webApi.Login(loginNicknameInput.text, loginPassInput.text, LoginSuccess, LoginFailed);
    }
    
    void LoginFailed()
    {
        
    }

    void LoginSuccess()
    {
        
    }

    public void OpenRegister()
    {
        if (generatedPlants == null)
        {
            generatedPlants = new Plant[4];
            generatedPlants[0] = plantManager.CreateBasePlant();
            generatedPlants[1] = plantManager.CreateBasePlant();
            generatedPlants[2] = plantManager.CreateBasePlant();
            generatedPlants[3] = plantManager.CreateBasePlant();
            
        }
    }

    public void SelectPlant(int nb)
    {
        _selectedPlant = nb;
    }

    public void TryRegister()
    {
        webApi.Register(registerNicknameInput.text, registerPassInput.text, generatedPlants[_selectedPlant], registerPlantName.text, RegisterSuccess,RegisterFail);
    }


    public void RegisterSuccess()
    {
        
    }


    public void RegisterFail()
    {
        
    }

}
