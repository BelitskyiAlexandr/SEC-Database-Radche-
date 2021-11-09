using System;

class Train 
{
    public long id;
    public string directions;
    public string manufacturer;

    public Train(string directions, string manufacturer)
    {
        this.directions = directions;
        this.manufacturer = manufacturer;
    }

    public Train()
    {
        this.id = 0;
        this.directions = "";
        this.manufacturer = "";
    }
}