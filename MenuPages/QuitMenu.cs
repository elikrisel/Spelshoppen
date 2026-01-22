using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

public class QuitMenu : IMenuPage
{
    public void Draw(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
    }

    public void HandleInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        throw new NotImplementedException();
    }
}