using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 using UnityEngine.SceneManagement;


public class GoPlayerController : MonoBehaviour
{
    public InputActionReference resetInput;
    private void Awake() {
        resetInput.action.started += ResetScene;
    }
    private void OnDestroy() {
        resetInput.action.started -= ResetScene;
    }
    void ResetScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
