using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ICanvasRaycastFilter
{
    public static JumpButton instance;
    public float Radius;
    public float neededRadius = -1;
    public bool canJump = false;
    private RectTransform rectTransform;
    public bool isHoldingDown = false;
    Camera camera;

    void Awake()
    {
        instance = this;
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        camera = CompleteCameraController.instance.GetComponent<Camera>();
    }

    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        Vector2 pivotToCursorVector;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPoint, eventCamera, out pivotToCursorVector);

        Vector2 pivotOffsetRatio = rectTransform.pivot - new Vector2(0.5f, 0.5f);
        Vector2 pivotOffset = Vector2.Scale(rectTransform.rect.size, pivotOffsetRatio);
        Vector2 centerToCursorVector = pivotToCursorVector + pivotOffset;
        return (centerToCursorVector.magnitude < Radius);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        neededRadius = eventData.position.x;
        if (eventData != null && IsRaycastLocationValid(eventData.position, camera))
        {
            UnitBase._TEMPINSTANCE.PlayerJump();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData != null)
            isHoldingDown = false;
    }
}
