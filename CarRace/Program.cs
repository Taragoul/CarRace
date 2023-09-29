

  class CarRace
{
    public static readonly object ConsoleLock = new object();
    public static readonly Random Random = new Random();
    public static readonly int RaceDistance = 15000;
    public static readonly int BaseSpeed = 120;
    public static readonly int MaxAcceleration = 10;

    static async Task Main()
    {
        List<Car> cars = new List<Car>
        {
            new Car("Car 1"),
            new Car("Car 2")
        };

        Console.WriteLine("Press Enter to start the race...");
        Console.ReadLine();

        var raceTasks = new List<Task>();

        foreach (var car in cars)
        {
            raceTasks.Add(car.StartRaceAsync());
        }

        // Allow the user to check the race status by pressing Enter.
        _ = Task.Run(() =>
        {
            while (true)
            {
                if (Console.ReadLine().ToLower() == "status")
                {
                    lock (ConsoleLock)
                    {
                        foreach (var car in cars)
                        {
                            Console.WriteLine($"{car.Name}: Position {car.Position} km, Speed {car.Speed} km/h");
                        }
                    }
                }
            }
        });

        await Task.WhenAll(raceTasks);
    }
}

class Car
{
    public string Name { get; }
    public int Position { get; private set; }
    public int Speed { get; private set; }

    public Car(string name)
    {
        Name = name;
    }

    public async Task StartRaceAsync()
    {
        Console.WriteLine($"{Name} starts the race!");

        while (Position < CarRace.RaceDistance)
        {
            await DriveAsync();
        }

        Console.WriteLine($"{Name} has finished the race!");
    }

    async Task DriveAsync()
    {
        await Task.Delay(1000); // Simulate time passing

        int acceleration = CarRace.Random.Next(-CarRace.MaxAcceleration, CarRace.MaxAcceleration + 1);
        Speed = CarRace.BaseSpeed + acceleration;

        Position += Speed;

        if (CarRace.Random.Next(50) == 0)
        {
            await HandleRandomEventAsync();
        }
    }

    async Task HandleRandomEventAsync()
    {
        int randomEvent = CarRace.Random.Next(50);

        if (randomEvent <15)
        {
            await RefuelAsync();
        }
        else if (randomEvent >= 15 && randomEvent < 25)
        {
            await TireChangeAsync();
        }
        else if (randomEvent >= 25 && randomEvent <=40)
        {
            await WashWindshieldAsync();
        }
        else if (randomEvent >40)
        {
            ReduceSpeed();
        }
    }

    async Task RefuelAsync()
    {
        Console.WriteLine($"{Name} is out of gas. Refueling...");
        await Task.Delay(8000); // Simulate refueling time
        Console.WriteLine($"{Name} has refueled and is back on the road.");
    }

    async Task TireChangeAsync()
    {
        Console.WriteLine($"{Name} has a puncture. Changing tires...");
        await Task.Delay(5000); // Simulate tire change time
        Console.WriteLine($"{Name} has changed tires and is back on the road.");
    }

    async Task WashWindshieldAsync()
    {
        Console.WriteLine($"{Name} has a bird on the windshield. Washing windshield...");
        await Task.Delay(1000); // Simulate windshield wash time
        Console.WriteLine($"{Name} has a clean windshield and is back on the road.");
    }

    void ReduceSpeed()
    {
        Console.WriteLine($"{Name} has engine failure. Reducing speed by 15 km/h.");
        Speed -= 15;
    }
}