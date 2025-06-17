using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionDialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI dialogue;
    [SerializeField] private TextMeshProUGUI[] options;
    [SerializeField] private GameObject optionsParent;
    [SerializeField] private ByeoriController byeoriController;

    [Header("Values")] [SerializeField] private float holdDuration;

    private int _dialogueCnt;
    private string _dialogueText;
    private string[] _optionsText;
    private int _prevOption;

    public void Init()
    {
        _dialogueCnt = 0;
        _dialogueText = "";
        _optionsText = new string[3];
        
        optionsParent.SetActive(false);
        dialogue.text = "";
        foreach(TextMeshProUGUI text in options)
            text.text = string.Empty;
    }

    public void StartConversation(int selectedOption = -1)
    {
        switch (_dialogueCnt)
        {
            case 0:
                _dialogueText = "안녕? 만나서 반가워, 내 이름은 벼리야!";
                _optionsText[0] = "안녕!";
                _optionsText[1] = "반가워~";
                _optionsText[2] = "넌 뭐야?";
                break;
            
            case 1:
                byeoriController.SetByeoriSprite(1);
                _dialogueText = "나는 너의 고민을 상담해주려고 찾아 온 존재야! 요즘에 어떤 고민을 가지고 있는지 물어봐도 될까?";
                _optionsText[0] = "학업이 너무 힘들어";
                _optionsText[1] = "업무 스트레스가 너무 커";
                _optionsText[2] = "인간 관계 때문에 고민이야";
                break;
            
            case 2:
                byeoriController.SetByeoriSprite(2);
                _prevOption = selectedOption;
                switch (selectedOption)
                {
                    case 0:
                        _dialogueText = "공부 때문에 고민이구나... 어떤 부분이 제일 부담돼?";
                        _optionsText[0] = "시험이 다가오고 있어서 두려워";
                        _optionsText[1] = "과제가 너무 많아서 힘들어";
                        _optionsText[2] = "너무 오랜 기간 쉬지를 못했어";
                        break;
                    case 1:
                        _dialogueText = "업무 스트레스 때문에 고민이구나... 어떤 부분이 제일 부담돼?";
                        _optionsText[0] = "직장 상사 때문에 화가 나";
                        _optionsText[1] = "업무량이 너무 많아서 힘들어";
                        _optionsText[2] = "너무 오랜 기간 쉬지를 못했어";
                        break;
                    case 2:
                        _dialogueText = "인간 관계가 힘들구나... 어떤 부분이 제일 고민이야?";
                        _optionsText[0] = "요즘 외로움을 많이 타는 것 같아";
                        _optionsText[1] = "가족과 관계가 멀어지고 있어";
                        _optionsText[2] = "친구와 다퉈서 서먹해졌어";
                        break;
                }
                break;
            
            case 3:
                byeoriController.SetByeoriSprite(0);
                _dialogueText = 
                    _prevOption == 2 ? "우선 원인을 찾아보자! 천천히 시간을 가지고 생각을 정리하면 자연스레 마음이 정리될거야!"
                    : "우선 해야 할 일을 차근차근 정리하고, 스트레스를 해소하는 방법을 찾아보자! 충분한 수면과 가벼운 운동이 분명 도움이 될거야!";
                _optionsText[0] = "좋은 방법인 것 같아!";
                _optionsText[1] = "한번 시도해 볼게";
                _optionsText[2] = "글쎄, 별로 도움이 될 것 같지 않아";
                break;
            
            case 4:
                switch (selectedOption)
                {
                    case 0: case 1:
                        byeoriController.SetByeoriSprite(1);
                        _dialogueText = "또 다른 고민이 생기면 언제든지 나한테 얘기해줘!\n우리 또 만나자!";
                        break;
                    case 2:
                        byeoriController.SetByeoriSprite(2);
                        _dialogueText = "도움이 되지 못해서 미안해. 하지만 너라면 반드시 좋은 해결책을 찾아낼거야.\n우리 또 만나자!";
                        break;
                }

                StartCoroutine(EndConversation());
                return;
        }

        StartCoroutine(ShowDialogue());
        _dialogueCnt++;
    }

    private IEnumerator ShowDialogue()
    {
        dialogue.text = _dialogueText;
        yield return new WaitForSeconds(holdDuration);
        for (int i = 0; i < 3; i++)
            options[i].text = _optionsText[i];
        optionsParent.SetActive(true);
    }

    public void SelectOption0()
    {
        StartConversation(0);
        optionsParent.SetActive(false);
    }
    public void SelectOption1()
    {
        StartConversation(1);
        optionsParent.SetActive(false);
    }
    public void SelectOption2()
    {
        StartConversation(2);
        optionsParent.SetActive(false);;
    }

    private IEnumerator EndConversation()
    {
        yield return new WaitForSeconds(4f);
        StartCoroutine(OptionSceneManager.Instance.EndConversation());
    }
}
