using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
   public Camera cam;

   public float xSensitivity = 30f;
   public float ySensitivity = 30f;

   private float xRotation = 0f;

   public void ProcessLook(Vector2 input)
   {
      float mouseX = input.x;
      float mouseY = input.y;

      // calculate cam rotation for looking up and down
      xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
      xRotation = Mathf.Clamp(xRotation, -80f, 80f);

      // apply to camera rotation
      cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

      // rotate player to look left and right
      transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
   }
}
