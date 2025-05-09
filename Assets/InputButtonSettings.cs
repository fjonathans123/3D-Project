using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputButtonSettings : MonoBehaviour
{
    //public TextMeshProUGUI jumpKeyText;
    //public Button JumpButton;

    private string currentAction = null;

    [System.Serializable]

    public class KeyBindingUI
    {
        public string actionName;
        public TextMeshProUGUI actionText;
        public Button keyBindButton;
        public TextMeshProUGUI keyText;
    }

    public List<KeyBindingUI> keyBindUI;
    private string currentRebindingAction = null;
    private TextMeshProUGUI currentKeyText = null;

    void Start()
    {
        foreach(var binding in keyBindUI)
        {
            binding.keyText.text = CustomInput.Instance.GetKey(binding.actionName).ToString();
            binding.keyBindButton.onClick.AddListener(() => StartBinding(binding));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentAction != null && Input.anyKeyDown)
        {
            foreach(KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                if(Input.GetKeyDown(key))
                {
                    CustomInput.Instance.SetKey(currentAction, key);
                    currentKeyText.text = key.ToString();

                    currentAction = null;
                    currentKeyText = null;
                    UpdateUI();
                    break;
                }
            }
        }
    }

    void StartBinding(KeyBindingUI binding)
    {
        currentAction = binding.actionName;
        currentKeyText = binding.keyText;

        currentKeyText.text = "Press Key...";
        //Debug.Log("Rebinding " + action);
    }
    
    public void UpdateUI()
    {
        foreach(var binding in keyBindUI)
        {
            binding.keyText.text = CustomInput.Instance.GetKey(binding.actionName).ToString();
        }
    }
}
