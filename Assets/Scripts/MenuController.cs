using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
   public void Start()
   {
      if (GameStateManager.Instance.GameInProgress)
      {
         // find the Resume Game button
         Button resumeButton = GameObject.Find("BtnResumeGame").GetComponent<Button>();

         // reenable Resume Game button
         resumeButton.interactable = true;
      }
   }

   public void NewGameOnClick()
   {
      GameStateManager.Instance.GameInProgress = true;
      GameStateManager.Instance.PlayerPosition = new Vector3(48, 0.5f, 17);

      SceneManager.LoadScene("Maze");
   }

   public void ResumeGameOnClick()
   {
      SceneManager.LoadScene("Maze");
   }

   public void QuitOnClick()
   {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#endif
      Application.Quit();
   }
}
