using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

string file = "mario.csv";
// make sure movie file exists
if (!File.Exists(file))
{
    logger.Error("File does not exist: {File}", file);
}
else
{
    // create parallel lists of character details
    // lists are used since we do not know number of lines of data
    List<Character> characters = [];
    // to populate the lists with data, read from the data file
    try
    {
        StreamReader sr = new(file);
        // first line contains column headers
        sr.ReadLine();
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (line is not null)
            {
                Character character = new();
                // character details are separated with comma(,)
                string[] characterDetails = line.Split(',');
                // 1st array element contains id
                character.Ids = UInt64.Parse(characterDetails[0]);
                // 2nd array element contains character name
                character.Names = characterDetails[1];
                // 3rd array element contains character description
                character.Descriptions = characterDetails[2];
                // 4th array element contains species
                character.Species = characterDetails[3];
                // 5th array element contains character first appearances
                character.FirstAppearances = characterDetails[4];
                // 3rd array element contains character year created
                character.YearsCreated = UInt64.Parse(characterDetails[5]);
                characters.Add(character);
            }
        }
        sr.Close();
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }

    string? choice;
    do
    {
        // display choices to user
        Console.WriteLine("1) Add Character");
        Console.WriteLine("2) Display All Characters");
        Console.WriteLine("Enter to quit");

        // input selection
        choice = Console.ReadLine();
        logger.Info("User choice: {Choice}", choice);

        if (choice == "1")
        {
            // Add Character
            Character character = new();
            Console.WriteLine("Enter new character name: ");
            character.Names = Console.ReadLine() ?? string.Empty;
            if (!string.IsNullOrEmpty(character.Names)){
                // check for duplicate name
                List<string> LowerCaseNames = characters.ConvertAll(character => character.Names.ToLower());
                if (LowerCaseNames.Contains(character.Names.ToLower()))
                {
                    logger.Info($"Duplicate name {character.Names}");
                }
                else
                {
                    // generate id - use max value in Ids + 1
                    character.Ids = characters.Max(character => character.Ids) + 1;
                    // input character description
                    Console.WriteLine("Enter description:");
                    character.Descriptions = Console.ReadLine() ??string.Empty;
                    // input character species
                    Console.WriteLine("Enter species:");
                    character.Species = Console.ReadLine() ??string.Empty;
                    // input character first appearance
                    Console.WriteLine("Enter first appearance:");
                    character.FirstAppearances = Console.ReadLine() ??string.Empty;
                    // input character year created
                    Console.WriteLine("Enter year created:");
                    character.YearsCreated = UInt64.Parse(Console.ReadLine() ??string.Empty);

                    // Console.WriteLine($"{Id}, {Name}, {Description}, {Specie}, {FirstAppearance}, {YearCreated}");
                    // create file from data
                    StreamWriter sw = new(file, true);
                    sw.WriteLine($"{character.Ids},{character.Names},{character.Descriptions},{character.Species},{character.FirstAppearances},{character.YearsCreated}");
                    sw.Close();
                    // add new character details to Lists
                    characters.Add(character);
                    // log transaction
                    logger.Info($"Character id {character.Ids} added");
                }
            } else {
                logger.Error("You must enter a name");
            }
        }
    //     else if (choice == "2")
    //     {
    //         // Display All Characters
    //         // loop thru Lists
    //         for (int i = 0; i < Ids.Count; i++)
    //         {
    //             // display character details
    //             Console.WriteLine($"Id: {Ids[i]}");
    //             Console.WriteLine($"Name: {Names[i]}");
    //             Console.WriteLine($"Description: {Descriptions[i]}");
    //             Console.WriteLine($"Species: {Species[i]}");
    //             Console.WriteLine($"First-Appearance: {FirstAppearances[i]}");
    //             Console.WriteLine($"Year-Created: {YearsCreated[i]}");
    //             Console.WriteLine();
    //         }
    //     }
    } while (choice == "1" || choice == "2");
}

logger.Info("Program ended");