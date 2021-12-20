using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        string databaseFileName = "journaldb";
        SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
        _11ARepository _11arepo = new _11ARepository(connection);
        _11BRepository _11brepository = new _11BRepository(connection);
        MathRepository mathRepository = new MathRepository(connection);
        BiologyRepository biologyRepository = new BiologyRepository(connection);

        Console.WriteLine("Welcome! ");
        while (true)
        {
            Console.WriteLine("Choose working mode(11A, 11B, math, biology) or do 'backup' or 'change connection':");
            string str = Console.ReadLine();
            if (str.Trim() == "11A")
            {
                _11AProcess(_11arepo, mathRepository, biologyRepository);
            }
            else if (str.Trim() == "11B")
            {
                _11BProcess(_11brepository, mathRepository, biologyRepository);
            }
            else if (str.Trim() == "math")
            {
                MathProcess(mathRepository);
            }
            else if (str.Trim() == "biology")
            {
                BiologyProcess(biologyRepository);
            }
            else if (str.Trim() == "backup")
            {
                if (System.IO.File.Exists("./journaldbbackup"))
                {
                    System.IO.File.Delete("./journaldbbackup");
                    System.IO.File.Copy("journaldb", "journaldbbackup");
                }
                else
                {
                    System.IO.File.Copy("journaldb", "journaldbbackup");
                }
                Console.WriteLine("Backup was created");
            }
            else if (str.Trim() == "change connection")
            {
                if (System.IO.File.Exists("./journaldbbackup"))
                {
                    if (databaseFileName == "journaldbbackup")
                    {
                        databaseFileName = "journaldb";
                        connection = new SqliteConnection($"Data Source={databaseFileName}");
                        _11arepo = new _11ARepository(connection);
                        _11brepository = new _11BRepository(connection);
                        mathRepository = new MathRepository(connection);
                        biologyRepository = new BiologyRepository(connection);
                    }
                    else if (databaseFileName == "journaldb")
                    {
                        databaseFileName = "journaldbbackup";
                        connection = new SqliteConnection($"Data Source={databaseFileName}");
                        _11arepo = new _11ARepository(connection);
                        _11brepository = new _11BRepository(connection);
                        mathRepository = new MathRepository(connection);
                        biologyRepository = new BiologyRepository(connection);
                    }
                }
                else
                {
                    Console.WriteLine("There is no back up");
                }
                Console.WriteLine("Connection was changed");
            }
            else if (str.Trim() == "exit")
            {
                Console.WriteLine("Ending proccessing...");
                break;
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }

    public static void BiologyProcess(BiologyRepository biologyRepository)
    {
        while (true)
        {
            Console.WriteLine("Choose command(change mark, get all, diagram): ");
            string str = Console.ReadLine().Trim();
            if (str == "back")
            {
                break;
            }
            else if (str == "get all")
            {
                List<Subject> students = biologyRepository.GetAll();
                foreach (var item in students)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else if (str == "diagram")
            {
                biologyRepository.ShowMarks();
                Console.WriteLine("Diagram was created. Check png file");
            }
            else if (str == "change mark")
            {
                while (true)
                {
                    Console.WriteLine("Enter id: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int id))
                    {
                        Console.WriteLine("Enter mark: ");
                        string marks = Console.ReadLine();
                        if (marks.Trim() == "back")
                            break;

                        if (int.TryParse(marks, out int mark))
                        {
                            if (mark < 0 || mark > 12)
                            {
                                Console.WriteLine("Mark must bu from 0 to 12");
                                continue;
                            }
                            if (biologyRepository.ChangeMark(id, mark))
                            {
                                Console.WriteLine("Student was updated by id: {0}", id);
                            }
                            else
                            {
                                Console.WriteLine("Student did not update");
                                //break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter number");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }

    public static void MathProcess(MathRepository mathRepository)
    {
        while (true)
        {
            Console.WriteLine("Choose command(change mark, get all, diagram): ");
            string str = Console.ReadLine().Trim();
            if (str == "back")
            {
                break;
            }
            else if (str == "get all")
            {
                List<Subject> students = mathRepository.GetAll();
                foreach (var item in students)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else if (str == "diagram")
            {
                mathRepository.ShowMarks();
                Console.WriteLine("Diagram was created. Check png file");
            }
            else if (str == "change mark")
            {
                while (true)
                {
                    Console.WriteLine("Enter id: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int id))
                    {
                        Console.WriteLine("Enter mark: ");
                        string marks = Console.ReadLine();
                        if (marks.Trim() == "back")
                            break;

                        if (int.TryParse(marks, out int mark))
                        {
                            if (mark < 0 || mark > 12)
                            {
                                Console.WriteLine("Mark must bu from 0 to 12");
                                continue;
                            }
                            if (mathRepository.ChangeMark(id, mark))
                            {
                                Console.WriteLine("Student was updated by id: {0}", id);
                            }
                            else
                            {
                                Console.WriteLine("Student did not update");
                                //break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter number");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }

    public static void _11AProcess(_11ARepository _11arepo, MathRepository mathRepository, BiologyRepository biologyRepository)
    {
        while (true)
        {
            Console.WriteLine("Choose command(insert, delete by id, update by id, get by id, get all, generate): ");
            string str = Console.ReadLine().Trim();
            if (str == "back")
            {
                break;
            }
            else if (str == "insert")
            {
                Console.WriteLine("Enter surname: ");
                string sur = Console.ReadLine();
                if (sur.Trim() == "back")
                    break;
                Console.WriteLine("Enter name: ");
                string name = Console.ReadLine();
                Console.WriteLine("Student was added by id: {0}", _11arepo.Insert(sur, name));
            }
            else if (str == "delete by id")
            {
                while (true)
                {
                    Console.WriteLine("Enter id: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int id))
                    {
                        if (_11arepo.DeleteById(id, mathRepository, biologyRepository) == 0)
                        {
                            Console.WriteLine("Student did not delete");
                        }
                        else
                        {
                            Console.WriteLine("Student was deleted by id: {0}", id);
                            //break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else if (str == "update by id")
            {
                while (true)
                {
                    Console.WriteLine("Enter id: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int id))
                    {
                        Console.WriteLine("Enter surname: ");
                        string sur = Console.ReadLine();
                        Console.WriteLine("Enter name: ");
                        string name = Console.ReadLine();
                        if (_11arepo.Update(id, new Pupil(sur, name), mathRepository, biologyRepository))
                        {
                            Console.WriteLine("Student was update by id: {0}", id);
                            //break;
                        }
                        else
                        {
                            Console.WriteLine("Student did not update");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else if (str == "get by id")
            {
                while (true)
                {
                    Console.WriteLine("Enter id: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int id))
                    {
                        Pupil student = _11arepo.GetById(id);
                        if (student.id == 0)
                        {
                            Console.WriteLine("Student did not found");
                        }
                        else
                        {
                            Console.WriteLine("Student was found by id: {0}", student.ToString());
                            //break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else if (str == "get all")
            {
                List<Pupil> students = _11arepo.GetAll();
                foreach (var item in students)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else if (str == "generate")
            {
                while (true)
                {
                    Console.WriteLine("Enter number: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int number))
                    {
                        _11arepo.GenerateClassmates(number);
                        Console.WriteLine("Data was generated");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }

    public static void _11BProcess(_11BRepository _11brepo, MathRepository mathRepository, BiologyRepository biologyRepository)
    {
        while (true)
        {
            Console.WriteLine("Choose command(insert, delete by id, update by id, get by id, get all, generate): ");
            string str = Console.ReadLine().Trim();
            if (str == "back")
            {
                break;
            }
            else if (str == "insert")
            {
                Console.WriteLine("Enter surname: ");
                string sur = Console.ReadLine();
                if (sur.Trim() == "back")
                    break;
                Console.WriteLine("Enter name: ");
                string name = Console.ReadLine();
                Console.WriteLine("Student was added by id: {0}", _11brepo.Insert(sur, name));
            }
            else if (str == "delete by id")
            {
                while (true)
                {
                    Console.WriteLine("Enter id: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int id))
                    {
                        if (_11brepo.DeleteById(id, mathRepository, biologyRepository) == 0)
                        {
                            Console.WriteLine("Student did not delete");
                        }
                        else
                        {
                            Console.WriteLine("Student was deleted by id: {0}", id);
                            //break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else if (str == "update by id")
            {
                while (true)
                {
                    Console.WriteLine("Enter id: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int id))
                    {
                        Console.WriteLine("Enter surname: ");
                        string sur = Console.ReadLine();
                        Console.WriteLine("Enter name: ");
                        string name = Console.ReadLine();
                        if (_11brepo.Update(id, new Pupil(sur, name), mathRepository, biologyRepository))
                        {
                            Console.WriteLine("Student was update by id: {0}", id);
                            //break;
                        }
                        else
                        {
                            Console.WriteLine("Student did not update");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else if (str == "get by id")
            {
                while (true)
                {
                    Console.WriteLine("Enter id: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int id))
                    {
                        Pupil student = _11brepo.GetById(id);
                        if (student.id == 0)
                        {
                            Console.WriteLine("Student did not found");
                        }
                        else
                        {
                            Console.WriteLine("Student was found by id: {0}", student.ToString());
                            //break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else if (str == "get all")
            {
                List<Pupil> students = _11brepo.GetAll();
                foreach (var item in students)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            else if (str == "generate")
            {
                while (true)
                {
                    Console.WriteLine("Enter number: ");
                    string s = Console.ReadLine();
                    if (s.Trim() == "back")
                        break;
                    if (int.TryParse(s, out int number))
                    {
                        _11brepo.GenerateClassmates(number);
                        Console.WriteLine("Data was generated");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Please enter number");
                    }
                }
            }
            else
            {
                Console.WriteLine("Unknown command");
            }
        }
    }
}

