using Air_Tek;
using Newtonsoft.Json.Linq;

public class Program
{
    private static List<Flight> flights = new List<Flight>();
    public static async Task Main(string[] args)
    {
        await LoadFlightSchedule();
        await DisplayFlightSchedule();
        await ScheduleOrders();
        await DisplayScheduledOrders();
    }
    private static async Task LoadFlightSchedule()
    {
        await Task.Run(() =>
        {
            flights.Add(new Flight { Number = 1, Departure = "YUL", Arrival = "YYZ", Day = 1 });
            flights.Add(new Flight { Number = 2, Departure = "YUL", Arrival = "YYC", Day = 1 });
            flights.Add(new Flight { Number = 3, Departure = "YUL", Arrival = "YVR", Day = 1 });
            flights.Add(new Flight { Number = 4, Departure = "YUL", Arrival = "YYZ", Day = 2 });
            flights.Add(new Flight { Number = 5, Departure = "YUL", Arrival = "YYC", Day = 2 });
            flights.Add(new Flight { Number = 6, Departure = "YUL", Arrival = "YVR", Day = 2 });
        });
       
    }
    private static async Task DisplayFlightSchedule()
    {
        await Task.Run(() =>
        {
            Console.WriteLine("Flight Schedule:");
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight: {flight.Number}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
            }
        });

        
    }

    private static async Task ScheduleOrders()
    {
        await Task.Run(() =>
        {
            JObject ordersJson = JObject.Parse(File.ReadAllText("orders.json"));

            foreach (var order in ordersJson)
            {
                string orderId = order.Key;
                string destination = (string)order.Value["destination"];

                bool scheduled = false;
                foreach (var flight in flights)
                {
                    if (flight.Orders.Count < 20 && flight.Arrival == destination)
                    {
                        flight.Orders.Add(orderId);
                        scheduled = true;
                        break;
                    }
                }
                if (!scheduled)
                {
                    Console.WriteLine($"order: {orderId}, flightNumber: not scheduled");
                }
            }
        });
        
    }

    private static async Task DisplayScheduledOrders()
    {
        await Task.Run(() =>
        {
            Console.WriteLine("\nScheduled Orders:");
            foreach (var flight in flights)
            {
                foreach (var order in flight.Orders)
                {
                    Console.WriteLine($"order: {order}, flightNumber: {flight.Number}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
                }
            }
        });
       
    }
}