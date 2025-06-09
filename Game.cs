using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Grand_Prix;

public class Game : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private GameState currentGameState = GameState.MainMenu;
    
    private Texture2D car1Texture;
    private Texture2D car2Texture;
    private Texture2D roadTexture;
    private Texture2D background;
    private Texture2D wall_r;
    private Texture2D wall_l;
    private Texture2D wall_u;
    private Texture2D wall_d;
    private Texture2D wall_du;
    private Texture2D wall_ur;
    private Texture2D wall_ul;
    private Texture2D wall_dr;
    private Texture2D wall_dl;
    private Texture2D wall_ula;
    private Texture2D wall_dra;
    private Texture2D wall_ura;
    private Texture2D wall_dla;
    private Texture2D wall_o;
    private Texture2D wall_o1;
    private Texture2D wall_o2;
    private Texture2D wall_o3;
    private Texture2D wall_o4;
    private Texture2D wall_o5;
    private Texture2D wall_o6;
    private Texture2D wall_o7;
    private Texture2D finishTexture;
    private SoundEffect clickSound;
    private SoundEffectInstance clickSoundInstance;
    private SoundEffect v10Sound;
    private SoundEffect f1Sound;
    private SoundEffect idlingSound;
    private SoundEffectInstance idlingSoundInstance;
    private SpriteFont font;
    private Button startButton;
    private Button level1Button;
    private Button level2Button;
    private Button level3Button;
    private Button car1Button;
    private Button car2Button;
    private Button exitButton;
    private Button educButton;
    private Button backButton;
    private MouseState mouseState;
    private Timer timer = new Timer();
    private string bestLap;
    private Vector2 cameraOffset;
    
    private PlayerCar playerCar;

    private int[,] trackMap;
    private double countdownTime = 3;
    private bool isCountdownActive = true;

    public List<GameState> history = new List<GameState>();
    
    /// <summary>
    /// Конструктор элементов класса Game1
    /// </summary>
    public Game()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        bestLap = "";
        graphics.PreferredBackBufferWidth = 800; // Начальная ширина
        graphics.PreferredBackBufferHeight = 600; // Начальная высота
        graphics.IsFullScreen = false; // Не полноэкранный режим
        graphics.ApplyChanges();
        Window.AllowUserResizing = true; // Разрешаем изменение размера окна
    }

    /// <summary>
    /// Метод, создающий окно
    /// </summary>
    protected override void Initialize()
    { 
        base.Initialize();
        Window.ClientSizeChanged += OnClientSizeChanged;
    }

    /// <summary>
    /// Метод, загружающий контент
    /// </summary>
    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        var startTexture = Content.Load<Texture2D>("image/start_button");
        var startHoverTexture = Content.Load<Texture2D>("image/start_hover_button");
        var exitTexture = Content.Load<Texture2D>("image/exit_button");
        var exitHoverTexture = Content.Load<Texture2D>("image/exit_hover_button");
        var educTexture = Content.Load<Texture2D>("image/educ_button");
        var educHoverTexture = Content.Load<Texture2D>("image/educ_hover_button");
        var backTexture = Content.Load<Texture2D>("image/back_button");
        var backHoverTexture = Content.Load<Texture2D>("image/back_hover_button");
        var level1Texture = Content.Load<Texture2D>("image/level1_button");
        var level1HoverTexture = Content.Load<Texture2D>("image/level1_hover_button");
        var level2Texture = Content.Load<Texture2D>("image/level2_button");
        var level2HoverTexture = Content.Load<Texture2D>("image/level2_hover_button");
        var level3Texture = Content.Load<Texture2D>("image/level3_button");
        var level3HoverTexture = Content.Load<Texture2D>("image/level3_hover_button");
        var car1ButtonTexture = Content.Load<Texture2D>("image/car1_button");
        var car1HoverTexture = Content.Load<Texture2D>("image/car1_hover_button");
        var car2ButtonTexture = Content.Load<Texture2D>("image/car2_button");
        var car2HoverTexture = Content.Load<Texture2D>("image/car2_hover_button");
        
        startButton = new Button(
            startTexture,
            startHoverTexture,
            new Rectangle(0, 0, 264, 72),
            StartGame
        );
        educButton = new Button(
            educTexture,
            educHoverTexture,
            new Rectangle(0, 0, 107, 20),
            Education
        );
        exitButton = new Button(
            exitTexture,
            exitHoverTexture,
            new Rectangle(0, 0, 98, 36 ),
            Exit
        );
        backButton = new Button(
            backTexture,
            backHoverTexture,
            new Rectangle(0, 0, 48, 18),
            Back
        );
        level1Button = new Button(
            level1Texture,
            level1HoverTexture,
            new Rectangle(0, 0, 100, 160),
            Level1
            );
        level2Button = new Button(
            level2Texture,
            level2HoverTexture,
            new Rectangle(0, 0, 100, 160),
            Level2
        );
        level3Button = new Button(
            level3Texture,
            level3HoverTexture,
            new Rectangle(0, 0, 100, 160),
            Level3
        );
        car1Button = new Button(
            car1ButtonTexture,
            car1HoverTexture,
            new Rectangle(0, 0, 80, 112),
            Car1
        );
        car2Button = new Button(
            car2ButtonTexture,
            car2HoverTexture,
            new Rectangle(0, 0, 80, 112),
            Car2
        );
        UpdateButtonPositions();
        car1Texture = Content.Load<Texture2D>("image/car1");
        car2Texture = Content.Load<Texture2D>("image/car2");
        roadTexture = Content.Load<Texture2D>("image/road");
        background = Content.Load<Texture2D>("image/background");
        wall_r = Content.Load<Texture2D>("image/wall_r");
        wall_l = Content.Load<Texture2D>("image/wall_l");
        wall_u = Content.Load<Texture2D>("image/wall_u");
        wall_d = Content.Load<Texture2D>("image/wall_d");
        wall_du = Content.Load<Texture2D>("image/wall_du");
        wall_ur = Content.Load<Texture2D>("image/wall_ur");
        wall_ul = Content.Load<Texture2D>("image/wall_ul");
        wall_dl = Content.Load<Texture2D>("image/wall_dl");
        wall_dr = Content.Load<Texture2D>("image/wall_dr");
        wall_dla = Content.Load<Texture2D>("image/wall_dla");
        wall_ura = Content.Load<Texture2D>("image/wall_ura");
        wall_dra = Content.Load<Texture2D>("image/wall_dra");
        wall_ula = Content.Load<Texture2D>("image/wall_ula");
        wall_o = Content.Load<Texture2D>("image/wall_o");
        wall_o1 = Content.Load<Texture2D>("image/wall_o1");
        wall_o2 = Content.Load<Texture2D>("image/wall_o2");
        wall_o3 = Content.Load<Texture2D>("image/wall_o3");
        wall_o4 = Content.Load<Texture2D>("image/wall_o4");
        wall_o5 = Content.Load<Texture2D>("image/wall_o5");
        wall_o6 = Content.Load<Texture2D>("image/wall_o6");
        wall_o7 = Content.Load<Texture2D>("image/wall_o7");
        finishTexture = Content.Load<Texture2D>("image/finish");
        font = Content.Load<SpriteFont>("Arial");
        
        clickSound = Content.Load<SoundEffect>("sound/click");
        clickSoundInstance = clickSound.CreateInstance();
        clickSoundInstance.Volume = 0.5f;
        v10Sound = Content.Load<SoundEffect>("sound/v10");
        f1Sound = Content.Load<SoundEffect>("sound/f1");
        idlingSound = Content.Load<SoundEffect>("sound/idling");
        idlingSoundInstance = idlingSound.CreateInstance();
        idlingSoundInstance.IsLooped = true;
        idlingSoundInstance.Volume = 0.2f;
    }

    /// <summary>
    /// Метод, обновляющий всю программу
    /// </summary>
    /// <param name="gameTime">Объект GameTime для получения прошедшего времени.</param>
    protected override void Update(GameTime gameTime)
    {
        mouseState = Mouse.GetState();
        if (currentGameState == GameState.MainMenu)
        {
            startButton.Update(mouseState);
            exitButton.Update(mouseState);
            educButton.Update(mouseState);
        }
        else if (currentGameState == GameState.Level)
        {
            level1Button.Update(mouseState);
            level2Button.Update(mouseState);
            level3Button.Update(mouseState);
            backButton.Update(mouseState);
        }
        else if (currentGameState == GameState.SelectCar)
        {
            car1Button.Update(mouseState);
            car2Button.Update(mouseState);
            backButton.Update(mouseState);
        }
        else if (currentGameState == GameState.Educ)
        {
            backButton.Update(mouseState);
        }
        else if (currentGameState == GameState.Playing)
        {
            backButton.Update(mouseState);
            timer.Update(gameTime);
            timer.Start();
            if (isCountdownActive)
            {
                countdownTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (countdownTime <= 0)
                {
                    isCountdownActive = false; // Завершение отсчёта
                }
            }

            // Обновление игрока только после завершения отсчёта
            if (!isCountdownActive)
            {
                playerCar.Update(gameTime, trackMap, 64);
            }

            Rectangle playerBounds = new Rectangle(
                (int)playerCar.Position.X,
                (int)playerCar.Position.Y,
                playerCar.Texture.Width,
                playerCar.Texture.Height
                );

            for (int y = 0; y < trackMap.GetLength(0); y++)
            {
                for (int x = 0; x < trackMap.GetLength(1); x++)
                {
                    if (trackMap[y, x] == 2)
                    {
                        Rectangle finishBounds = new Rectangle(
                            x * 64,
                            y * 64,
                            64,
                            64
                            );

                        if (playerBounds.Intersects(finishBounds))
                        {
                            FinishGame();
                        }
                    }
                }
            }

            cameraOffset = new Vector2(
                -playerCar.Position.X + GraphicsDevice.Viewport.Width / 2,
                -playerCar.Position.Y + GraphicsDevice.Viewport.Height / 2
                );
        }
        base.Update(gameTime);
    }

    /// <summary>
    /// Метод, отрисовывающий всю программу
    /// </summary>
    /// <param name="gameTime">Объект GameTime для получения прошедшего времени.</param>
    protected override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin();
            if (currentGameState == GameState.MainMenu)
            {
                startButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
                educButton.Draw(spriteBatch);
                GraphicsDevice.Clear(Color.White);
            }
            else if (currentGameState == GameState.Educ)
            {
                UpdateButtonPositions();
                string educName = "Управление:";
                Vector2 textSize = font.MeasureString(educName);
                var stringPosition = new Vector2(
                    Window.ClientBounds.Width / 2 - textSize.X / 2,
                    Window.ClientBounds.Height / 10
                );
                spriteBatch.DrawString(
                    font,
                    educName,
                    stringPosition,
                    Color.Black
                );
                string educ1 = "Газ - W";
                textSize = font.MeasureString(educ1);
                stringPosition = new Vector2(
                    Window.ClientBounds.Width / 2 - textSize.X / 2,
                    Window.ClientBounds.Height / 2 - textSize.Y - textSize.Y / 2
                );
                spriteBatch.DrawString(
                    font,
                    educ1,
                    stringPosition,
                    Color.Black
                );
                string educ2 = "Тормоз/задний ход - S";
                textSize = font.MeasureString(educ2);
                stringPosition = new Vector2(
                    Window.ClientBounds.Width / 2 - textSize.X / 2,
                    Window.ClientBounds.Height / 2 - textSize.Y / 2
                );
                spriteBatch.DrawString(
                    font,
                    educ2,
                    stringPosition,
                    Color.Black
                );
                string educ3 = "Поворот - A/D";
                textSize = font.MeasureString(educ3);
                stringPosition = new Vector2(
                    Window.ClientBounds.Width / 2 - textSize.X / 2,
                    Window.ClientBounds.Height / 2 + textSize.Y / 2
                );
                spriteBatch.DrawString(
                    font,
                    educ3,
                    stringPosition,
                    Color.Black
                );
                string educ4 = "Начать заново - R";
                textSize = font.MeasureString(educ4);
                stringPosition = new Vector2(
                    Window.ClientBounds.Width / 2 - textSize.X / 2,
                    Window.ClientBounds.Height / 2 + textSize.Y + textSize.Y / 2
                );
                spriteBatch.DrawString(
                    font,
                    educ4,
                    stringPosition,
                    Color.Black
                );
                backButton.Draw(spriteBatch);
                GraphicsDevice.Clear(Color.White);
            }
            else if (currentGameState == GameState.Level)
            {
                UpdateButtonPositions();
                backButton.Draw(spriteBatch);
                string selectLevel = "Выберите трассу:";
                Vector2 textSize = font.MeasureString(selectLevel);
                var stringPosition = new Vector2(
                    Window.ClientBounds.Width / 2 - textSize.X / 2,
                    Window.ClientBounds.Height / 10
                );
                spriteBatch.DrawString(
                    font,
                    selectLevel,
                    stringPosition,
                    Color.Black
                );
                level1Button.Draw(spriteBatch);
                level2Button.Draw(spriteBatch);
                level3Button.Draw(spriteBatch);
                GraphicsDevice.Clear(Color.White);
            }
            else if (currentGameState == GameState.SelectCar)
            {
                UpdateButtonPositions();
                backButton.Draw(spriteBatch);
                string selCar = "Выберите машину:";
                Vector2 textSize = font.MeasureString(selCar);
                var stringPosition = new Vector2(
                    Window.ClientBounds.Width / 2 - textSize.X / 2,
                    Window.ClientBounds.Height / 10
                );
                spriteBatch.DrawString(
                    font,
                    selCar,
                    stringPosition,
                    Color.Black
                );
                car1Button.Draw(spriteBatch);
                car2Button.Draw(spriteBatch);
                GraphicsDevice.Clear(Color.White);
            }
            else if (currentGameState == GameState.Playing)
            {
                UpdateButtonPositions();
                backButton.Draw(spriteBatch);
                GraphicsDevice.Clear(new Color(106322));
                for (int y = 0; y < trackMap.GetLength(0); y++)
                {
                    for (int x = 0; x < trackMap.GetLength(1); x++)
                    {
                        Vector2 position = new Vector2(x * 64, y * 64) + cameraOffset;

                        switch (trackMap[y, x])
                        {
                            case 0:
                                spriteBatch.Draw(roadTexture, position, Color.White);
                                break;
                            case 1:
                                break;
                            case 2:
                                spriteBatch.Draw(finishTexture, position, Color.White);
                                break;
                            case 3:
                                spriteBatch.Draw(wall_r, position, Color.White);
                                break;
                            case 4:
                                spriteBatch.Draw(wall_l, position, Color.White);
                                break;
                            case 5:
                                spriteBatch.Draw(wall_d, position, Color.White);
                                break;
                            case 6:
                                spriteBatch.Draw(wall_u, position, Color.White);
                                break;
                            case 7:
                                spriteBatch.Draw(wall_ur, position, Color.White);
                                break;
                            case 8:
                                spriteBatch.Draw(wall_dr, position, Color.White);
                                break;
                            case 9:
                                spriteBatch.Draw(wall_ul, position, Color.White);
                                break;
                            case 10:
                                spriteBatch.Draw(wall_dl, position, Color.White);
                                break;
                            case 11:
                                spriteBatch.Draw(wall_ura, position, Color.White);
                                break;
                            case 12:
                                spriteBatch.Draw(wall_ula, position, Color.White);
                                break;
                            case 13:
                                spriteBatch.Draw(wall_dra, position, Color.White);
                                break;
                            case 14:
                                spriteBatch.Draw(wall_dla, position, Color.White);
                                break;
                            case 15:
                                spriteBatch.Draw(wall_o, position, Color.White);
                                break;
                            case 16:
                                spriteBatch.Draw(wall_o1, position, Color.White);
                                break;
                            case 17:
                                spriteBatch.Draw(wall_o2, position, Color.White);
                                break;
                            case 18:
                                spriteBatch.Draw(wall_o3, position, Color.White);
                                break;
                            case 19:
                                spriteBatch.Draw(wall_o4, position, Color.White);
                                break;
                            case 20:
                                spriteBatch.Draw(wall_du, position, Color.White);
                                break;
                            case 21:
                                spriteBatch.Draw(wall_o5, position, Color.White);
                                break;
                            case 22:
                                spriteBatch.Draw(wall_o6, position, Color.White);
                                break;
                            case 23:
                                spriteBatch.Draw(wall_o7, position, Color.White);
                                break;
                        }
                    }
                    playerCar.Draw(spriteBatch, cameraOffset);
                    string formattedTime = timer.GetFormattedTime(timer.GetElapsedTime());
                    Vector2 textSize = font.MeasureString(formattedTime);
                    var timerPosition = new Vector2(
                        Window.ClientBounds.Width / 2 - textSize.X / 2,
                        50 // Фиксированное расстояние от верхнего края
                        );
                    spriteBatch.DrawString(
                        font,
                        formattedTime,
                        timerPosition,
                        Color.White
                        );
                    var bestPosition = new Vector2(
                        Window.ClientBounds.Width / 2 - textSize.X / 2,
                        50 - textSize.Y// Фиксированное расстояние от верхнего края
                        );
                    spriteBatch.DrawString(
                        font,
                        bestLap,
                        bestPosition,
                        Color.Yellow
                        );
                }
                if (isCountdownActive)
                {
                    string countdownText = Math.Ceiling(countdownTime).ToString();
                    Vector2 textSize = font.MeasureString(countdownText);
                    spriteBatch.DrawString(
                        font,
                        countdownText,
                        new Vector2(GraphicsDevice.Viewport.Width / 2 - textSize.X / 2, GraphicsDevice.Viewport.Height / 2 - textSize.Y / 2),
                        Color.White
                    );
                }
            }
            
            spriteBatch.End();
        
        base.Draw(gameTime);
    }

    /// <summary>
    /// Метод, ищущий стартовую позицию игрока
    /// </summary>
    /// <param name="trackMap">Карта уровня</param>
    /// <param name="cellSize">Размер клетки</param>
    /// <param name="carTexture">Текстура игрока</param>
    /// <returns>Элемент класса Vector2, описывающий стартовую позицию</returns>
    public static Vector2 FindSpawnPosition(int[,] trackMap, int cellSize, Texture2D carTexture)
        {
            for (int y = 0; y < trackMap.GetLength(0); y++)
            {
                for (int x = 0; x < trackMap.GetLength(1); x++)
                {
                    if (trackMap[y, x] == 2)
                    {
                        float baseX = x * cellSize + cellSize / 2;
                        float baseY = y * cellSize + cellSize / 2;

                        float offsetX = cellSize * 1.5f;
                        float offsetY = cellSize * 0.5f;

                        float spawnX = baseX + offsetX - carTexture.Width / 2;
                        float spawnY = baseY + offsetY - carTexture.Height / 2;

                        Position testPosition = new Position(spawnX, spawnY);
                        return new Vector2(spawnX, spawnY);
                    }
                }
            }

            return new Vector2(400, 300); 
        }
   
    /// <summary>
    /// Метод, описывающий логику кнопки старт
    /// </summary>
    private void StartGame()
    {
        clickSoundInstance.Play();
        history.Add(currentGameState);
        currentGameState = GameState.Level;
    }

    /// <summary>
    /// Метод, описывающий логику пересечения финишной черты
    /// </summary>
    private void FinishGame()
    {
        bestLap = timer.GetFormattedTime(timer.UpdateBestTime(timer.GetElapsedTime()));
        timer.Reset();
    }
    
    /// <summary>
    /// Метод, описывающий логику кнопки educ
    /// </summary>
    private void Education()
    {
        clickSoundInstance.Play();
        history.Add(currentGameState);
        currentGameState = GameState.Educ;
    }

    /// <summary>
    /// Метод, описывающий логику кнопки назад
    /// </summary>
    private void Back()
    {
        clickSoundInstance.Play();
        currentGameState = history[history.Count - 1];
        history.RemoveAt(history.Count - 1);
    }
    
    /// <summary>
    /// Метод, описывающий логику кнопки выбора трассы SSW
    /// </summary>
    private void Level1()
    {
        clickSoundInstance.Play();
        history.Add(currentGameState);
        trackMap = Levels.LevelsSelect(Level.SSW);
        currentGameState = GameState.SelectCar;
    }
    
    /// <summary>
    /// Метод, описывающий логику кнопки выбора трассы Monza
    /// </summary>
    private void Level2()
    {
        clickSoundInstance.Play();
        history.Add(currentGameState);
        trackMap = Levels.LevelsSelect(Level.Monza);
        currentGameState = GameState.SelectCar;
    }
    
    /// <summary>
    /// Метод, описывающий логику кнопки выбора трассы Monaco
    /// </summary>
    private void Level3()
    {
        clickSoundInstance.Play();
        history.Add(currentGameState);
        trackMap = Levels.LevelsSelect(Level.Monaco);
        currentGameState = GameState.SelectCar;
    }

    /// <summary>
    /// Метод, описывающий логику кнопки выбора машины 1
    /// </summary>
    private void Car1()
    {
        clickSoundInstance.Play();
        history.Add(currentGameState);
        Vector2 spawnPosition = FindSpawnPosition(trackMap, 64, car1Texture);
        playerCar = new PlayerCar(car1Texture, v10Sound, idlingSoundInstance, new Position(spawnPosition.X, spawnPosition.Y), 300f, 1.5f, 200f);
        currentGameState = GameState.Playing;
        countdownTime = 3f;
        isCountdownActive = true;
    }
    
    /// <summary>
    /// Метод, описывающий логику кнопки выбора машины 2
    /// </summary>
    private void Car2()
    {
        clickSoundInstance.Play();
        history.Add(currentGameState);
        Vector2 spawnPosition = FindSpawnPosition(trackMap, 64, car2Texture);
        playerCar = new PlayerCar(car2Texture, f1Sound, idlingSoundInstance, new Position(spawnPosition.X, spawnPosition.Y), 275f, 1.7f, 175f);
        currentGameState = GameState.Playing;
        countdownTime = 3f;
        isCountdownActive = true;
    }

    /// <summary>
    /// Метод, реализующий возможность изменения размеров окна
    /// </summary>
    /// <param name="sender">Отправитель</param>
    /// <param name="e">Событие</param>
    private void OnClientSizeChanged(object sender, EventArgs e)
    {
        graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
        graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
        graphics.ApplyChanges();
        
        UpdateButtonPositions();
    }

    /// <summary>
    /// Метод, обновляющий позицию кнопок
    /// </summary>
    private void UpdateButtonPositions()
    {
        int screenWidth = Window.ClientBounds.Width;
        int screenHeight = Window.ClientBounds.Height;
        
        startButton.Rectangle = new Rectangle(
            screenWidth / 2 - startButton.Rectangle.Width / 2,
            screenHeight / 2 - 100, 
            startButton.Rectangle.Width,
            startButton.Rectangle.Height
        );

        exitButton.Rectangle = new Rectangle(
            screenWidth / 2 - exitButton.Rectangle.Width / 2,
            screenHeight / 2 + 50, 
            exitButton.Rectangle.Width,
            exitButton.Rectangle.Height
        );
        
        educButton.Rectangle = new Rectangle(
            screenWidth / 2 - educButton.Rectangle.Width / 2,
            screenHeight / 2, 
            exitButton.Rectangle.Width,
            exitButton.Rectangle.Height
        );
        if (currentGameState == GameState.SelectCar)
        {
            backButton.Rectangle = new Rectangle(
                screenWidth / 20 - backButton.Rectangle.Width / 2,
                screenHeight / 20 + backButton.Rectangle.Height, 
                backButton.Rectangle.Width,
                backButton.Rectangle.Height
            );
        }
        else
            backButton.Rectangle = new Rectangle(
                screenWidth / 20 - backButton.Rectangle.Width / 2,
                screenHeight / 20, 
                backButton.Rectangle.Width,
                backButton.Rectangle.Height
            );

        level1Button.Rectangle = new Rectangle(
            screenWidth / 2 - level1Button.Rectangle.Width * 2 ,
            screenHeight / 2 - level1Button.Rectangle.Height / 2,
            level1Button.Rectangle.Width,
            level1Button.Rectangle.Height
        );
        level2Button.Rectangle = new Rectangle(
            screenWidth / 2 - level2Button.Rectangle.Width / 2,
            screenHeight / 2 - level2Button.Rectangle.Height / 2,
            level2Button.Rectangle.Width,
            level2Button.Rectangle.Height
        );
        level3Button.Rectangle = new Rectangle(
            screenWidth / 2 + level3Button.Rectangle.Width,
            screenHeight / 2 - level3Button.Rectangle.Height / 2,
            level3Button.Rectangle.Width,
            level3Button.Rectangle.Height
        );
        car1Button.Rectangle = new Rectangle(
            screenWidth / 2 - car1Button.Rectangle.Width / 2 - car1Button.Rectangle.Width,
            screenHeight / 2 - car1Button.Rectangle.Height / 2 + 100,
            car1Button.Rectangle.Width,
            car1Button.Rectangle.Height
        );
        car2Button.Rectangle = new Rectangle(
            screenWidth / 2 + car2Button.Rectangle.Width / 2,
            screenHeight / 2 - car2Button.Rectangle.Height / 2 + 100,
            car2Button.Rectangle.Width,
            car2Button.Rectangle.Height
        );
    }
}