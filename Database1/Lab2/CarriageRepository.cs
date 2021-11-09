using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

class CarriageRepository
{
    private SqliteConnection connection;

    public CarriageRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public long Insert(Carriage carriage)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
                INSERT INTO carriage (type, manufacturer) 
                VALUES ($type, $manufacturer);
            
                SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$type", carriage.type);
        command.Parameters.AddWithValue("$manufacturer", carriage.manufacturer);

        long newId = (long)command.ExecuteScalar();

        connection.Close();
        return newId;
    }

    public bool CarriageExists(long id)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM carriage WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        SqliteDataReader reader = command.ExecuteReader();

        bool result = reader.Read();

        connection.Close();
        return result;
    }

    public Carriage GetById(long id)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM carriage WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        SqliteDataReader reader = command.ExecuteReader();
        Carriage carriage = new Carriage();
        if (reader.Read())
        {
            carriage.id = long.Parse(reader.GetString(0));
            carriage.type = reader.GetString(1);
            carriage.manufacturer = reader.GetString(2);
        }
        connection.Close();

        return carriage;
    }

    public bool Delete(Carriage carriage, SeatRepository seatRepository)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM carriage WHERE id = $id";
        command.Parameters.AddWithValue("$id", carriage.id);

        int nChanged = command.ExecuteNonQuery();

        if (nChanged != 0)
        {
            seatRepository.DeleteCarriageSeats(carriage);
        }

        connection.Close();

        return !(nChanged == 0);
    }

    public void AddCarriageToTrain(Carriage carriage, Train train)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO trains_carriages (trainId, carriageId)
        VALUES ($trainId, $carriageId)";
        command.Parameters.AddWithValue("$trainId", train.id);
        command.Parameters.AddWithValue("$carriageId", carriage.id);


        int nChanged = command.ExecuteNonQuery();

        connection.Close();
    }

    public List<Carriage> FindTypeCarriges(string type)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM carriage WHERE type LIKE $type";

        command.Parameters.AddWithValue("$type", type);

        SqliteDataReader reader = command.ExecuteReader();
        List<Carriage> carriages = new List<Carriage>();
        
        while(reader.Read())
        {
            Carriage c = new Carriage();
            c.id = long.Parse(reader.GetString(0));
            c.type = reader.GetString(1);
            c.manufacturer = reader.GetString(2);
            carriages.Add(c);
        }
        connection.Close();

        return carriages;
    }

}