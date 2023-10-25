using UnityEngine;

public class CornPainter : MonoBehaviour
{
   private Terrain t;

   private void Awake()
   {
      t = GetComponent<Terrain>();
   }
}
