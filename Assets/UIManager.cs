using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject SignUpCanvas, LoginCanvas, SignUpPlantSelectionCanvas, CanvasBackground, GameCanvas;

    public TMP_InputField loginNicknameInput;
    public TMP_InputField loginPassInput;
    
    
    public TMP_InputField registerNicknameInput;
    public TMP_InputField registerPassInput;
    public TMP_InputField registerPlantName;

    private string registerNicknameInputTemp, registerPassInputTemp;

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

        SignUpCanvas.SetActive(false);
        SignUpPlantSelectionCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenLogin()
    {
        LoginCanvas.SetActive(true);
        SignUpCanvas.SetActive(false);
        CanvasBackground.SetActive(true);
        GameCanvas.SetActive(false);
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
        LoginCanvas.SetActive(false);
        SignUpCanvas.SetActive(false);
        SignUpPlantSelectionCanvas.SetActive(false);
        CanvasBackground.SetActive(false);
        GameCanvas.SetActive(true);
    }

    public void OpenRegister()
    {
        LoginCanvas.SetActive(false);
        SignUpPlantSelectionCanvas.SetActive(false);
        SignUpCanvas.SetActive(true);

        if (generatedPlants == null)
        {
            generatedPlants = new Plant[4];
            generatedPlants[0] = plantManager.CreateBasePlant();
            generatedPlants[1] = plantManager.CreateBasePlant();
            generatedPlants[2] = plantManager.CreateBasePlant();
            generatedPlants[3] = plantManager.CreateBasePlant();
        }
    }

    public void OpenPlantSelection()
    {
        SignUpCanvas.SetActive(false);
        SignUpPlantSelectionCanvas.SetActive(true);
        registerNicknameInputTemp = registerNicknameInput.text;
        registerPassInputTemp = registerPassInput.text;
    }

    public void SelectPlant(int nb)
    {
        _selectedPlant = nb;
    }

    public void TryRegister()
    {
        webApi.Register(registerNicknameInputTemp, registerPassInputTemp, generatedPlants[_selectedPlant], registerPlantName.text, RegisterSuccess,RegisterFail);
    }


    public void RegisterSuccess()
    {
        LoginCanvas.SetActive(false);
        SignUpCanvas.SetActive(false);
        SignUpPlantSelectionCanvas.SetActive(false);
        CanvasBackground.SetActive(false);
        GameCanvas.SetActive(true);
    }


    public void RegisterFail()
    {
        
    }

}
