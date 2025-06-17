using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int dialogueCnt;
    public string code;
    public bool llmFirst;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        dialogueCnt = 0;
        
        int tmp = UnityEngine.Random.Range(0, 2);

        if (tmp == 0)
        {
            llmFirst = true;
            code = "HOME";
        }
        else
        {
            llmFirst = false;
            code = "집에보내줘\n(띄어쓰기 없음 주의)";
        }
    }

    public void EndLlm()
    {
        if (llmFirst) SceneManager.LoadScene("Option");
        else SceneManager.LoadScene("Code");
    }

    public void EndOption()
    {
        if (llmFirst) SceneManager.LoadScene("Code");
        else SceneManager.LoadScene("LLM");
    }

    public void StartGame()
    {
        if (llmFirst) SceneManager.LoadScene("LLM");
        else SceneManager.LoadScene("Option");
    }
}
