using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codeTMP;
    
    private void Start()
    {
        string code = $"문자 코드: {GameManager.Instance.code}" +
                      $"\n숫자 코드: {GameManager.Instance.dialogueCnt}";
        
        codeTMP.text = code;
    }
}
