namespace Quran_Playlist_Manager
{
    public class PlaybackService
    {
        private readonly Playlist _playlist;
        private Queue<Song> _playbackQueue;
        private Song _currentSong;
        private bool _isPlaying;
        private bool _isPaused;
        private bool _shuffleMode;
        private bool _repeatMode;

        public delegate void SongChangedHandler(Song song);
        public event SongChangedHandler OnSongChanged;

        public PlaybackService(Playlist playlist)
        {
            _playlist = playlist ?? throw new ArgumentNullException(nameof(playlist), "Playlist cannot be null.");
            _playbackQueue = new Queue<Song>();
            _currentSong = null;
            _isPlaying = false;
            _isPaused = false;
            _shuffleMode = false;
            _repeatMode = false;
        }

        public void InitialQueue()
        {
            var songs = _shuffleMode
                ? _playlist.Songs.OrderBy(s => Guid.NewGuid()).ToList()
                : _playlist.Songs;

            _playbackQueue = new Queue<Song>(songs);
        }

        public void SetShuffleMode(bool enabled)
        {
            _shuffleMode = enabled;
            InitialQueue();
            Console.WriteLine($"🔀 Shuffle Mode: {(_shuffleMode ? "Enabled" : "Disabled")}");
        }

        public void SetRepeatMode(bool enabled)
        {
            _repeatMode = enabled;
            Console.WriteLine($"🔁 Repeat Mode: {(_repeatMode ? "Enabled" : "Disabled")}");
        }

        public async Task PlayAsync()
        {
            if (!_playbackQueue.Any())
            {
                if (_repeatMode)
                {
                    InitialQueue();
                }
                else
                {
                    Console.WriteLine("❌ No songs in queue.");
                    _isPlaying = false;
                    _currentSong = null;
                    return;
                }
            }

            _currentSong = _playbackQueue.Dequeue();
            _isPlaying = true;
            _isPaused = false;
            OnSongChanged?.Invoke(_currentSong);
            await _currentSong.PlayAsync();
            _isPlaying = false;

            if (_repeatMode || _playbackQueue.Any())
                await PlayAsync();
        }

        public async Task PlaySongAsync(Song song)
        {
            if (song == null) throw new ArgumentException("❌ Song cannot be null.");
            _playbackQueue.Clear();
            _playbackQueue.Enqueue(song);
            await PlayAsync();
        }

        public void Pause()
        {
            if (!_isPlaying)
            {
                Console.WriteLine("❌ No song is playing.");
                return;
            }
            if (_isPaused)
            {
                Console.WriteLine("⏸️ Song is already paused.");
                return;
            }
            _isPaused = true;
            _currentSong.Pause();
        }

        public void Resume()
        {
            if (!_isPlaying)
            {
                Console.WriteLine("❌ No song is playing.");
                return;
            }
            if (!_isPaused)
            {
                Console.WriteLine("▶️ Song isn't paused.");
                return;
            }
            _isPaused = false;
            _currentSong.Resume();
        }

        public void Stop()
        {
            if (!_isPlaying)
            {
                Console.WriteLine("❌ No song is currently playing.");
                return;
            }
            _currentSong.Stop();
            _isPlaying = false;
            _isPaused = false;
            _currentSong = null;
            _playbackQueue.Clear();
        }

        public async Task Skip()
        {
            if (!_isPlaying || !_playbackQueue.Any()) return;
            Stop();
            await PlayAsync();
        }

        public async Task Previous()
        {
            if (!_isPlaying) return;
            Stop();
            _playbackQueue = new Queue<Song>(_playlist.Songs);
            await PlayAsync();
        }
    }
}
