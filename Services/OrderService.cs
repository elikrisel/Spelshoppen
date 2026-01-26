using Spelshoppen.MenuPages;
using Spelshoppen.Models;
using Spelshoppen.Transactions;

namespace Spelshoppen;

public class OrderService
{
    public static void CreateOrder(MyDbContext db, UserSession session)
    {
        try
        {
            // Hämtning av Country och Payment
            var country = db.Countries.FirstOrDefault() ?? new Country { Name = "Sverige" };
            var payment = db.PaymentMethods.FirstOrDefault() ?? new PaymentMethod { Name = "Faktura" };

            var customer = db.Customers.FirstOrDefault(c => 
                c.FirstName == session.FirstName && c.LastName == session.LastName)
                ?? new Customer { FirstName = session.FirstName, LastName = session.LastName, Street = session.StreetName };

            var city = db.Cities.FirstOrDefault(c => c.Name == session.CityName)
                       ?? new City { Name = session.CityName, Country = country };

            var order = new Order {
                Customers = customer,
                Cities = city,
                PaymentMethods = payment,
                OrderDate = DateTime.Now,
                TotalAmount = session.CartItem.Sum(kvp => kvp.Key.Price * kvp.Value),
                Street = session.StreetName
            };
            
            foreach (var entry in session.CartItem)
            {
                order.OrderLines.Add(new OrderLine {
                    ProductItemId = entry.Key.Id,
                    Quantity = entry.Value,
                    TotalPrice = entry.Key.Price,
                    VatRate = 0.25m
                });
            }

            db.Orders.Add(order);
            db.SaveChanges();
            
            session.CartItem.Clear();
            session.NotificationMessage = "Order sparad! Tack för ditt köp.";
        }
        catch (Exception ex)
        {
            session.NotificationMessage = "Kunde inte spara order: " + ex.Message;
        }
        finally
        {
            
            session.Status = CheckoutState.ReviewingCart;
            session.State = MenuState.MainMenu;
        }
    }
}