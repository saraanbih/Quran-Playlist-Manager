using System;
using System.Collections.Generic;
using System.Linq;

namespace Quran_Playlist_Manager
{
    public class StatisticsService
    {
        private readonly PlaybackService _playbackService;
        private readonly Dictionary<string, int> _playCounts;

        public StatisticsService(PlaybackService playbackService)
        {
            _playbackService = playbackService ?? throw new ArgumentNullException(nameof(playbackService));
            _playCounts = new Dictionary<string, int>();
            _playbackService.OnSongChanged += UpdatePlayCount;
            InitializePlayCounts();
        }

        // Initialize play counts from existing songs
        private void InitializePlayCounts()
        {
            var playlists = new PlaylistManager().Playlists.Values.SelectMany(p => p.Songs);
            foreach (var song in playlists)
            {
                _playCounts[song.Title] = song.PlayCount;
            }
        }

        // Update play count when a song plays
        private void UpdatePlayCount(Song song)
        {
            if (song == null) return;
            _playCounts[song.Title] = _playCounts.GetValueOrDefault(song.Title, 0) + 1;
            song.IncrementPlayCount();
        }

        // Show the most played songs
        public void ShowMostPlayedSongs(int topCount = 5)
        {
            var sortedSongs = _playCounts
                .OrderByDescending(kv => kv.Value)
                .Take(topCount)
                .Select(kv => new { Title = kv.Key, PlayCount = kv.Value })
                .ToList();

            if (!sortedSongs.Any())
            {
                Console.WriteLine("No play statistics available.");
                return;
            }

            Console.WriteLine($"Top {topCount} Most Played Songs:");
            foreach (var item in sortedSongs)
            {
                Console.WriteLine($"- {item.Title}: Played {item.PlayCount} times");
            }
        }

        // Auto-remove overplayed songs from a playlist
        public void AutoRemoveOverplayedSongs(Playlist playlist, int maxPlayCount)
        {
            if (playlist == null) throw new ArgumentNullException(nameof(playlist));
            var overplayed = playlist.Songs.Where(s => _playCounts.GetValueOrDefault(s.Title, 0) > maxPlayCount).ToList();
            foreach (var song in overplayed)
            {
                playlist.RemoveSong(song);
                _playCounts.Remove(song.Title);
            }
            Console.WriteLine($"{overplayed.Count} overplayed songs removed from {playlist.Name}.");
        }
    }
}