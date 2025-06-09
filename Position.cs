using Microsoft.Xna.Framework;

namespace Grand_Prix;

/// <summary>
/// Класс, описывающий позицию объекта
/// </summary>
public class Position
{
    
    public float X { get; set; }
    public float Y { get; set; }


    /// <summary>
    /// Конструктор элементов класса Position
    /// </summary>
    /// <param name="x">Координата X</param>
    /// <param name="y">Координата Y</param>
    public Position(float x, float y)
    {
        X = x;
        Y = y;
    }
    
    /// <summary>
    /// Метод, описывающий перемещение
    /// </summary>
    /// <param name="direction">Направление</param>
    /// <param name="speed">Скорость</param>
    public void Move(Vector2 direction, float speed)
    {
        X += direction.X * speed;
        Y += direction.Y * speed;
    }

    /// <summary>
    /// Метод, реализующий колизию объектов
    /// </summary>
    /// <param name="trackMap">Карта</param>
    /// <param name="cellSize">Размер клеток</param>
    /// <param name="objectWidth">Ширина объекта</param>
    /// <param name="objectHeight">Высота объекта</param>
    /// <returns>True - если объекты сталкиваются, false - нет</returns>
    public bool CheckTrackCollision(int[,] trackMap, int cellSize, int objectWidth, int objectHeight)
    {
        var leftCell = (X) / cellSize;
        var rightCell = (X + objectWidth - 1) / cellSize; 
        var topCell = (Y) / cellSize;
        var bottomCell = (Y + objectHeight - 1) / cellSize;
        
        for (var x = leftCell; x <= rightCell; x++)
        {
            for (var y = topCell; y <= bottomCell; y++)
            {
                if (x < 0 || y < 0 || y >= trackMap.GetLength(0) || x >= trackMap.GetLength(1))
                    return true; 
                
                if (!(trackMap[(int)(y), (int)(x)] == 0) && !(trackMap[(int)(y), (int)(x)] == 2))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Метод, описывающий сложение элементов типа Position и типа Vector2
    /// </summary>
    /// <param name="position">Элемент типа Position</param>
    /// <param name="offset">Элемент типа Vector2</param>
    /// <returns>Элемент типа Position</returns>
    public static Position operator +(Position position, Vector2 offset)
    {
        return new Position(position.X + offset.X, position.Y + offset.Y);
    }
}