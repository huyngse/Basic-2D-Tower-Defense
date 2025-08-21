using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActivationScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField keyInput;

    [SerializeField]
    private TMP_Text feedbackText;

    [SerializeField]
    private Button submitButton;

    [SerializeField]
    private TMP_Text buttonText;

    void Start()
    {
        if (LicenseManager.IsActivated())
        {
            SceneManager.LoadScene("MainScene");
        }
        keyInput.onEndEdit.AddListener(TrimInput);
    }

    private void TrimInput(string text)
    {
        keyInput.text = text.Trim();
    }

    public void OnSubmitKey()
    {
        submitButton.interactable = false;
        buttonText.text = "Submitting...";
        StartCoroutine(
            LicenseManager.Activate(
                keyInput.text,
                success =>
                {
                    if (success)
                    {
                        feedbackText.text = "Key Accepted!";
                        SceneManager.LoadScene("MainScene");
                    }
                    else
                    {
                        feedbackText.text = "Invalid key";
                        submitButton.interactable = true;
                        buttonText.text = "Submit";
                    }
                }
            )
        );
    }
}
