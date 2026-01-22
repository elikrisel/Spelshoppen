using Spelshoppen.Models;

namespace Spelshoppen.MenuPages;

public interface IMenuPage
{
    void Draw(MyDbContext db, UserSession session);
    
    void HandleInput(ConsoleKeyInfo key,char input,MyDbContext db, UserSession session);
    
}