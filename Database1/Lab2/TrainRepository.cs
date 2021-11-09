using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

class TrainRepository
{
    private SqliteConnection connection;

    public TrainRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public long Insert(Train train)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
                INSERT INTO train (directions, manufacturer) 
                VALUES ($directions, $manufacturer);
            
                SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$directions", train.directions);
        command.Parameters.AddWithValue("$manufacturer", train.manufacturer);

        long newId = (long)command.ExecuteScalar();

        connection.Close();
        return newId;
    }

    public long InsertRandom(int n)
    {
        connection.Open();
        for (int i = 0; i < n; i++)
        {
            Random rnd = new Random();
            string alphabet = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            StringBuilder sb = new StringBuilder(5);
            int position = 0;
            for (int j = 0; j < 5; j++)
            {
                position = rnd.Next(0, alphabet.Length - 1);
                sb.Append(alphabet[position]);
            }

            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
        @"
                INSERT INTO train (directions, manufacturer) 
                VALUES ($directions, $manufacturer);
            
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$directions", sb.ToString());
            command.Parameters.AddWithValue("$manufacturer", sb.ToString());

            long newId = (long)command.ExecuteScalar();
        }
        connection.Close();
        return 0;
    }

    public bool Update(long id, Train train)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"UPDATE train SET directions = $directions WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$directions", train.directions);
        int rowChange = command.ExecuteNonQuery();

        connection.Close();
        if (rowChange == 0)
        {
            return false;
        }

        return true;
    }

    public bool Delete(Train train)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM train WHERE id = $id";
        command.Parameters.AddWithValue("$id", train.id);

        int nChanged = command.ExecuteNonQuery();

        connection.Close();
        return !(nChanged == 0);
    }

    public List<Train> FindTrains(int frontId, int endId)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM train WHERE id BETWEEN $frontId and $endId";

        command.Parameters.AddWithValue("$frontId", frontId);
        command.Parameters.AddWithValue("$endId", endId);

        SqliteDataReader reader = command.ExecuteReader();
        List<Train> trains = new List<Train>();

        while (reader.Read())
        {
            Train t = new Train();
            t.id = long.Parse(reader.GetString(0));
            t.directions = reader.GetString(1);
            t.manufacturer = reader.GetString(2);
            trains.Add(t);
        }
        connection.Close();

        return trains;
    }
}