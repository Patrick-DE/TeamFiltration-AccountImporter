using System;
using System.IO;
using LiteDB;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Usage: dotnet run <database_name> <file_path>");
            return;
        }

        string databaseName = args[0];
        string fileName = args[1];
        int counter = 0;

        try { 
            // Create a new LiteDB database and connect to it
            using (var db = new LiteDatabase(databaseName))
            {
                // Get a reference to the validaccounts collection
                var collection = db.GetCollection<ValidAccount>("validaccounts");
                
                if (collection == null) {
                    Console.WriteLine("The table \"validaccounts\" does not exist!");
                    return;
                }

                // Read the lines from the text file
                string[] lines = File.ReadAllLines(fileName);

                // Loop through each line and insert it into the collection
                foreach (string line in lines)
                {
                    // Generate a new UUID for the _id field
                    String uuid = Guid.NewGuid().ToString();

                    // Create a new ValidAccount object with the Username and _id fields
                    var account = new ValidAccount
                    {
                        Username = line,
                        _id = uuid
                    };

                    // Insert the account object into the collection
                    if (account.Username != "") { 
                        collection.Insert(account);
                        counter++;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("Import complete.");
        Console.WriteLine(counter + " entries imported!");
        Console.ReadKey(true);
    }
}

// Define a class to represent the ValidAccount object in the LiteDB database
public class ValidAccount
{
    public string Username { get; set; }
    public string _id { get; set; }
}
