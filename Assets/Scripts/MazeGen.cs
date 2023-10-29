using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
   public string MapFileName;
   public float positionOffsetX = 1.5f;
   public float positionOffsetY = 1.5f;
   public float VertScale = 1f;
   public bool ShowWalls = true;
   public Material WallMaterial = null;

   // where to start the maze relative to the detail grid
   private int detailGridOffsetX = 8;
   private int detailGridOffsetY = 8;
   private int detailWidth, detailHeight;
   private float terrainWidth, terrainHeight;

   private char[,] grid = null;
   private List<Wall> walls;

   private static char WALL_CHAR = '#';
   private static char vSep = Path.VolumeSeparatorChar;
   private static char dSep = Path.DirectorySeparatorChar;
   private static DirectoryInfo mapsDir = new DirectoryInfo($"Assets{dSep}Mazes");

   private string mapFilePath = string.Empty;
   private string mapFileBitmapPath = string.Empty;

   private Terrain terrain = null;

   // Start is called before the first frame update
   void Start()
   {
      // set terrain to active terrain
      terrain = Terrain.activeTerrain;

      // read dimensions of detail grid
      detailWidth = terrain.terrainData.detailWidth;
      detailHeight = terrain.terrainData.detailHeight;

      terrainWidth = terrain.terrainData.size.x;
      terrainHeight = terrain.terrainData.size.z;

      float detailBoxWidth = terrainWidth / detailWidth;
      float detailBoxHeight = terrainHeight / detailHeight;

      // place maze based on detail grid dimensions and offsets
      Vector3 pos = new Vector3(
         detailGridOffsetX * detailBoxWidth + positionOffsetX,
         0f,
         detailGridOffsetY * detailBoxHeight + positionOffsetY);
      transform.SetPositionAndRotation(pos, Quaternion.identity);
      transform.localScale = new Vector3(detailBoxWidth, 1f, detailBoxHeight);

      mapFilePath = $"{mapsDir.FullName}{dSep}{MapFileName}.txt";
      mapFileBitmapPath = $"{mapsDir.FullName}{dSep}{MapFileName}.bmp";

      LoadGrid();
      IdentifyWalls();
      GenerateWalls();
      PlaceCornstalks();
   }

   private void ReadBitmap() 
   {
      byte[] mapByteAry = File.ReadAllBytes(mapFileBitmapPath);
   }

   private void LoadGrid()
   {
      // read a text file using StreamReader
      using (StreamReader reader = new StreamReader(mapFilePath))
      {
         // read entire contents of file, trim any white psace at the end
         string contents = reader.ReadToEnd().TrimEnd();
         
         // split the rows on NewLine character(s) ... e.g., \r\n
         string[] rows = contents.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

         // calculate the dimensions of the maze
         int colCount = rows[0].Length;
         int rowCount = rows.Length;

         // instantiate the grid to represent the maze
         grid = new char[colCount, rowCount];

         // loop through each row
         for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
         {
            // loop through each column in the current row
            for (int colIndex = 0; colIndex < colCount; colIndex++)
            {
               // read the character from the string
               char c = rows[rowIndex][colIndex];
               
               // if it's a wall character, transpose it to the grid in the same position
               if (char.Equals(c, WALL_CHAR))
               {
                  grid[colIndex, rowIndex] = c;
               }
            }
         }
      }
   }

   private void IdentifyWalls()
   {
      walls = new List<Wall>();

      // here's where various algorithms would identify walls
      // differently, and hopefully more efficiently

      int cols = grid.GetLength(0);
      int rows = grid.GetLength(1);

      for (int rowIndex = 0; rowIndex < cols; rowIndex++)
      {
         for (int colIndex = 0; colIndex < rows; colIndex++)
         {
            if (char.Equals(grid[rowIndex, colIndex], WALL_CHAR))
            {
               Vector2 loc = new Vector2(rowIndex, colIndex);
               Wall newWall = new Wall(loc, 1, 1);
               walls.Add(newWall);
            }
         }
      }
   }

   private void GenerateWalls()
   {
      Debug.Log($"Wall count: {walls.Count}");

      // FOR REFERENCE: how to load a marerial from code
      // Material wallMaterial = Resources.Load("Materials/BasicWalls", typeof(Material)) as Material;

      foreach (Wall wall in walls)
      {
         GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
         cube.transform.parent = this.gameObject.transform;
         MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
         renderer.enabled = ShowWalls;
         if (ShowWalls)
         {
            renderer.material = WallMaterial;
         }
         cube.transform.localPosition = new Vector3(wall.Location.x, 0.0f, wall.Location.y);
         cube.transform.localScale = new Vector3(wall.Width, 1f * VertScale, wall.Length);
      }
   }

   private void PlaceCornstalks()
   {
      // TODO: determine which index refers to the Cornstalk layer
      var map = terrain.terrainData.GetDetailLayer(0, 0, detailWidth, detailHeight, 0);

      // clear existing detailLayer
      for (int x=0; x < detailWidth; x++)
      {
         for (int y=0; y < detailHeight; y++) 
         {
            map[x, y] = 0;
         }
      }

      foreach (Wall wall in walls)
      {
         map[(int)wall.Location.y + detailGridOffsetY, (int)wall.Location.x + detailGridOffsetX] = 255;
      }

      terrain.terrainData.SetDetailLayer(0, 0, 0, map);
   }
}