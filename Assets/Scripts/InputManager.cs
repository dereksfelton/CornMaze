using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
   }

   private void OnDisable()
   {
      onFoot.Disable();
   }
}
