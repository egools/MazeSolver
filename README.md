# MazeSolver

Maze Constraints:
1. Paths are 1px wide white argb(255, 255, 255, 255) pixels
2. Start opening must be on the top row of pixels
3. End opening must be on the bottom row of pixels
4. Looped paths are allowed

Command line arguments takes path to maze image to solve, otherwise uses a default image included in the project files.
Creates a directory "CompletedMazes" (if it doesn't exist) in the directory of original image.
