using System;

class Carriage
{
    public long id;
    public string type;
    public string manufacturer;

    public Carriage(string type, string manufacturer)
    {
        this.type = type;
        this.manufacturer = manufacturer;
    }

    public Carriage()
    {
        this.id = 0;
        this.type = "";
        this.manufacturer = "";
    }
}