using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CubeCollections : MonoBehaviour
{
    [SerializeField] private List<GameObject> _cubes;
    [SerializeField] private TMP_InputField _inputText;

    public void DeleteCube()
    {
        Destroy(_cubes[int.Parse(_inputText.text)]);
        _cubes.RemoveAt(int.Parse(_inputText.text));
    }

    public void PrintCube()
    {
        print(_cubes[int.Parse(_inputText.text)]);
    }
}
