using Air_Tek;
using Newtonsoft.Json.Linq;

public interface IFlightScheduleLoader
{
    List<Flight> LoadFlightSchedule();
}

public interface IOrderScheduler
{
    void ScheduleOrders(List<Flight> flights);
}



public class Program
{
    private static readonly IFlightScheduleLoader _flightScheduleLoader = new FlightScheduleLoader();
    private static readonly IOrderScheduler _orderScheduler = new OrderScheduler();

    public static void Main(string[] args)
    {
        var flights = _flightScheduleLoader.LoadFlightSchedule();
        DisplayFlightSchedule(flights);
        _orderScheduler.ScheduleOrders(flights);
        DisplayScheduledOrders(flights);

    }
    public class FlightScheduleLoader : IFlightScheduleLoader
    {
        public List<Flight> LoadFlightSchedule()
        {
            var flights = new List<Flight>();
            flights.Add(new Flight { Number = 1, Departure = "YUL", Arrival = "YYZ", Day = 1 });
            flights.Add(new Flight { Number = 2, Departure = "YUL", Arrival = "YYC", Day = 1 });
            flights.Add(new Flight { Number = 3, Departure = "YUL", Arrival = "YVR", Day = 1 });
            flights.Add(new Flight { Number = 4, Departure = "YUL", Arrival = "YYZ", Day = 2 });
            flights.Add(new Flight { Number = 5, Departure = "YUL", Arrival = "YYC", Day = 2 });
            flights.Add(new Flight { Number = 6, Departure = "YUL", Arrival = "YVR", Day = 2 });
            return flights;
        }
    }

    public class OrderScheduler : IOrderScheduler
    {
        public void ScheduleOrders(List<Flight> flights)
        {
            try
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
                        Console.WriteLine($"Order: {orderId}, flightNumber: not scheduled");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error scheduling orders: " + ex.Message);
            }
        }
    }
    private static void DisplayFlightSchedule(List<Flight> flights)
    {
        Console.WriteLine("Flight Schedule:");
        foreach (var flight in flights)
        {
            Console.WriteLine($"Flight: {flight.Number}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
        }
    }

    private static void DisplayScheduledOrders(List<Flight> flights)
    {
        Console.WriteLine("\nScheduled Orders:");
        foreach (var flight in flights)
        {
            foreach (var order in flight.Orders)
            {
                Console.WriteLine($"Order: {order}, flightNumber: {flight.Number}, departure: {flight.Departure}, arrival: {flight.Arrival}, day: {flight.Day}");
            }
        }



    }
}