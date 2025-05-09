using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomInput : MonoBehaviour
{
    public static CustomInput Instance;
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadKeyBinding();

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public KeyCode GetKey(string action)
    {
        if (keyBindings.ContainsKey(action))
        {
            return keyBindings[action];
        }
        return KeyCode.None;
    }

    public void SetKey(string action, KeyCode newKey)
    {
        string conflictAction = null;

        foreach (var binding in keyBindings)
        {
            if (binding.Value == newKey)
            {
                conflictAction = binding.Key;
                break;
            }
        }
        
        if (conflictAction != null)
        {
            keyBindings[conflictAction] = KeyCode.None;
            PlayerPrefs.SetInt(conflictAction, (int)KeyCode.None);
        }

        if (keyBindings.ContainsKey(action))
        {
            keyBindings[action] = newKey;
            PlayerPrefs.SetInt(action, (int)newKey);
            PlayerPrefs.Save();
        }

        FindObjectOfType<InputButtonSettings>()?.UpdateUI();
    }

    


    void LoadKeyBinding()
    {
        keyBindings["Jump"] = (KeyCode)PlayerPrefs.GetInt("Jump", (int)KeyCode.Space);
        keyBindings["Dash"] = (KeyCode)PlayerPrefs.GetInt("Dash", (int)KeyCode.V);
    }
}
