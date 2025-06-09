using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Grand_Prix;


/// <summary>
/// Класс, описывающий игрока
/// </summary>
public class PlayerCar
{
    public Texture2D Texture { get; set; }
    public SoundEffect CarSound { get; set; }
    public SoundEffectInstance CarSoundInstance { get; set; }
    public SoundEffectInstance IdlingSoundInstance { get; set; }
    public Position Position { get; set; } 
    private float Acceleration;// Ускорение
    private float MaxSpeed;// Максимальная скорость
    private float RotationSpeed;// Скорость поворота
    private float currentSpeed;// Текущая скорость
    private float accumulatedRotation;// Полный угол поворота
    private float targetRotation;// Целевой угол
    private Vector2 origin;

    /// <summary>
    /// Конструктор элементов класса PlayerCar
    /// </summary>
    /// <param name="texture">Текстура игрока</param>
    /// <param name="carSound">Звук машины</param>
    /// <param name="idlingSound">Звук машины на холостом ходу</param>
    /// <param name="startPosition">Стартовая позиция</param>
    /// <param name="speed">Максимальная скорость</param>
    /// <param name="rotationSpeed">Скорость поворота</param>
    /// <param name="acceleration">Ускорение</param>
    public PlayerCar(Texture2D texture, SoundEffect carSound, SoundEffectInstance idlingSound, Position startPosition, float speed, float rotationSpeed, float acceleration)
    {
        Texture = texture;
        CarSound = carSound;
        CarSoundInstance = carSound.CreateInstance();
        CarSoundInstance.IsLooped = true;
        CarSoundInstance.Volume = 0.1f;
        IdlingSoundInstance = idlingSound;
        Position = startPosition;
        RotationSpeed = rotationSpeed;
        Acceleration = acceleration;
        origin = new Vector2(texture.Width / 2, texture.Height / 2);
        currentSpeed = 0f;
        MaxSpeed = speed;
        accumulatedRotation = -MathHelper.PiOver2; // -90°
        targetRotation = -MathHelper.PiOver2;      // -90°
    }

    /// <summary>
    /// Метод, обновляющий игрока
    /// </summary>
    /// <param name="gameTime">Объект GameTime для получения прошедшего времени</param>
    /// <param name="trackMap">Карта</param>
    /// <param name="cellSize">Размер клетки</param>
    public void Update(GameTime gameTime, int[,] trackMap, int cellSize)
    {
        var keyboardState = Keyboard.GetState();
        Vector2 movementDirection = Vector2.Zero;
        bool soundOn = false;
        bool soundOn1 = false;

        //  Управление газом/тормозом
        if (keyboardState.IsKeyDown(Keys.W))
        {
            if (!soundOn)
            {
                CarSoundInstance.Play();
                soundOn = true;
            }
            else CarSoundInstance.Resume();
            IdlingSoundInstance.Pause();
            currentSpeed += Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (keyboardState.IsKeyDown(Keys.S))
            currentSpeed -= Acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Ограничение скорости 
        currentSpeed = MathHelper.Clamp(currentSpeed, -MaxSpeed, MaxSpeed);

        // Поворот в любую сторону 
        float turnInput = 0f;
        if (keyboardState.IsKeyDown(Keys.A)) turnInput = -1;
        if (keyboardState.IsKeyDown(Keys.D)) turnInput = 1;
        
        if (keyboardState.IsKeyDown(Keys.R))
            Position = new Position(Game.FindSpawnPosition(trackMap, cellSize, Texture).X, Game.FindSpawnPosition(trackMap, cellSize, Texture).Y);
        if (turnInput != 0)
        {
            // Расчет изменения угла на основе ввода 
            float deltaRotation = turnInput * RotationSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            targetRotation += deltaRotation;
        }

        // Интерполяция угла для плавного поворота 
        accumulatedRotation = MathHelper.Lerp(
            accumulatedRotation,
            targetRotation,
            (float)gameTime.ElapsedGameTime.TotalSeconds * RotationSpeed
        );

        //  Расчет направления движения
        var direction = new Vector2(
            (float)Math.Cos(accumulatedRotation),
            (float)Math.Sin(accumulatedRotation)
        );

        // Применение скорости и проверка коллизий 
        Position tempPosition = new Position(Position.X, Position.Y);
        tempPosition.Move(direction, currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

        if (!tempPosition.CheckTrackCollision(trackMap, cellSize, Texture.Width, Texture.Height))
        {
            Position = tempPosition;
        }

        //  Постепенное замедление при отпускании клавиш 
        if (!keyboardState.IsKeyDown(Keys.W) && !keyboardState.IsKeyDown(Keys.S))
        {
            currentSpeed *= 0.99f; // Трение
            if (Math.Abs(currentSpeed) < 1f) currentSpeed = 0f;
        }

        if (!keyboardState.IsKeyDown(Keys.W))
        {
            if (!soundOn1)
            {
                IdlingSoundInstance.Play();
                soundOn1 = true;
            }
            else
            {
                IdlingSoundInstance.Resume();
            }
            CarSoundInstance.Pause();
        }
    }

    /// <summary>
    /// Метод, отрисовывающий игрока
    /// </summary>
    /// <param name="spriteBatch">Спрайт игрока</param>
    /// <param name="cameraOffset">Смещение камеры</param>
    public void Draw(SpriteBatch spriteBatch, Vector2 cameraOffset)
    {
        Position finalPosition = Position + cameraOffset; 
        spriteBatch.Draw(
            Texture,
            new Vector2(finalPosition.X, finalPosition.Y),
            null,
            Color.White,
            accumulatedRotation,
            origin,
            1f,
            SpriteEffects.None,
            0f
        );
    }
}
