using System;

public class Pupil
{
    public long id;
    public string surname;
    public string name;

    public Pupil(string surname, string name)
    {
        this.surname = surname;
        this.name = name;
    }

    public Pupil()
    {
        this.id = 0;
        this.surname = "";
        this.name = "";
    }

    public override string ToString()
    {
        return $"Id: {this.id}\t{this.surname} {this.name}";
    }

}