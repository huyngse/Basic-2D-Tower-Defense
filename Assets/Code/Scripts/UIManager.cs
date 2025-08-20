using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager main;
    private bool isHoveringUI = false;

    private void Awake()
    {
        main = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            OnResetKey();
        }
    }

    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }

    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }

    public void OnResetKey()
    {
        LicenseManager.ResetLicense();
        SceneManager.LoadScene("ActivationScene");
    }
}
