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
            // // Hämtning av Country och Payment, om det inte finns i databasen så skapas nytt land och payment
            var country = session.SelectedCountryId > 0 
                ? db.Countries.Find(session.SelectedCountryId) 
                : new Country { Name = session.CountryName };

            var payment = session.SelectedPaymentMethodId > 0 
                ? db.PaymentMethods.Find(session.SelectedPaymentMethodId) 
                : new PaymentMethod { Name = session.PaymentMethodName };
            
            var city = db.Cities.FirstOrDefault(c => c.Name == session.CityName)
                       ?? new City { Name = session.CityName, Country = country };
        
            
            //Se vem det är som skapade ordern
             var order = new Order {
                 Customers = GetOrCreateCustomer(db, session),
                 Cities = city,
                 PaymentMethods = payment,
                 OrderDate = DateTime.Now,
                 TotalAmount = session.CartItem.Sum(kvp => kvp.Key.Price * kvp.Value),
                 Street = session.StreetName
             };
            
             //Detaljer om vem som köpte det och vad för items som köptes
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
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DATABASE ERROR: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"INNER: {ex.InnerException.Message}");
            throw;
        }
        // finally
        // {
        //     //Går tillbaks till initial states        
        //     session.Status = CheckoutState.ReviewingCart;
        //     session.State = MenuState.MainMenu;
        // }
    }
    
    private static Customer GetOrCreateCustomer(MyDbContext db, UserSession session)
    {
        // Kollar efter redan en existerande kund registrerad
        var customer = db.Customers.FirstOrDefault(c => 
            c.FirstName == session.FirstName && 
            c.LastName == session.LastName);

        //Om kunden inte finns så skapar vi en ny
        if (customer == null)
        {
            customer = new Customer 
            { 
                FirstName = session.FirstName, 
                LastName = session.LastName, 
                Street = session.StreetName 
            };
            db.Customers.Add(customer);
        }
        return customer;
    }
    
}