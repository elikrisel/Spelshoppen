using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen;

//TODO: Flytta mer saker till Helpers
public class Helpers
{
    //Kollar vilket state jag är i under programmets gång
    public static void ShowDebugInfo(MenuState state)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"Nuvarande State: {state}");
        Console.ResetColor();
    }

    public static void ShowDebugInCategorySelection(UserSession session)
    {
        Console.SetCursorPosition(0, 28);
        Console.Write($"Nuvarande selection. CategoryId: {session.SelectedCategoryId} | ProductId: {session.SelectedProductId}");
    }

    public static void ShowCart(UserSession session)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.SetCursorPosition(80, 2);
        Console.Write($"Varukorg: {session.CartItem.Count} stycken");
        Console.ResetColor();
    }
    //Printar x linjer enligt användaren
    public static string PrintXNumberOfLines(int number) => new('-', number);
    #region Properties for Featured Menu
    public static int[] SetXPositionOnFeatured => [10, 40, 80];
    public static int SetYPositionOnFeatured => 15;
    public static int MaxTitleLengthOnFeatured => 30;
    #endregion
    public static string Prompt(string message)
    {
        Console.CursorVisible = true;
        Console.Write($" >> {message}: ");
        string input = Console.ReadLine() ?? string.Empty;
        Console.CursorVisible = false;
        return input;
    }
}