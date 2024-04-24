using Air_Tek;
using Newtonsoft.Json;

public class Program
{
    private static List<Flight> flights = new List<Flight>();
    public static void Main(string[] args)
    {
        LoadFlightSchedule();
        DisplayFlightSchedule();
        ScheduleOrders();
        DisplayScheduledOrders();
    }
    private static void LoadFlightSchedule()
    {
        flights.Add(new Flight { Number = 1, Departure = "YUL", Arrival = "YYZ", Day = 1 });
        flights.Add(new Flight { Number = 2, Departure = "YUL", Arrival = "YYC", Day = 1 });
        flights.Add(new Flight { Number = 3, Departure = "YUL", Arrival = "YVR", Day = 1 });
        flights.Add(new Flight { Number = 4, Departure = "YUL", Arrival = "YYZ", Day = 2 });
        flights.Add(new Flight { Number = 5, Departure = "YUL", Arrival = "YYC", Day = 2 });
        flights.Add(new Flight { Number = 6, Departure = "YUL", Arrival = "YVR", Day = 2 });
    }
    private static void DisplayFlightSchedule()
    {
        Console.WriteLine("Flight Schedule:");
        foreach (var flight in flights)
        {
            Console.WriteLine($"Flight: {flight.Number}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
        }
    }

    private static void ScheduleOrders()
    {
        using (var fileStream = File.OpenText("orders.json"))
        using (var reader = new JsonTextReader(fileStream))
        {
            var serializer = new JsonSerializer();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.StartObject)
                {
                    var order = serializer.Deserialize<Order>(reader);
                    ScheduleOrder(order);
                }
            }
        }
    }

    private static void ScheduleOrder(Order order)
    {
        bool scheduled = false;
        foreach (var flight in flights)
        {
            if (flight.Orders.Count < 20 && flight.Arrival == order.Destination)
            {
                flight.Orders.Add(order.OrderId);
                scheduled = true;
                break;
            }
        }
        if (!scheduled)
        {
            Console.WriteLine($"order: {order.OrderId}, flightNumber: not scheduled");
        }
    }

    private static void DisplayScheduledOrders()
    {
        Console.WriteLine("\nScheduled Orders:");
        foreach (var flight in flights)
        {
            foreach (var order in flight.Orders)
            {
                Console.WriteLine($"order: {order}, flightNumber: {flight.Number}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
            }
        }
    }
}