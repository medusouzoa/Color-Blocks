using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
  public class MenuUIController : MonoBehaviour
  {
    public void OnOpenGameScene()
    {
      SceneManager.LoadScene("Scenes/GameScene");
    }

    public void QuitGame()
    {
      Application.Quit();
    }
  }
}