using System.Collections;
using System.Collections.Generic;
using OpenAI_API;
using OpenAI_API.Chat;
using TMPro;
using UnityEngine;

public class OpenAIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private TMP_InputField inputField;

    private OpenAIAPI _api;
    private List<ChatMessage> _messages;
    
    
}
