using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class OptionSceneManager : MonoBehaviour
{
    public static OptionSceneManager Instance { get; private set; }
    
    [Header("References")]
    [SerializeField] private ByeoriController byeoriController;
    [SerializeField] private OptionDialogueManager optionDialogueManager;
    [SerializeField] private GameObject bottomParent;
    [SerializeField] private Image fadePanelImg;
    
    [Header("Values")]
    [SerializeField] private float fadeDuration;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        Init();
    }
    private void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    private void Init()
    {
        bottomParent.SetActive(false);
        optionDialogueManager.Init();
        byeoriController.Init();
    }
    
    private IEnumerator InitCoroutine()
    {
        yield return FadeIn();
        yield return byeoriController.FadeIn();
        optionDialogueManager.StartConversation();
        bottomParent.SetActive(true);
    }
    
    private IEnumerator FadeIn()
    {
        fadePanelImg.color = Color.black;
        fadePanelImg.gameObject.SetActive(true);
        
        fadePanelImg.DOColor(Color.clear, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        
        fadePanelImg.gameObject.SetActive(false);
    }
    
    private IEnumerator FadeOut()
    {
        fadePanelImg.color = Color.clear;
        fadePanelImg.gameObject.SetActive(true);
        
        fadePanelImg.DOColor(Color.black, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }
    
    public IEnumerator EndConversation()
    {
        yield return FadeOut();
        GameManager.Instance.EndOption();
    }
}
