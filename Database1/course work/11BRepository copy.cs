using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

public class _11BRepository
{
    private SqliteConnection connection;

    public _11BRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public List<Pupil> GetAll()
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM _11B";

        SqliteDataReader reader = command.ExecuteReader();

        List<Pupil> classroom = new List<Pupil>();
        while (reader.Read())
        {
            Pupil pupil = new Pupil();
            pupil.id = int.Parse(reader.GetString(0));
            pupil.surname = reader.GetString(1);
            pupil.name = reader.GetString(2);

            classroom.Add(pupil);
        }

        reader.Close();

        connection.Close();
        return classroom;
    }

    public Pupil GetById(long id)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM _11B WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        SqliteDataReader reader = command.ExecuteReader();

        Pupil pupil = new Pupil();
        if (reader.Read())
        {
            pupil.id = int.Parse(reader.GetString(0));
            pupil.surname = reader.GetString(1);
            pupil.name = reader.GetString(2);
        }

        reader.Close();

        connection.Close();
        return pupil;           //add if pupil == null then not found
    }

    public int DeleteById(long id, MathRepository mathRepository, BiologyRepository biologyRepository)
    {
        Pupil pupil1 = this.GetById(id);
        Subject mathPupil = mathRepository.GetSameStudent(pupil1);
        Subject bioPupil = biologyRepository.GetSameStudent(pupil1);

        mathRepository.DeleteById(mathPupil.id);
        biologyRepository.DeleteById(bioPupil.id);
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM _11B WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        int nChange = command.ExecuteNonQuery();

        connection.Close();
        return nChange;           //add if nChange == 0 then not deleted
    }

    public bool Update(long id, Pupil pupil, MathRepository mathRepository, BiologyRepository biologyRepository)
    {
        Pupil pupil1 = this.GetById(id);
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"UPDATE _11B SET surname = $surname, name = $name WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$surname", pupil.surname);
        command.Parameters.AddWithValue("$name", pupil.name);
        int rowChange = command.ExecuteNonQuery();

        Subject mathPupil = mathRepository.GetSameStudent(pupil1);

        mathRepository.Update(mathPupil.id, new Subject(pupil.surname, pupil.name, mathPupil.classroom, mathPupil.mark));

        Subject bioPupil = biologyRepository.GetSameStudent(pupil1);

        biologyRepository.Update(bioPupil.id, new Subject(pupil.surname, pupil.name, bioPupil.classroom, bioPupil.mark));

        connection.Close();
        if (rowChange == 0)
        {
            return false;
        }

        return true;
    }

    public long Insert(string surname, string name)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @" 
            INSERT INTO _11B (surname, name)
            VALUES ($surname, $name)
            
            SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$surname", surname);
        command.Parameters.AddWithValue("$name", name);

        long newId = (long)command.ExecuteScalar();         //add if ==0 not added


        command.CommandText =
       @" 
            INSERT INTO math (surname, name, class, mark)
            VALUES ($surname, $name, $class, $mark);

            INSERT INTO biology (surname, name, class, mark)
            VALUES ($surname, $name, $class, $mark)
        ";

        command.Parameters.AddWithValue("$class", "11B");
        command.Parameters.AddWithValue("$mark", 0);
        command.ExecuteScalar();

        connection.Close();
        return newId;
    }

    public void GenerateClassmates(int n)
    {
        Random rnd = new Random();
        List<Pupil> pupils = new List<Pupil>();
        string[] randomNames = { "Петро", "Микола", "Сергій", "Панас", "Юрій", "Василь", "Олександр" };
        string[] randomSurnames = { "Шваб", "Вознюк", "Мошківський", "Чичирко", "Аксенчик", "Мороз", "Назиров" };

        for (int i = 0; i < n; i++)
        {
            Pupil pupil = new Pupil(randomSurnames[rnd.Next(1, randomSurnames.Length)], randomNames[rnd.Next(1, randomNames.Length)]);
            pupils.Add(pupil);
        }

        foreach (var item in pupils)
            this.Insert(item.surname, item.name);
    }
}