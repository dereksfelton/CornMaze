using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
   public string MapFileName;
   public float HorizScale = 1f;
   public float VertScale = 1f;
   public bool ShowWalls = true;
   public Material WallMaterial = null;

   private char[,] grid = null;
   private List<Wall> walls;

   private static char WALL_CHAR = '#';
   private static char vSep = Path.VolumeSeparatorChar;
   private static char dSep = Path.DirectorySeparatorChar;
   private static DirectoryInfo mapsDir = new DirectoryInfo($"Assets{dSep}Mazes");

   private string mapFilePath = string.Empty;
   private string mapFileBitmapPath = string.Empty;

   // Start is called before the first frame update
   void Start()
   {
      mapFilePath = $"{mapsDir.FullName}{dSep}{MapFileName}.txt";
      mapFileBitmapPath = $"{mapsDir.FullName}{dSep}{MapFileName}.bmp";

      LoadGrid();
      IdentifyWalls();
      GenerateWalls();
      PlaceGrass();
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
               Vector2 loc = new Vector2(rowIndex, colIndex) * HorizScale;
               Wall newWall = new Wall(loc, 1 * HorizScale, 1 * HorizScale);
               walls.Add(newWall);
            }
         }
      }
   }

   private void GenerateWalls()
   {
      Debug.Log($"Wall count: {walls.Count}");

      GameObject wallsGroup = new GameObject("MazeWalls");


      // FOR REFERENCE: how to load a marerial from code
      // Material wallMaterial = Resources.Load("Materials/BasicWalls", typeof(Material)) as Material;

      foreach (Wall wall in walls)
      {
         GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
         cube.transform.parent = wallsGroup.gameObject.transform;
         MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
         renderer.enabled = ShowWalls;
         if (ShowWalls)
         {
            renderer.material = WallMaterial;
         }
         cube.transform.position = new Vector3(wall.Location.x, 0.0f, wall.Location.y);
         cube.transform.localScale = new Vector3(wall.Width, 1f * VertScale, wall.Length);
      }
   }

   private void PlaceGrass()
   {
      Terrain terrainToPopulate = GetComponent<Terrain>();
      //terrainToPopulate.terrainData.SetDetailResolution(grassDensity, patchDetail);
      terrainToPopulate.terrainData.SetDetailResolution(1, 32);
   }
}