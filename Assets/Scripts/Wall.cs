using UnityEngine;

namespace Assets.Scripts
{
   internal class Wall
   {
      public Vector2 Location { get; private set; }
      public float Width { get; private set; }
      public float Length { get; private set; }

      public Wall(Vector2 location, float width, float length)
      {
         this.Location = location;
         this.Width = width;
         this.Length = length;
      }
   }
}
