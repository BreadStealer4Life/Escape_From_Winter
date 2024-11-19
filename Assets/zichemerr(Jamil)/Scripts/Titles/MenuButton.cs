using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour
{
    private Button _button;
    private Action _clicked;

    public void Init(Action clicked)
    {
        _clicked = clicked;
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _clicked?.Invoke();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}