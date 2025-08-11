using Platformer.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public enum TriggerMode { OnRelease, OnRepress }

    [SerializeField] TriggerMode mode = TriggerMode.OnRelease;
    [SerializeField] ButtonEvent[] onPressEvents;
    [SerializeField] ButtonEvent[] onReleaseEvents;

    private readonly HashSet<Collider2D> pressingObjects = new();
    private bool isPressed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsValidPresser(other)) return;

        pressingObjects.Add(other);
        UpdateButtonState();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsValidPresser(other)) return;

        pressingObjects.Remove(other);
        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        bool shouldBePressed = pressingObjects.Count > 0;

        // 状态变化检测
        if (shouldBePressed == isPressed) return;

        // 状态变更处理
        if (shouldBePressed) HandlePress();
        else HandleRelease();

        isPressed = shouldBePressed;
    }

    private void HandlePress()
    {
        foreach (var e in onPressEvents) e.Trigger();
        if (mode == TriggerMode.OnRepress) HandleRelease();
    }

    private void HandleRelease()
    {
        if (mode != TriggerMode.OnRelease) return;
        foreach (var e in onReleaseEvents) e.Trigger();
    }

    private bool IsValidPresser(Collider2D col)
    {
        return col.CompareTag("Player") || col.CompareTag("Box");
    }

    public void ResetButton()
    {
        pressingObjects.Clear();
        isPressed = false;
    }
}