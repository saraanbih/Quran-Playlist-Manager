namespace Quran_Playlist_Manager
{
    class Program
    {
        static PlaylistManager playlistManager = new PlaylistManager();
        static PlaybackService playbackService;
        static StatisticsService statisticsService;

        static async Task Main()
        {
            Console.WriteLine("\t\tWelcome to the Quran Playlist Manager");
            Console.WriteLine("=================================================================");

            playlistManager.CreatePlaylist("My Quran Playlist");
            playbackService = new PlaybackService(playlistManager.CurrentPlaylist);
            statisticsService = new StatisticsService(playbackService);

            // Add sample songs
            playlistManager.AddSong(new Song("Surah Al-Fatiha", "Qari Abdul Basit", TimeSpan.FromSeconds(5)));
            playlistManager.AddSong(new Song("Surah Al-Baqara", "Sheikh Sudais", TimeSpan.FromSeconds(6)));
            playlistManager.AddSong(new Song("Surah Al-Mulk", "Saad Al-Ghamdi", TimeSpan.FromSeconds(7)));

            Console.WriteLine("\n\t\tDefault Playlist & Songs Added!");
            DisplayMenu();

            while (true)
            {
                Console.Write("\nEnter your choice: ");
                string choice = Console.ReadLine();
                Console.Clear();
                await HandleMenuChoice(choice);
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\n\t\tQuran Playlist Manager - Main Menu");
            Console.WriteLine("================================================================");
            Console.WriteLine("1.  View Playlists");
            Console.WriteLine("2.  Create New Playlist");
            Console.WriteLine("3.  Switch Playlist");
            Console.WriteLine("4.  Add Song to Current Playlist");
            Console.WriteLine("5.  Remove Song");
            Console.WriteLine("6.  View Songs in Playlist");
            Console.WriteLine("7.  Search for a Song");
            Console.WriteLine("8.  Play Songs");
            Console.WriteLine("9.  Skip to Next Song");
            Console.WriteLine("10. Toggle Shuffle Mode");
            Console.WriteLine("11. Toggle Repeat Mode");
            Console.WriteLine("12. Show Most Played Songs");
            Console.WriteLine("13. Exit");

        }

        static async Task HandleMenuChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    playlistManager.ViewPlaylists();
                    break;
                case "2":
                    Console.Write("Enter new playlist name: ");
                    string newPlaylistName = Console.ReadLine();
                    playlistManager.CreatePlaylist(newPlaylistName);
                    Console.WriteLine($"Playlist '{newPlaylistName}' created.");
                    break;
                case "3":
                    Console.Write("Enter playlist name to switch: ");
                    string switchPlaylist = Console.ReadLine();
                    playlistManager.SwitchPlaylist(switchPlaylist);
                    playbackService = new PlaybackService(playlistManager.CurrentPlaylist);
                    Console.WriteLine($"Switched to playlist '{switchPlaylist}'.");
                    break;
                case "4":
                    Console.Write("Enter song title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter artist name: ");
                    string artist = Console.ReadLine();
                    playlistManager.AddSong(new Song(title, artist, TimeSpan.FromSeconds(5)));
                    Console.WriteLine($" Added '{title}' by {artist} to the playlist.");
                    break;
                case "5":
                    Console.Write("Enter song title to remove: ");
                    string removeTitle = Console.ReadLine();
                    playlistManager.RemoveSong(removeTitle);
                    Console.WriteLine($" Removed '{removeTitle}' from the playlist.");
                    break;
                case "6":
                    playlistManager.ViewSongs();
                    break;
                case "7":
                    Console.Write("Enter song title to search: ");
                    string searchTitle = Console.ReadLine();
                    var song = playlistManager.SearchSong(searchTitle);
                    Console.WriteLine(song != null ? $" Found: {song}" : " Song not found.");
                    break;
                case "8":
                    await playbackService.PlayAsync();
                    break;
                case "9":
                    await playbackService.Skip();
                    break;
                case "10":
                    playbackService.SetShuffleMode(true); 
                    break;
                case "11":
                    playbackService.SetRepeatMode(true); 
                    break;
                case "12":
                    statisticsService.ShowMostPlayedSongs();
                    break;
                case "13":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine(" Invalid choice.");
                    break;
            }

            DisplayMenu();
        }
    }
}
