using Spelshoppen.UX;

namespace Spelshoppen;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            //Titel
            List<string> topText = new List<string> { "# Spelshoppen #", "Finns nu i Konsol app!" };
            var windowTop = new UX.Window("", 45, 1, topText);
            windowTop.Draw();
            
            // Hämtar från databasen
            List<string> categoriesText = new List<string> { "1. Spel", "2. Konsoler", "3. Tillbehör" };
            
            // Detta hämtas från databas
            List<string> cartText = new List<string> { "1 st PS4, Metro Exodus", "1 st NSW Pro Controller", "Tryck X för att checka ut" };
            var windowCart = new UX.Window("Din varukorg", 75, 1, cartText);
            windowCart.Draw();
            
            List<string> topText3 = new List<string> { "1. Startsida", "2. Shoppen", "3. Varukorgen" };
            var windowTop3 = new UX.Window("Kundmeny", 2, 1, topText3);
            windowTop3.Draw();
            
            
            var windowCategories = new UX.Window("Kategorier", 2, 20, categoriesText);
            windowCategories.Draw();
            WindowExample.DrawShop();
            
            
            
            List<string> topText2 = new List<string> { "NSW2, The Legend of Zelda: Tears of the Kingdom",
                "PS5, The Last Of Us Part 2: Remastered", "XBONE, Starfield" };
            var windowTop2 = new UX.Window("Bäst säljande produkter", 25, 6, topText2);
            windowTop2.Draw();
            
            
            List<string> topText4 = new List<string> { "1. Administrera produkter", "2. Administrera kategorier", "3. Administrera kunder", "4. Se statistik(Queries)" };
            var windowTop4 = new UX.Window("Admin", 75, 20, topText4);
            windowTop4.Draw();
            
            
            
            Console.WriteLine();
            Console.WriteLine("Tryck för att navigera i menyn");
            
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.Clear();
            
            
            switch (keyInfo.KeyChar)
            {
                
                case '1':
                    Console.WriteLine(" Du har tryckt 1");
                    break;
                
            }
        }
        
    }
}
