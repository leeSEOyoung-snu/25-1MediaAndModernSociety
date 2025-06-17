using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ByeoriController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image byeoriImage;
    
    [Header("Values")]
    [SerializeField] private Sprite[] byeoriSprites;
    [SerializeField] private float fadeDuration;

    public void Init()
    {
        byeoriImage.sprite = byeoriSprites[0];
        byeoriImage.color = Color.clear;
    }
    
    public void SetByeoriSprite(int idx)
    {
        // 0 == 일반
        // 1 == 행복
        // 2 == 슬픔
        byeoriImage.sprite = byeoriSprites[idx];
    }

    public IEnumerator FadeIn()
    {
        byeoriImage.DOColor(Color.white, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }
}
