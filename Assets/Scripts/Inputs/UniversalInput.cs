using UnityEngine;

public static class UniversalInput {
    public static bool GetInputDown(out Vector2 position) {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0)) {
            position = Input.mousePosition;
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                position = touch.position;
                return true;
            }
        }
#endif
        position = Vector2.zero;
        return false;
    }

    public static bool GetInputHeld(out Vector2 position) {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0)) {
            position = Input.mousePosition;
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                position = touch.position;
                return true;
            }
        }
#endif
        position = Vector2.zero;
        return false;
    }

    public static bool GetInputUp(out Vector2 position) {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButtonUp(0)) {
            position = Input.mousePosition;
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                position = touch.position;
                return true;
            }
        }
#endif
        position = Vector2.zero;
        return false;
    }
}
