using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
class Program
{
    static void Main(string[] args)
    {
        string dbFileName = "train";
        SqliteConnection connection = new SqliteConnection($"Data Source={dbFileName}");
        TrainRepository trainRepository = new TrainRepository(connection);
        CarriageRepository carriageRepository = new CarriageRepository(connection);
        SeatRepository seatRepository = new SeatRepository(connection);

        Train train = new Train("Kiyv - Rivne", "Man");
        Carriage carriage = carriageRepository.GetById(5);
        carriageRepository.Insert(new Carriage("passanger", "Mersedes"));
        if(seatRepository.Insert(new Seat("ordinary", 2), carriageRepository) == 0)
        {
            Console.WriteLine("Error: such carriage not found or type is not passenger");
        }
        trainRepository.Update(1,new Train("Odessa", "ZAZ"));
        carriageRepository.Delete(carriage,seatRepository);
        carriageRepository.AddCarriageToTrain(carriage, train);
        List<Train> trains = trainRepository.FindTrains(2, 4);
        foreach (var item in trains)
            Console.WriteLine($"{item.id} {item.directions} {item.manufacturer}");

        List<Carriage> carriages = carriageRepository.FindTypeCarriges("passenger");
        foreach (var item in carriages)
            Console.WriteLine($"{item.id} {item.type} {item.manufacturer}");

        trainRepository.InsertRandom(5);

    }
}

