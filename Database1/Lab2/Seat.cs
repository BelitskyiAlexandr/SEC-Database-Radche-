using System;

class Seat 
{
    public long id;
    public string type;
    public long carriageId;

    public Seat(string type, long carriageId)
    {
        this.type = type;
        this.carriageId = carriageId;
    }

    public Seat()
    {
        this.id = 0;
        this.type = "";
        this.carriageId = 0;
    }
}