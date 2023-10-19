using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
   public string MapFile;
   public float HorizScale = 1f;
   public float VertScale = 1f;
   public bool ShowWalls = true;

   private char[,] grid = null;
   private List<Wall> walls;

   private static char WALL_CHAR = '#';

   // Start is called before the first frame update
   void Start()
   {
      LoadGrid();
      IdentifyWalls();
      GenerateWalls();
   }

   private void LoadGrid()
   {
      DirectoryInfo mapsDir;
      char vSep = Path.VolumeSeparatorChar;
      char dSep = Path.DirectorySeparatorChar;

      //string fileName = "Maze2_13x17_Braid_Rooms.txt";
      string filePath = string.Empty;

      // bind to an existing directory 
      mapsDir = new DirectoryInfo($"Assets{dSep}Mazes");
      Debug.Log(mapsDir);

      // specify a file in that path
      filePath = $"{mapsDir.FullName}{dSep}{MapFile}.txt";

      // read a text file using StreamReader
      Console.WriteLine($"Opening and reading {filePath}...");
      Console.WriteLine();

      using (StreamReader reader = new StreamReader(filePath))
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
               walls.Add(new Wall(loc, 1 * HorizScale, 1 * HorizScale));
            }
         }
      }
   }

   private void GenerateWalls()
   {
      Debug.Log($"Wall count: {walls.Count}");
      foreach(Wall wall in walls)
      {
         GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
         cube.GetComponent<MeshRenderer>().enabled = ShowWalls;
         cube.transform.position = new Vector3(wall.Location.x, 0.5f, wall.Location.y);
         cube.transform.localScale = new Vector3(wall.Width, 1f * VertScale, wall.Length);
      }
   }
}