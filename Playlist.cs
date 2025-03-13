using System;
using System.Collections.Generic;
using System.Linq;

namespace Quran_Playlist_Manager
{
    public class Playlist
    {
        public string Name { get; private set; }
        public List<Song> Songs;
        private HashSet<string> _songTitles;

        public Playlist(string name)
        {
            Name = name ?? throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            Songs = new List<Song>();
            _songTitles = new HashSet<string>();
        }

        // Overload 1: Add a Song object
        public void AddSong(Song song)
        {
            if (song == null) throw new ArgumentNullException(nameof(song), "Song cannot be null.");
            if (_songTitles.Contains(song.Title)) throw new InvalidOperationException($"Song '{song.Title}' already exists.");
            Songs.Add(song);
            _songTitles.Add(song.Title);
        }

        // Overload 2: Add by title only (default artist and duration)
        public void AddSong(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            if (_songTitles.Contains(title)) throw new InvalidOperationException($"Song '{title}' already exists.");
            var song = new Song(title, "Unknown", TimeSpan.FromSeconds(30));
            Songs.Add(song);
            _songTitles.Add(song.Title);
        }

        // Overload 3: Add by title and artist (default duration)
        public void AddSong(string title, string artist)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(artist)) throw new ArgumentException("Artist cannot be null or empty.", nameof(artist));
            if (_songTitles.Contains(title)) throw new InvalidOperationException($"Song '{title}' already exists.");
            var song = new Song(title, artist, TimeSpan.FromSeconds(30));
            Songs.Add(song);
            _songTitles.Add(song.Title);
        }

        // Overload 4: Add by title, artist, and duration
        public void AddSong(string title, string artist, TimeSpan duration)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(artist)) 
                throw new ArgumentException("Artist cannot be null or empty.", nameof(artist));
            if (duration.TotalSeconds < 0)
                throw new ArgumentException("Duration cannot be negative.", nameof(duration));
            if (_songTitles.Contains(title)) 
                throw new InvalidOperationException($"Song '{title}' already exists.");
            var song = new Song(title, artist, duration);
            Songs.Add(song);
            _songTitles.Add(song.Title);
        }

        // Overload 1: Remove by Song object
        public bool RemoveSong(Song song)
        {
            if (song == null) return false;
            bool removed = Songs.Remove(song);
            if (removed) _songTitles.Remove(song.Title);
            return removed;
        }

        // Overload 2: Remove by title
        public bool RemoveSong(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return false;
            var song = Songs.FirstOrDefault(s => s.Title == title);
            return song != null && RemoveSong(song);
        }

        // Overload 3: Remove by title and artist (in case titles aren’t unique)
        public bool RemoveSong(string title, string artist)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(artist)) return false;
            var song = Songs.FirstOrDefault(s => s.Title == title && s.Artist == artist);
            return song != null && RemoveSong(song);
        }

        // Overload 1: Search by title
        public Song SearchSong(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return null;
            return Songs.FirstOrDefault(s => s.Title == title);
        }

        // Overload 2: Search by title and artist
        public Song SearchSong(string title, string artist)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(artist)) return null;
            return Songs.FirstOrDefault(s => s.Title == title && s.Artist == artist);
        }

        // Overload 1: View all songs
        public void ViewSongs()
        {
            if (!Songs.Any()) Console.WriteLine("No songs in playlist.");
            else foreach (var song in Songs) Console.WriteLine(song.ToString());
        }

        // Overload 2: View songs by artist
        public void ViewSongs(string artist)
        {
            if (string.IsNullOrWhiteSpace(artist))
            {
                Console.WriteLine("Artist cannot be empty.");
                return;
            }
            var artistSongs = Songs.Where(s => s.Artist == artist).ToList();
            if (!artistSongs.Any()) Console.WriteLine($"No songs by {artist} in playlist.");
            else foreach (var song in artistSongs) Console.WriteLine(song.ToString());
        }

        // Overload 1: Auto-remove by max play count
        public void AutoRemoveOverplayedSongs(int maxPlayCount)
        {
            var toRemove = Songs.Where(s => s.PlayCount > maxPlayCount).ToList();
            foreach (var song in toRemove)
            {
                Songs.Remove(song);
                _songTitles.Remove(song.Title);
            }
        }

        // Overload 2: Auto-remove by max play count with artist filter
        public void AutoRemoveOverplayedSongs(int maxPlayCount, string artist)
        {
            if (string.IsNullOrWhiteSpace(artist)) return;
            var toRemove = Songs.Where(s => s.PlayCount > maxPlayCount && s.Artist == artist).ToList();
            foreach (var song in toRemove)
            {
                Songs.Remove(song);
                _songTitles.Remove(song.Title);
            }
        }

    }
}