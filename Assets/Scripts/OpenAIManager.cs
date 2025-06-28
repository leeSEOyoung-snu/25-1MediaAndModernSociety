using System.Collections.Generic;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class OpenAIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ByeoriController byeoriController;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private TMP_InputField inputField;
    
    [Header("Values")]
    [SerializeField] private string apiKey;

    private OpenAIAPI _api;
    private List<ChatMessage> _messages;
    private ChatMessage _currentMessage;

    private readonly int _maxMessages = 10;
    
    public void Init()
    {
        _api = new OpenAIAPI(apiKey);
        inputField.text = string.Empty;
    }

    public void StartConversation()
    {
        _messages = new List<ChatMessage>()
        {
            new ChatMessage(ChatMessageRole.System, "너는 고민 상담 친구 벼리야. " +
                                                    "반말로 친구끼리 말하듯이 친근하고 자연스러운 대화를 이어 나가고, 각 단계별로:" +
                                                    "0: 이름 묻기, " +
                                                    "1: 대화 상대의 고민을 상담해주는 친구임을 소개하며 고민 묻기, " +
                                                    "2: 고민 구체화, " +
                                                    "3: 위로와 간단 해결책 제시 -> “이 방법 어때?” 라고 묻기, " +
                                                    "4: 추가 대응 또는 종료 단계를 수행해. " +
                                                    "5: 종료 단계가 수행되면 이 단계로 넘어오고, 마지막 인사를 나눠." +
                                                    "단계에 대한 정보는 넘겨줄 필요 없지만, 단계 5에 대한 응답일 때는 마지막에 반드시 \"\\end\"를 붙여." +
                                                    "\n그리고 항상 모든 응답 끝에는 알파벳 \"\\a\", \\\"b\", \\\"c\" 중 반드시 하나만을 다음 상황에 맞게 붙여야 해." +
                                                    "\\a: 평범한 대화가 오갈 때" +
                                                    "\\b: 즐거운 또는 행복한 표정을 지을 만한 상황일 때" +
                                                    "\\c: 안타까운 혹은 위로하는 표정을 지을 만한 상황일 때"
                                                    )
        };
        
        inputField.text = string.Empty;
        string startString = "안녕? 만나서 반가워, 내 이름은 벼리야!";
        textField.text = startString;
        Debug.Log(startString);
    }
    
    public async void GetResponse(string input)
    {
        if (input.Length < 1) return;

        // textField.enabled = false;
        inputField.enabled = false;

        GameManager.Instance.dialogueCnt++;
        
        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.TextContent = input;
        Debug.Log($"{userMessage.rawRole}: {userMessage.TextContent}");
        
        // Add the message to the list
        _messages.Add(userMessage);
        
        // // Update the text field with the user message
        // textField.text = userMessage.TextContent;
        
        // Clear the input field
        inputField.text = string.Empty;
        
        // Send the entire chat to OpenAI to get the next message
        var chatResult = await _api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.1,
            MaxTokens = 200,
            Messages = _messages
        });
        
        // Get the response message
        _currentMessage = new ChatMessage();
        _currentMessage.Role = chatResult.Choices[0].Message.Role;
        _currentMessage.TextContent = chatResult.Choices[0].Message.TextContent;
        Debug.Log($"{_currentMessage.rawRole}: {_currentMessage.TextContent}");
        
        // Add the response to the list of messages
        _messages.Add(_currentMessage);
        
        ProcessResponse();
    }

    private void ProcessResponse()
    {
        // Update the text field with the response
        string processedContent = _currentMessage.TextContent;

        // 표정 지정
        if (processedContent.Contains("\\c"))
        {
            byeoriController.SetByeoriSprite(2);
            processedContent = processedContent.Replace("\\c", "");
        }
        if (processedContent.Contains("\\b"))
        {
            byeoriController.SetByeoriSprite(1);
            processedContent = processedContent.Replace("\\b", "");
        }
        if (processedContent.Contains("\\a"))
        {
            byeoriController.SetByeoriSprite(0);
            processedContent = processedContent.Replace("\\a", "");
        }
        
        // 대화 종료
        if (processedContent.Contains("\\end"))
        {
            StartCoroutine(LLMManager.Instance.EndConversation());
            processedContent = processedContent.Replace("\\end", "");
        }
        else if (GameManager.Instance.dialogueCnt >= _maxMessages)
        {
            textField.text = processedContent;
            StartCoroutine(LLMManager.Instance.EndConversation());
            return;
        }
        // else textField.enabled = true;
        else inputField.enabled = true;
        
        textField.text = processedContent;
    }
}
