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
    private Material _paperMaterial;

    private Coroutine _animRoutine;

    private void Awake()
    {
        if (scrollRoot == null)
            scrollRoot = GetComponent<RectTransform>();

        _scrollRootStartPos = scrollRoot.anchoredPosition;
        if (paperBody != null)
        {
            _paperBodyInitialSize = paperBody.sizeDelta;
            var img = paperBody.GetComponent<UnityEngine.UI.Image>();
            if (img != null && img.material != null)
            {
                img.material = new Material(img.material);
                _paperMaterial = img.material;
            }
        }
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

        if (_paperMaterial != null)
        {
            _paperMaterial.SetFloat("_UnrollProgress", 0f);
            _paperMaterial.SetFloat("_BottomWobbleStrength", 0f);
            _paperMaterial.SetFloat("_BottomBendStrength", 0f);
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

        // Unfold
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
                float wobbleDamping = 1f - (p * 0.75f); // �ʹ� ���� �� �װ�
                float wobbleX = Mathf.Sin(p * 18f) * 12f * wobbleDamping;
                float wobbleRot = Mathf.Sin(p * 16f) * 6f * wobbleDamping;

                bottomEdge.anchoredPosition = new Vector2(
                    _bottomEdgeInitialPos.x + wobbleX,
                    _bottomEdgeInitialPos.y - currentHeight
                );

                bottomEdge.localRotation = Quaternion.Euler(0f, 0f, wobbleRot);
            }

            if (_paperMaterial != null)
            {
                float wobbleDamping = 1f - (p * 0.75f);
                float wobbleX = Mathf.Sin(p * 18f) * 12f * wobbleDamping;
                
                // 펼침 진행도 및 펼침 중 좌우 흔들림/휨을 셰이더로 전달
                _paperMaterial.SetFloat("_UnrollProgress", easeP);
                _paperMaterial.SetFloat("_BottomWobbleStrength", wobbleX / 12f);
                _paperMaterial.SetFloat("_BottomBendStrength", Mathf.Sin(p * Mathf.PI) * 0.05f);
            }

            yield return null;
        }

        if (bottomEdge != null)
        {
            float settleTime = 0.22f;
            float settle = 0f;

            Vector2 settleStartPos = new Vector2(
                _bottomEdgeInitialPos.x,
                _bottomEdgeInitialPos.y - targetHeight - 12f   // �Ʒ��� ������Ʈ
            );

            while (settle < settleTime)
            {
                settle += Time.deltaTime;
                float sp = Mathf.Clamp01(settle / settleTime);

                float bounceX = Mathf.Sin(sp * 20f) * 5f * (1f - sp);
                float bounceRot = Mathf.Sin(sp * 22f) * 4f * (1f - sp);

                float y = Mathf.Lerp(
                    _bottomEdgeInitialPos.y - targetHeight - 12f,
                    _bottomEdgeInitialPos.y - targetHeight,
                    EaseOutCubic(sp)
                );

                bottomEdge.anchoredPosition = new Vector2(
                    _bottomEdgeInitialPos.x + bounceX,
                    y
                );

                bottomEdge.localRotation = Quaternion.Euler(0f, 0f, bounceRot);

                // 오버슈트에 의해 목표치보다 오버된 길이 산출 (최대 12f)
                float extraStretch = (_bottomEdgeInitialPos.y - y) - targetHeight;
                
                // 오버슈트 되는 길이만큼 PaperBody와 MaskArea도 강제로 늘려줌으로써 하단 빈틈 방지 & 물리적 확장 통일 
                if (paperBody != null)
                    paperBody.sizeDelta = new Vector2(_paperBodyInitialSize.x, targetHeight + extraStretch);
                if (maskArea != null)
                    maskArea.sizeDelta = new Vector2(_maskAreaInitialSize.x, targetHeight + extraStretch);

                if (_paperMaterial != null)
                {
                    // 정착 시 튕겨올라오는 흔들림과 늘어진 여운을 셰이더 수치에 연동
                    _paperMaterial.SetFloat("_BottomWobbleStrength", bounceX / 5f);
                    _paperMaterial.SetFloat("_BottomBendStrength", extraStretch / 12f * 0.08f);
                }

                yield return null;
            }

            bottomEdge.anchoredPosition = new Vector2(
                _bottomEdgeInitialPos.x,
                _bottomEdgeInitialPos.y - targetHeight
            );
            bottomEdge.localRotation = Quaternion.identity;
        }

        if (paperBody != null)
            paperBody.sizeDelta = new Vector2(_paperBodyInitialSize.x, targetHeight);

        if (maskArea != null)
            maskArea.sizeDelta = new Vector2(_maskAreaInitialSize.x, targetHeight);

        // 연동 수치 리셋
        if (_paperMaterial != null)
        {
            _paperMaterial.SetFloat("_UnrollProgress", 1f);
            _paperMaterial.SetFloat("_BottomWobbleStrength", 0f);
            _paperMaterial.SetFloat("_BottomBendStrength", 0f);
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