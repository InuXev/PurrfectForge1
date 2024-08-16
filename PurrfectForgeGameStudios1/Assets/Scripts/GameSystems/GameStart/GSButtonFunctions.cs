using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GSButtonFunctions : MonoBehaviour
{
    #region Fields/Objects

    [SerializeField] GSGameManager gameManager;
    public Button targetButton;

    #endregion

    #region Buttons

    void Start()
    {
        if (gameManager == null)
        {
            Debug.LogError("GameManager is not assigned in GSButtonFunctions!");
        }
    }
    public void LoadGame()
    {
        gameManager.LoadGame();
    }
    public void NewGame()
    {
        gameManager.NewGame();
    }
    public void quit2()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    #endregion
}