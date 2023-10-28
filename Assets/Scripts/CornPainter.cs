using UnityEngine;

public class CornPainter : MonoBehaviour
{
   private Terrain t;

   private void Awake()
   {
      t = GetComponent<Terrain>();

      var map = t.terrainData.GetDetailLayer(0, 0, t.terrainData.detailWidth, t.terrainData.detailHeight, 0);

      // For each pixel in the detail map...
      for (var y = 0; y < t.terrainData.detailHeight; y++)
      {
         for (var x = 0; x < t.terrainData.detailWidth; x++)
         {
            map[x, y] = (x * y % 2 == 0) ? 0 : 255;
         }
      }

      // Assign the modified map back.
      t.terrainData.SetDetailLayer(0, 0, 0, map);
   }
}
