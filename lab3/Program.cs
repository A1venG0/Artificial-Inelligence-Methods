using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public class HopfieldNetwork
{
    private readonly string[] _filePaths;
    private readonly List<int[]> _learningShapes;

    private readonly double[,] _weights;

    private readonly int _numberOfRows;
    private readonly int _numberOfColumns;

    private readonly int _iterationCount;
    private readonly int _shapeSize;

    private readonly char _fillChar;

    public HopfieldNetwork(string[] filePaths, int rowCount, int columnCount, char fillChar, int iterationCount)
    {
        _shapeSize = rowCount * columnCount;
        _weights = new double[_shapeSize, _shapeSize];

        _filePaths = filePaths;
        _learningShapes = new List<int[]>();

        _numberOfRows = rowCount;
        _numberOfColumns = columnCount;

        _fillChar = fillChar;
        _iterationCount = iterationCount;

        LoadAllLearningShapes(filePaths);
    }

    private void LoadAllLearningShapes(IEnumerable<string> learningShapesFilePaths)
    {
        foreach (var filePath in learningShapesFilePaths)
        {
            var shape = ReadShapeFromFile(filePath);
            shape = shape.Replace("\n", "").Replace("\r", "");

            var parsedShape = ParseShapeToList(shape);

            var rowsCount = CountShapeRows(parsedShape);
            var columnsCount = shape.Length / rowsCount;

            if (ValidateShapeSizes(rowsCount, columnsCount) == false)
                continue;

            _learningShapes.Add(parsedShape);
        }
    }

    private static string ReadShapeFromFile(string filePath)
    {
        Console.WriteLine($"Reading shape from file {filePath}.");
        var shape = File.ReadAllText(filePath);

        Console.WriteLine($"The read shape is:\n{shape}");
        return shape;
    }

    private int CountShapeRows(IReadOnlyCollection<int> shape)
    {
        return shape.Count / _numberOfColumns;
    }

    private int[] ParseShapeToList(string shape)
    {
        return shape.Select(ch => ch == _fillChar ? 1 : -1).ToArray();
    }

    private bool ValidateShapeSizes(int shapeRowsNum, int shapeColsNum)
    {
        if (shapeRowsNum != _numberOfRows || shapeColsNum != _numberOfColumns)
        {
            Console.WriteLine("Incorrect number of rows or columns");
            return false;
        }
        return true;
    }

    public void StartLearning()
    {
        foreach (var learningShape in _learningShapes)
        {
            for (var i = 0; i < learningShape.Length; i++)
            {
                for (var j = 0; j < learningShape.Length; j++)
                {
                    _weights[i, j] += learningShape[i] * learningShape[j];
                }
            }
        }

        for (int i = 0; i < _weights.GetLength(0); i++)
        {
            _weights[i, _weights.GetLength(1) - 1 - i] = 0;
        }

        var coefficient = Math.Round(1.0 / _shapeSize, 3);
        for (var i = 0; i < _shapeSize; i++)
        {
            for (var j = 0; j < _shapeSize; j++)
            {
                _weights[i, j] *= coefficient;
            }
        }
    }

    public void Recognize(string shapeToRecognizeFilePath)
    {
        var shape = ReadShapeFromFile(shapeToRecognizeFilePath);
        shape = shape.Replace("\n", "").Replace("\r", "");
        var shapeToRecognize = ParseShapeToList(shape);

        var rowsCount = CountShapeRows(shapeToRecognize);
        var columnsCount = shapeToRecognize.Length / rowsCount;

        if (ValidateShapeSizes(rowsCount, columnsCount) == false)
            return;

        var initialShapeToRecognize = shapeToRecognize;

        var iteration = 0;
        var isRecognized = false;
        var matchedShapeIndex = 0;

        while (iteration < _iterationCount)
        {
            shapeToRecognize = ExecuteStep(shapeToRecognize);
            matchedShapeIndex = FindMatch(shapeToRecognize);

            if (matchedShapeIndex != -1)
            {
                isRecognized = true;
                break;
            }

            iteration++;
        }

        Console.WriteLine(isRecognized ?
            $"The shape has been successfully recognized for {iteration + 1} cycles. {_filePaths[matchedShapeIndex]}" :
            $"Can't recognize the shape from file");
    }

    private int[] ExecuteStep(IReadOnlyList<int> shape)
    {
        var newShape = SumFunction(shape);
        return TransferFunction(newShape);
    }

    private IEnumerable<double> SumFunction(IReadOnlyList<int> shape)
    {
        var result = new double[_shapeSize];
        for (var i = 0; i < _shapeSize; i++)
        {
            for (var j = 0; j < _shapeSize; j++)
            {
                result[i] += _weights[i, j] * shape[j];
            }
        }
        return result;
    }

    private static int[] TransferFunction(IEnumerable<double> shape)
    {
        return shape.Select(value => value < 0 ? -1 : 1).ToArray();
    }

    private int FindMatch(int[] shape)
    {
        for (var i = 0; i < _learningShapes.Count; i++)
        {
            if (CompareShapes(_learningShapes[i], shape))
            {
                return i;
            }
        }

        return -1;
    }

    private static bool CompareShapes(IEnumerable<int> shape1, IEnumerable<int> shape2)
    {
        return shape1.SequenceEqual(shape2);
    }
}

internal abstract class Program
{
    private static void Main()
    {
        var learningShapesFilePaths = new[] { "fileOne.txt", "fileTwo.txt", "fileThree.txt" };
        var shapesToRecognizeFilePaths = new[] { "ModifiedOne.txt", "ModifiedTwo.txt", "ModifiedThree.txt", "Undefined.txt" };

        var hopfieldNetwork = new HopfieldNetwork(learningShapesFilePaths, 5, 5, '#', 200);
        hopfieldNetwork.StartLearning();
        hopfieldNetwork.Recognize(shapesToRecognizeFilePaths[0]);
        Console.ReadKey();
    }
}
