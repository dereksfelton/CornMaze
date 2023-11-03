using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
   public static GameStateManager Instance;

   public bool GameInProgress = false;
   public Vector3 PlayerPosition = Vector3.zero;

   public void Awake()
   {
      // ensure that only one instance of GameStateManager is active
      if (Instance != null)
      {
         Destroy(gameObject);
         return;
      }

      Instance = this;
      DontDestroyOnLoad(gameObject);
   }
}
