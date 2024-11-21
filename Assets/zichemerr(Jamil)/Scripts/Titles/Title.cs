using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] private RectTransform _endPositionPoint;
    [SerializeField] private float _durationTitle;

    [Header("Button")]
    [SerializeField] private MenuButton _menuButton;
    [SerializeField] private float _endPositionY;
    [SerializeField] private float _durationButton;

    private RectTransform _rectTransform;
    private Sequence _sequence;

    private void Awake()
    {
        _sequence = DOTween.Sequence();
        _rectTransform = GetComponent<RectTransform>();
        _menuButton.Init(LoadMainScene);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        _sequence.Append(
            _rectTransform.DOMoveY(_endPositionPoint.position.y, _durationTitle)
            .SetEase(Ease.Linear))
            .onComplete += PlayButtonAnimation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _sequence.Kill();
            _rectTransform.position = new Vector2(_rectTransform.position.x, _endPositionPoint.position.y);
            PlayButtonAnimation();
        }
    }

    private void PlayButtonAnimation()
    {
        _menuButton.Enable();
        _menuButton.transform.DOMoveY(_endPositionY, _durationButton);
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
