using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivationScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField keyInput;

    [SerializeField]
    private TMP_Text feedbackText;

    void Start()
    {
        if (LicenseManager.IsActivated())
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public void OnSubmitKey()
    {
        if (LicenseManager.Activate(keyInput.text))
        {
            feedbackText.text = "Key Accepted!";
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            feedbackText.text = "Invalid key";
        }
    }
}
