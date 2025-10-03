using AudioProj.DataLayer;
using AudioProj.Services;

var repo = new AudioRepository();
var audioService = new AudioService();

Console.WriteLine("1. Record and Save");
Console.WriteLine("2. List and Play Audio");
string choice = Console.ReadLine();

if (choice == "1")
{
    var audio = audioService.RecordAudio();
    repo.SaveToDatabase(audio);
    Console.WriteLine("Saved.)");
}
else if (choice == "2")
{
    var entries = repo.GetAllAudioEntries();

    if (entries.Count == 0)
    {
        Console.WriteLine("No recordings found.");
        return;
    }

    Console.WriteLine("Available recordings:");
    foreach (var entry in entries)
    {
        Console.WriteLine($"[{entry.Id}] Recorded at {entry.RecordedAt}");
    }

    Console.Write("Enter Id to play: ");
    if (int.TryParse(Console.ReadLine(), out int selectedId))
    {
        try
        {
            var audio = repo.LoadAudioById(selectedId);
            audioService.PlayAudio(audio);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Invalid input.");
    }
}