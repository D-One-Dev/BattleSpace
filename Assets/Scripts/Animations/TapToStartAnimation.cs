using DG.Tweening;
using TMPro;
using UnityEngine;

public class TapToStartAnimation : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        text.DOFade(0f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
