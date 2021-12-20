using System.Collections.Generic;
using Microsoft.Data.Sqlite;
public class BiologyRepository
{
    private SqliteConnection connection;

    public BiologyRepository(SqliteConnection connection)
    {
        this.connection = connection;
    }

    public List<Subject> GetAll()
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"SELECT * FROM biology";

        SqliteDataReader reader = command.ExecuteReader();

        List<Subject> subject = new List<Subject>();
        while (reader.Read())
        {
            Subject subj = new Subject();
            subj.id = int.Parse(reader.GetString(0));
            subj.surname = reader.GetString(1);
            subj.name = reader.GetString(2);
            subj.classroom = reader.GetString(3);
            subj.mark = int.Parse(reader.GetString(4));

            subject.Add(subj);
        }

        reader.Close();

        connection.Close();
        return subject;
    }

    public void ShowMarks()
    {
        List<Subject> subjects = this.GetAll();

        List<int> marks = new List<int>();
        foreach (var item in subjects)
        {
            marks.Add(item.mark);
        }

        double[] countedMarks = new double[13];
        for (int i = 0; i < countedMarks.Length; i++)
        {
            int counter = 0;
            for (int j = 0; j < marks.Count; j++)
            {
                if (i == marks[j])
                    counter++;
            }
            countedMarks[i] = counter;
        }

        var plt = new ScottPlot.Plot(600, 400);
        double[] values = countedMarks;
        double[] positions = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        string[] labels = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

        var bar = plt.AddBar(values, positions);

        plt.XTicks(positions, labels);

        plt.SetAxisLimits(yMin: 0);
        plt.XLabel("Marks");
        plt.YLabel("Count");

        plt.SaveFig("plot.png");
    }

    public bool ChangeMark(int id, int mark)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"UPDATE math SET mark = $mark WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$mark", mark);

        int rowChange = command.ExecuteNonQuery();

        connection.Close();
        if (rowChange == 0)
        {
            return false;
        }

        return true;
    }

    public Subject GetSameStudent(Pupil pupil)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM biology WHERE surname = $surname AND name = $name";
        command.Parameters.AddWithValue("$surname", pupil.surname);
        command.Parameters.AddWithValue("$name", pupil.name);

        SqliteDataReader reader = command.ExecuteReader();

        Subject bioPupil = new Subject();
        if (reader.Read())
        {
            bioPupil.id = int.Parse(reader.GetString(0));
            bioPupil.surname = reader.GetString(1);
            bioPupil.name = reader.GetString(2);
            bioPupil.classroom = reader.GetString(3);
            bioPupil.mark = int.Parse(reader.GetString(4));
        }
        reader.Close();
    

        return bioPupil;
    }

    public bool Update(long id, Subject subj)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"UPDATE biology SET surname = $surname, name = $name, class = $class, mark = $mark WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$surname", subj.surname);
        command.Parameters.AddWithValue("$name", subj.name);
        command.Parameters.AddWithValue("$class", subj.classroom);
        command.Parameters.AddWithValue("$mark", subj.mark);

        int rowChange = command.ExecuteNonQuery();

        connection.Close();
        if (rowChange == 0)
        {
            return false;
        }

        return true;
    }

    public int DeleteById(long id)
    {
        connection.Open();

        SqliteCommand command = connection.CreateCommand();
        command.CommandText = @"DELETE FROM biology WHERE id = $id";
        command.Parameters.AddWithValue("$id", id);

        int nChange = command.ExecuteNonQuery();

        connection.Close();
        return nChange;           //add if nChange == 0 then not deleted
    }

    public long Insert(string surname, string name, string classroom, int mark)
    {
        connection.Open();
        SqliteCommand command = connection.CreateCommand();
        command.CommandText =
        @" 
            INSERT INTO biology (surname, name, class, mark)
            VALUES ($surname, $name, $class, $mark)
            
            SELECT last_insert_rowid();
        ";
        command.Parameters.AddWithValue("$surname", surname);
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$class", classroom);
        command.Parameters.AddWithValue("$mark", mark);

        long newId = (long)command.ExecuteScalar();         //add if ==0 not added

        connection.Close();
        return newId;
    }
}
