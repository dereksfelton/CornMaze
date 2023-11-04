using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerMotor : MonoBehaviour
{
   private CharacterController controller;
   private AudioSource audioSource;
   private AudioSource adHocAudioSource;

   private Vector3 playerVelocity;
   private bool isGrounded;

   public float speed = 5f;
   public float gravity = -9.8f;
   public float jumpHeight = 3f;

   // NOTE: There is almost certainly a better way to specify/manage sounds!
   public AudioClip pickupSound;

   void Awake()
   {
      controller = GetComponent<CharacterController>();
      audioSource = GetComponent<AudioSource>();

      adHocAudioSource = transform.Find("AdHocAudio").GetComponent<AudioSource>();

      if (GameStateManager.Instance != null)
      {
         // disable player controller before moving player, reenable it after
         gameObject.GetComponent<CharacterController>().enabled = false;
         transform.position = GameStateManager.Instance.PlayerPosition;
         gameObject.GetComponent<CharacterController>().enabled = true;
      }
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

   // handle player jumps
   public void Jump()
   {
      if (isGrounded)
      {
         playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
      }
   }

   // handle collisions
   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Pickup"))
      {
         // do stuff on the player that would happen with a pickup
         Debug.Log("Player says: I picked up " + other.gameObject.name);

         other.gameObject.SetActive(false); // remove pickup from scene

         adHocAudioSource.PlayOneShot(pickupSound);
      }
   }
}
