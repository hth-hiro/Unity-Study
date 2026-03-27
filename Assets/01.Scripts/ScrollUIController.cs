using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ScrollUIController : MonoBehaviour, IPointerClickHandler
{
    [Header("Root")]
    [SerializeField] private RectTransform scrollRoot;
    [SerializeField] private CanvasGroup rootCanvasGroup;

    [Header("Unroll Parts")]
    [SerializeField] private RectTransform paperBody;
    [SerializeField] private RectTransform maskArea;
    [SerializeField] private RectTransform bottomEdge;

    [Header("Animation")]
    [SerializeField] private float introFadeDuration = 0.2f;
    [SerializeField] private float unfoldDuration = 0.45f;
    [SerializeField] private float targetHeight = 600f;
    [SerializeField] private float introMoveY = -40f;

    private Vector2 _scrollRootStartPos;
    private Vector2 _paperBodyInitialSize;
    private Vector2 _maskAreaInitialSize;
    private Vector2 _bottomEdgeInitialPos;

    private Coroutine _animRoutine;

    private void Awake()
    {
        if (scrollRoot == null)
            scrollRoot = GetComponent<RectTransform>();

        _scrollRootStartPos = scrollRoot.anchoredPosition;
        if (paperBody != null) _paperBodyInitialSize = paperBody.sizeDelta;
        if (maskArea != null) _maskAreaInitialSize = maskArea.sizeDelta;

        if (bottomEdge != null)
            _bottomEdgeInitialPos = bottomEdge.anchoredPosition;
    }

    private void Start()
    {
        PlayOpen();
    }

    public void PlayOpen()
    {
        if (_animRoutine != null) StopCoroutine(_animRoutine);

        rootCanvasGroup.alpha = 0f;
        scrollRoot.anchoredPosition = _scrollRootStartPos + new Vector2(0f, -introMoveY);

        if (paperBody != null) paperBody.sizeDelta = new Vector2(_paperBodyInitialSize.x, 0f);
        if (maskArea != null) maskArea.sizeDelta = new Vector2(_maskAreaInitialSize.x, 0f);

        if (bottomEdge != null)
        {
            bottomEdge.anchoredPosition = new Vector2(
                _bottomEdgeInitialPos.x,
                _bottomEdgeInitialPos.y
            );
        }

        _animRoutine = StartCoroutine(OpenRoutine());
    }

    private IEnumerator OpenRoutine()
    {
        float t = 0f;

        // Intro
        while (t < introFadeDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / introFadeDuration);
            float easeP = EaseOutCubic(p);

            rootCanvasGroup.alpha = p;
            scrollRoot.anchoredPosition = Vector2.Lerp(
                _scrollRootStartPos + new Vector2(0f, -introMoveY),
                _scrollRootStartPos,
                easeP
            );
            yield return null;
        }

        rootCanvasGroup.alpha = 1f;
        scrollRoot.anchoredPosition = _scrollRootStartPos;

        // Unfold + BottomEdge 동시 실행
        t = 0f;
        while (t < unfoldDuration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / unfoldDuration);
            float easeP = EaseOutBack(p);

            float currentHeight = Mathf.Lerp(0f, targetHeight, easeP);

            if (paperBody != null)
                paperBody.sizeDelta = new Vector2(_paperBodyInitialSize.x, currentHeight);

            if (maskArea != null)
                maskArea.sizeDelta = new Vector2(_maskAreaInitialSize.x, currentHeight);

            if (bottomEdge != null)
            {
                bottomEdge.anchoredPosition = new Vector2(
                    _bottomEdgeInitialPos.x,
                    _bottomEdgeInitialPos.y - currentHeight
                );
            }

            yield return null;
        }

        if (paperBody != null)
            paperBody.sizeDelta = new Vector2(_paperBodyInitialSize.x, targetHeight);

        if (maskArea != null)
            maskArea.sizeDelta = new Vector2(_maskAreaInitialSize.x, targetHeight);

        if (bottomEdge != null)
        {
            bottomEdge.anchoredPosition = new Vector2(
                _bottomEdgeInitialPos.x,
                _bottomEdgeInitialPos.y - targetHeight
            );
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySelectPunch();
    }

    public void PlaySelectPunch()
    {
        if (_animRoutine != null && rootCanvasGroup.alpha >= 1f) StopCoroutine(_animRoutine);
        StartCoroutine(PunchScaleRoutine());
    }

    private IEnumerator PunchScaleRoutine()
    {
        float t = 0f;
        float duration = 0.2f;
        Vector3 startScale = Vector3.one;
        Vector3 punchScale = startScale + Vector3.one * 0.05f;

        while (t < duration * 0.5f)
        {
            t += Time.deltaTime;
            scrollRoot.localScale = Vector3.Lerp(startScale, punchScale, EaseOutCubic(t / (duration * 0.5f)));
            yield return null;
        }

        while (t < duration)
        {
            t += Time.deltaTime;
            scrollRoot.localScale = Vector3.Lerp(punchScale, startScale, EaseOutCubic((t - duration * 0.5f) / (duration * 0.5f)));
            yield return null;
        }

        scrollRoot.localScale = startScale;
    }

    private float EaseOutCubic(float x)
    {
        return 1f - Mathf.Pow(1f - x, 3f);
    }

    private float EaseOutBack(float x)
    {
        const float c1 = 1.70158f;
        const float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(x - 1f, 3f) + c1 * Mathf.Pow(x - 1f, 2f);
    }
}