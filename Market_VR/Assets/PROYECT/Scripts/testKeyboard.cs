using UnityEngine;

public class testKeyboard : MonoBehaviour
{
    void Start()
    {
        var kb = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        Debug.Log("Keyboard supported: " + TouchScreenKeyboard.isSupported);
    }
}