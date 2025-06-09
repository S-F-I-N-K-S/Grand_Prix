using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Grand_Prix;

/// <summary>
/// Класс, описывающий функциональные кнопки
/// </summary>
public class Button
{
    private Texture2D Texture;
    private Texture2D HoverTexture;
    public Rectangle Rectangle { get; set; }
    private bool isHovered;
    private Action OnClick;

    /// <summary>
    /// Конструктор элементов класса Button
    /// </summary>
    /// <param name="texture">Текстура кнопки</param>
    /// <param name="hoverTexture">Текстура кнопки при наведении</param>
    /// <param name="rectangle">Расположение кнопки</param>
    /// <param name="onClick">Логика кнопки</param>
    public Button(Texture2D texture, Texture2D hoverTexture, Rectangle rectangle, Action onClick)
    {
        Texture = texture;
        HoverTexture = hoverTexture;
        Rectangle = rectangle;
        OnClick = onClick;
    }

    /// <summary>
    /// Метод, обновляющий кнопку
    /// </summary>
    /// <param name="mouseState">Положение курсора</param>
    public void Update(MouseState mouseState)
    {
        isHovered = Rectangle.Contains(mouseState.Position);
        if (isHovered && mouseState.LeftButton == ButtonState.Pressed)
            OnClick?.Invoke();
    }

    /// <summary>
    /// Метод, отрисовывающий кнопку
    /// </summary>
    /// <param name="spriteBatch">Спрайт кнопки</param>
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(isHovered ? HoverTexture : Texture, Rectangle, Color.White);
    }
}