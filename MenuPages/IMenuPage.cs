using Spelshoppen.Models;

namespace Spelshoppen.MenuPages;

public interface IMenuPage
{
    void DrawMenuPage(MyDbContext db, UserSession session);
    
    void PageInput(ConsoleKeyInfo key,char input,MyDbContext db, UserSession session);
    
}