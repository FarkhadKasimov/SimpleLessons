using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SelectingLoadOption : MonoBehaviour
{
    [SerializeField] private LoadAndDisplayingImages m_LoadAndDisplayingImages;

    [SerializeField] private TMP_Dropdown m_Dropdown;
    [SerializeField] private Button m_LoadButton;

    private void Start()
    {
        // Add listener for when the load button is clicked
        m_LoadButton.onClick.AddListener(m_LoadAndDisplayingImages.AllAtOnce);

        // Add listener for when the value of the Dropdown changes, to take action
        m_Dropdown.onValueChanged.AddListener(delegate
        {
            ChangeLoadOption(m_Dropdown);
        });
    }

    // Method to change the way images are loaded
    void ChangeLoadOption(TMP_Dropdown dropdown)
    {
        // Remove all click event listeners from the load button
        m_LoadButton.onClick.RemoveAllListeners();

        // Change the way images are loaded based on the selected value of the dropdown
        switch (dropdown.value)
        {
            case 0:
                m_LoadButton.onClick.AddListener(m_LoadAndDisplayingImages.AllAtOnce);
                break;
            case 1:
                m_LoadButton.onClick.AddListener(m_LoadAndDisplayingImages.OneByOne);
                break;
            case 2:
                m_LoadButton.onClick.AddListener(m_LoadAndDisplayingImages.WhenImageReady);
                break;
        }
    }
}