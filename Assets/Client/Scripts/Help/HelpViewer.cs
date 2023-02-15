using UnityEngine;
using UnityEngine.UI;

public class HelpViewer : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _helpButton;

    public void OnHelpButton()
    {
        _panel.SetActive(true);
        _helpButton.gameObject.SetActive(false);
    }

    public void OnBackButton()
    {
        _panel.SetActive(false);
        _helpButton.gameObject.SetActive(true);
    }
}
