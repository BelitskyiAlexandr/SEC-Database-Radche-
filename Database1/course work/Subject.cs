using System;

public class Subject
{
    public long id;
    public string surname;
    public string name;
    public string classroom;
    public int mark;


    public Subject(string surname, string name, string classroom, int mark)
    {
        this.surname = surname;
        this.name = name;
        this.classroom = classroom;
        this.mark = mark; 
    }

    public Subject()
    {
        this.id = 0;
        this.surname = "";
        this.name = "";
        this.classroom = "";
        this.mark = 0;
    }

    public override string ToString()
    {
        return $"Pupil: {this.surname} {this.name} \tId: {this.id}\nClass: {this.classroom} Mark: {this.mark}";
    }

}