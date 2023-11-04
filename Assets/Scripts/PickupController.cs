using System.Collections;
using UnityEngine;

public class PickupController : MonoBehaviour
{
   private AudioSource audio;

   private void Start()
   {
      audio = GetComponent<AudioSource>();
   }

   // Update is called once per frame
   void Update()
   {
      transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);
   }
}
