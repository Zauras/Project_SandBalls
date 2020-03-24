using UnityEngine;

namespace MeshGeneration
{
    public class SquareGrid
    {
        public SquareGrid(float[,] rasterMap, float squareSize)
        {
            int rowLength = rasterMap.GetLength(0);
            int columnLenght = rasterMap.GetLength(1);
            float squaresCountInRow = rowLength * squareSize;
            float squaresCountInColumn = columnLenght * squareSize;
            var array = new ControlPoint[rowLength, columnLenght];

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < columnLenght; j++)
                {
                    Vector3 position = new Vector3(
                        -squaresCountInRow / 2f + i * squareSize + squareSize / 2f,
                        -squaresCountInColumn / 2f + j * squareSize + squareSize / 2f,
                        0f);

                    float currentRow = 0f;
                    if (i < rowLength - 1)
                    {
                        currentRow = rasterMap[i + 1, j];
                    }

                    float currentColumn = 0f;
                    if (j < columnLenght - 1)
                    {
                        currentColumn = rasterMap[i, j + 1];
                    }

                    array[i, j] = new ControlPoint(position,
                                                rasterMap[i, j], currentRow, currentColumn,
                                                 squareSize);
                }
            }

            squares = new Square[rowLength - 1, columnLenght - 1];
            for (int row = 0; row < rowLength - 1; row++)
            {
                for (int column = 0; column < columnLenght - 1; column++)
                {
                    squares[row, column] = new Square(array[row, column + 1],
                                                    array[row + 1, column + 1],
                                                    array[row + 1, column],
                                                    array[row, column]);
                }
            }
        }

        public Square[,] squares;
    }
}