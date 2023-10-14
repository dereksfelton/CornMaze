using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
   private CharacterController controller;
   private AudioSource audioSource;

   private Vector3 playerVelocity;
   private bool isGrounded;

   public float speed = 5f;
   public float gravity = -9.8f;
   public float jumpHeight = 3f;

   // Start is called before the first frame update
   void Start()
   {
      controller = GetComponent<CharacterController>();
      audioSource = GetComponent<AudioSource>();
   }

   // Update is called once per frame
   void Update()
   {
      isGrounded = controller.isGrounded;
   }

   // receive inputs from our InputManager.cs and apply them to our character controller
   public void ProcessMove(Vector2 input)
   {
      Vector3 moveDirection = Vector3.zero;
      moveDirection.x = input.x;
      moveDirection.z = input.y;

      controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
      playerVelocity.y += gravity * Time.deltaTime;

      if (isGrounded && playerVelocity.y < 0)
      {
         playerVelocity.y = -2f;
      }

      controller.Move(playerVelocity * Time.deltaTime);

      // control walking sound...............................................
      Debug.Log($"mX: {moveDirection.x}\nmZ: {moveDirection.z}");

      // play walking sound if moving
      if (moveDirection.x != 0.0 || moveDirection.z != 0.0)
      {
         if (!audioSource.isPlaying)
         {
            audioSource.Play();
            audioSource.volume = 1.0f;
         }
      }
      // otherwise stop walking sound
      else
      {
         if (audioSource.isPlaying)
         {
            audioSource.volume -= 2.0f * Time.deltaTime;

            if (audioSource.volume <= 0f)
            {
               audioSource.Stop();
            }
         }
      }
   }

   // hanlde player jumps
   public void Jump()
   {
      if (isGrounded)
      {
         playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
      }
   }
}
