namespace Quran_Playlist_Manager
{
    public class PlaylistManager
    {
        public readonly Dictionary<string, Playlist> Playlists = new Dictionary<string, Playlist>();
        public Playlist CurrentPlaylist;

        public PlaylistManager()
        {
            Playlists = new Dictionary<string, Playlist>();
            CurrentPlaylist = null;
        }

        public void CreatePlaylist(string playlistName)
        {
            if (string.IsNullOrWhiteSpace(playlistName))
                throw new ArgumentException("Playlist name cannot be empty.");

            if (Playlists.ContainsKey(playlistName))  
                throw new InvalidOperationException($"Playlist '{playlistName}' already exists.");

            Playlists[playlistName] = new Playlist(playlistName);
            if (CurrentPlaylist == null) CurrentPlaylist = Playlists[playlistName];
        }

        public void SwitchPlaylist(string playlistName)
        {
            if (!Playlists.TryGetValue(playlistName, out Playlist playlist))
                throw new KeyNotFoundException($"{playlistName} isn't found in all playlists");
            CurrentPlaylist = playlist;
        }

        public void AddSong(Song song) { if (EnsureCurrentPlaylist()) CurrentPlaylist.AddSong(song); }
        public void AddSong(string title) { if (EnsureCurrentPlaylist()) CurrentPlaylist.AddSong(title); }
        public void AddSong(string title,string artist) { if(EnsureCurrentPlaylist()) CurrentPlaylist.AddSong(title,artist); }
        public void AddSong(string title,string artist,TimeSpan duration) { if(EnsureCurrentPlaylist()) CurrentPlaylist.AddSong(title, artist,duration); }


        public void RemoveSong(Song song) { if (EnsureCurrentPlaylist()) CurrentPlaylist.RemoveSong(song); }
        public void RemoveSong(string title) { if (EnsureCurrentPlaylist()) CurrentPlaylist.RemoveSong(title); }
        public void RemoveSong(string title, string artist) { if (EnsureCurrentPlaylist()) CurrentPlaylist.RemoveSong(title, artist); }

        public void ViewSongs() { if (EnsureCurrentPlaylist()) CurrentPlaylist.ViewSongs();}
        public void ShowSongs(string artist) { if (EnsureCurrentPlaylist()) CurrentPlaylist.ViewSongs(artist); }

        public Song SearchSong(string title) => EnsureCurrentPlaylist() ? CurrentPlaylist.SearchSong(title) : null;
        public Song SearchSong(string title,string artist) => EnsureCurrentPlaylist() ? CurrentPlaylist.SearchSong(title, artist) : null;

        public void AutoRemoveOverlapedSongs(int maxPlayCount) { if (EnsureCurrentPlaylist()) CurrentPlaylist.AutoRemoveOverplayedSongs(maxPlayCount); }
        public void AutoRemoveOldSongs(int maxPlayCount,string artist) { if (EnsureCurrentPlaylist()) CurrentPlaylist.AutoRemoveOverplayedSongs(maxPlayCount,artist); }

        public void ViewPlaylists()
        {
           if(!Playlists.Any()) {Console.WriteLine("No playlists found"); return; }
            Console.WriteLine("Available Playlists:");
            foreach (var name in Playlists.Keys)
                Console.WriteLine($"***{name}***");
        }

        public bool EnsureCurrentPlaylist() => CurrentPlaylist != null;

    }
}
