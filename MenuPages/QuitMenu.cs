using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;

//TODO: Fix Quit State
public class QuitMenu : IMenuPage
{
    private List<ProductSearchResult> searchResults = new List<ProductSearchResult>();
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
        
    }

    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        throw new NotImplementedException();
    }
}