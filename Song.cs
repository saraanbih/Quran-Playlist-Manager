namespace Quran_Playlist_Manager
{
    public interface IPlayable
    {
        Task PlayAsync();
        void Pause();
        void Resume();
        void Stop();
    }

    public class Song : IPlayable, IEquatable<Song>
    {
        private string _title;
        private string _artist;
        private int _playCount;

        public string Title
        {
            get => _title;
            set => _title = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Title cannot be empty.") : value.Trim();
        }

        public string Artist
        {
            get => _artist;
            set => _artist = string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("Artist cannot be empty.") : value.Trim();
        }

        public TimeSpan Duration
        {
            get => _duration;
            set => _duration = value.TotalMilliseconds < 0 ? throw new ArgumentException("Duration cannot be negative.") : value;
        }
        private TimeSpan _duration;

        public int PlayCount
        {
            get => _playCount;
            private set => _playCount = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value), "Play count cannot be negative.");
        }

        public DateTime DateAdded { get; } = DateTime.Now;

        public Song(string title, string artist, TimeSpan duration)
        {
            Title = title;
            Artist = artist;
            Duration = duration;
            PlayCount = 0;
        }

        public async Task PlayAsync()
        {
            Console.WriteLine($"Playing {ToString()}");
            await Task.Delay(Duration); 
            IncrementPlayCount();
        }

        public void Pause() => Console.WriteLine($"Paused {Title}");
        public void Resume() => Console.WriteLine($"Resumed {Title}");
        public void Stop() => Console.WriteLine($"Stopped {Title}");

        public override string ToString() => $"Title: {Title}, Artist: {Artist}, Duration: {Duration}, PlayCount: {PlayCount}, Added: {DateAdded}";

        public bool Equals(Song? other) => other != null && Title == other.Title; 
        public override int GetHashCode() => Title?.GetHashCode() ?? 0;
        public void IncrementPlayCount() => _playCount++;


    }
}