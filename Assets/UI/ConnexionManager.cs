using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnexionManager : MonoBehaviour
{
    public GameObject SignUpCanvas, LoginCanvas, SignUpPlantSelectionCanvas;


    // Start is called before the first frame update
    void Start()
    {
        LoginCanvas.SetActive(true);
        SignUpCanvas.SetActive(false);
        SignUpPlantSelectionCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        SceneManager.LoadScene("Game");
    }

    public void SignUp()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenLoginCanvas()
    {
        LoginCanvas.SetActive(true);
        SignUpCanvas.SetActive(false);
    }

    public void OpenSignUpCanvas()
    {
        LoginCanvas.SetActive(false);
        SignUpPlantSelectionCanvas.SetActive(false);
        SignUpCanvas.SetActive(true);
    }

    public void OpenSignUpPlantSelectionCanvas()
    {
        SignUpCanvas.SetActive(false);
        SignUpPlantSelectionCanvas.SetActive(true);
    }
}
