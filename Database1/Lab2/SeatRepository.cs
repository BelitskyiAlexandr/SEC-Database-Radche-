using System;
using Microsoft.Data.Sqlite;

class SeatRepository
{
    private SqliteConnection connection;

    public SeatRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public long Insert(Seat seat, CarriageRepository carriageRepository)
    {
        if (!carriageRepository.CarriageExists(seat.carriageId))
        {
            return 0;
        }
        if (!(carriageRepository.GetById(seat.carriageId).type == "passenger"))
        {
            return 0;
        }
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @"
                INSERT INTO seat (type, carriageId) 
                VALUES ($type, $carriageId);
            
                SELECT last_insert_rowid();
            ";
        command.Parameters.AddWithValue("$type", seat.type);
        command.Parameters.AddWithValue("$carriageId", seat.carriageId);

        long newId = (long)command.ExecuteScalar();

        connection.Close();
        return newId;
    }

    public bool Update(long id, Seat seat, CarriageRepository carriageRepository)
    {
        if (!carriageRepository.CarriageExists(seat.carriageId))
        {
            return false;
        }
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"UPDATE seat SET type = $type WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$type", seat.type);
        int rowChange = command.ExecuteNonQuery();

        connection.Close();
        if (rowChange == 0)
        {
            return false;
        }

        return true;
    }

    public bool Delete(Seat seat)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM seat WHERE id = $id";
        command.Parameters.AddWithValue("$id", seat.id);

        int nChanged = command.ExecuteNonQuery();

        connection.Close();
        return !(nChanged == 0);
    }

    public int DeleteCarriageSeats(Carriage carriage)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM seat WHERE carriageId = $carriageId";
        command.Parameters.AddWithValue("$carriageId", carriage.id);

        int nChanged = command.ExecuteNonQuery();


        return nChanged;

    }
}