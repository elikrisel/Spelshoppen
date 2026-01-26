using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

public class MainMenu : IMenuPage
{
    public void Draw(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
        
        new UX.Window("Välkommen!", 25, 6, new List<string> { 
            "Välkommen till Spelshoppen!",
            "Använd menyknapparna till vänster för att navigera."
        }).Draw();
        
    }
    
    //TODO: Fylla i sen
    public void HandleInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        
    }
}