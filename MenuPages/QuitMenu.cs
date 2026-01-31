using Spelshoppen.Models;
using Spelshoppen.UX;

namespace Spelshoppen.MenuPages;


public class QuitMenu : IMenuPage
{
    public void DrawMenuPage(MyDbContext db, UserSession session)
    {
        UIRenderer.DrawBaseLayout(session);
        var textRows = new List<string>
        {
            "ÄR DU SÄKER PÅ ATT DU VILL AVSLUTA?",
            "[J] JA, TA MIG HÄRIFRÅN!!!",
            "[N] NEJ! JAG VILL FORTSÄTTA HANDLA!!!"   
        };
        new UX.Window("AVSLUTA", 40, 10,textRows).Draw();
    }

    public void PageInput(ConsoleKeyInfo key, char input, MyDbContext db, UserSession session)
    {
        switch (char.ToUpper(input))
        {
            case 'J':
                session.IsRunning = false;
                break;
            case 'N':
                session.State = MenuState.MainMenu;
                break;
            default:
                session.NotificationMessage = "TRYCK [J] ELLER [N]!";
                break;
        }
    }
}