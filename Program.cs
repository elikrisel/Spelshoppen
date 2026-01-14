namespace Spelshoppen;

using UX;
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            // Hämtar från databasen
            List<string> categoriesText = new List<string> { "1. Spel", "2. Konsoler", "3. Tillbehör" };
        
            // Detta hämtas från databas
            List<string> cartText = new List<string> { "1 st PS4, Metro Exodus", "1 st NSW Pro Controller", "Tryck X för att checka ut" };
            var windowCart = new Window("Din varukorg", 30, 6, cartText);
            windowCart.Draw();

            var windowCategories = new Window("Kategorier", 2, 6, categoriesText);
            windowCategories.Draw();

            List<string> topText = new List<string> { "# Spelshoppen #", "Finns nu i Konsol app!" };
            var windowTop = new Window("", 2, 1, topText);
            windowTop.Draw();


            List<string> topText2 = new List<string> { "NSW2, The Legend of Zelda: Tears of the Kingdom",
                "PS5, The Last Of Us Part 2: Remastered", "XBONE, Starfield" };
            var windowTop2 = new Window("Bäst säljande produkter", 2, 15, topText2);
            windowTop2.Draw();

            List<string> topText3 = new List<string> { "1. Startsida", "2. Shoppen", "3. Varukorgen" };
            var windowTop3 = new Window("Kundmeny", 55, 15, topText3);
            windowTop3.Draw();

            List<string> topText4 = new List<string> { "1. Administrera produkter", "2. Administrera kategorier", "3. Administrera kunder", "4. Se statistik(Queries)" };
            var windowTop4 = new Window("Admin", 75, 15, topText4);
            windowTop4.Draw();

            windowTop.Left = 45;
            windowTop.Draw();
            
            Console.WriteLine();
            Console.WriteLine("Tryck för att navigera i menyn");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            
            
            switch (keyInfo.KeyChar)
            {
                
                case '1':
                    Console.WriteLine(" Du har tryckt 1");
                    break;
                
            }    
            
        }
        


    }
}