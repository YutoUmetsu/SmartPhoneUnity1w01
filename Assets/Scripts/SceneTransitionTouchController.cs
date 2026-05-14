using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class SceneTransitionTouchController : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName = "NextScene"; // 遷移先シーン名

    [SerializeField]
    private float maxTapDistance = 80f; // タップと判定する最大移動距離

    private Vector2 startPosition;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
#if UNITY_EDITOR
        UpdateMouseInput();
#else
        UpdateTouchInput();
#endif
    }

    void UpdateMouseInput()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPosition = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector2 endPosition = Mouse.current.position.ReadValue();
            CheckTapAndTransition(endPosition);
        }
    }

    void UpdateTouchInput()
    {
        if (Touch.activeTouches.Count == 0)
        {
            return;
        }

        Touch touch = Touch.activeTouches[0];

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
        {
            startPosition = touch.screenPosition;
        }

        if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
        {
            Vector2 endPosition = touch.screenPosition;
            CheckTapAndTransition(endPosition);
        }
    }

    void CheckTapAndTransition(Vector2 endPosition)
    {
        float distance = Vector2.Distance(startPosition, endPosition);

        // 指がほとんど動いていなければ「タップ」と判定してシーン遷移
        if (distance < maxTapDistance)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
