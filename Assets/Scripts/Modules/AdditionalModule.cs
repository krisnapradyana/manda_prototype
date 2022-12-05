using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Modules
{
    public static class AdditionalModule
    {
        public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
            screenPoint.z = 0;

            Vector2 screenPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
            {
                return screenPos;
            }

            return screenPoint;
        }
        public static Vector3 GetWorldPoint()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            Vector3 point = Vector3.zero;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                point = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
            return point;
        }

    }
}

