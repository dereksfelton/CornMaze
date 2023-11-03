using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class InputManager : MonoBehaviour
{
   private PlayerInput playerInput;
   private PlayerInput.OnFootActions onFoot;

   private PlayerMotor motor;
   private PlayerLook look;

   void Awake()
   {
      playerInput = new PlayerInput();
      onFoot = playerInput.OnFoot;

      motor = GetComponent<PlayerMotor>();
      look = GetComponent<PlayerLook>();

      onFoot.Jump.performed += ctx => motor.Jump();
   }

   void FixedUpdate()
   {
      // get movement vector from player input
      Vector2 horizontalMovement = onFoot.Movement.ReadValue<Vector2>();

      // tell player motor to move based on the value from movement action
      motor.ProcessMove(horizontalMovement);
   }

   private void LateUpdate()
   {
      look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
   }

   private void OnEnable()
   {
      onFoot.Enable();

      // specify method to call when escape key is pressed
      onFoot.GameControl.performed += OnGameControlActionPerformed;
   }

   private void OnDisable()
   {
      onFoot.Disable();

      // unsubscribe to the escape key event
      onFoot.GameControl.performed -= OnGameControlActionPerformed;
   }

   // because we told this method to "subscribe to" .performed events in OnEnable,
   // it will be called when 1) the input system is enabled, and 2) a "performed"
   // event is registered by the input system ... but this time, within the
   // GameControl set of actions
   private void OnGameControlActionPerformed(InputAction.CallbackContext context)
   {
      // some fairly gnarly code to get which control "path" we bound to...
      // recent comments online agree that Unity's API should make this much
      // easier to retrieve.
      var binding = context.action.GetBindingForControl(context.control);

      // dig down into that binding to get the path we set (again, this should be easier)
      var bindingPath = binding.Value.effectivePath;

      // if we performed a generic Back action, return to the main menu
      if (bindingPath == "*/{Back}")
      {
         // if the game state manager exists...
         if (GameStateManager.Instance != null)
         {
            // store current position of player to the data manager
            GameStateManager.Instance.PlayerPosition = transform.position;
         }

         // transition back to the main menu scene
         SceneManager.LoadScene("MainMenu");
      }
   }
}
