using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektGit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> playlistItems = new List<string>();
        private int currentTrackIndex = 0;
        private bool isPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlayPauseButtonClick(object sender, RoutedEventArgs e)
        {
            if (playlistItems.Count > 0)
            {
                if (isPlaying)
                {
                    mediaElement.Pause();
                }
                else
                {
                    if (mediaElement.Source == null || mediaElement.Position == mediaElement.NaturalDuration.TimeSpan)
                    {
                        PlayCurrentTrack();
                    }
                    else
                    {
                        mediaElement.Play();
                    }
                }

                isPlaying = !isPlaying;
            }
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentTrackIndex < playlistItems.Count - 1)
            {
                currentTrackIndex++;
                PlayCurrentTrack();
            }
        }

        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            if (currentTrackIndex > 0)
            {
                currentTrackIndex--;
                PlayCurrentTrack();
            }
        }

        private void PlayCurrentTrack()
        {
            mediaElement.Source = new Uri(playlistItems[currentTrackIndex]);
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.Play();
            isPlaying = true;
        }

        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += UpdateTimeDisplay;
            progressSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering -= UpdateTimeDisplay;
            NextButtonClick(null, null);
        }

        private void AddToPlaylist(string filePath)
        {
            playlistItems.Add(filePath);
            playlist.Items.Add(System.IO.Path.GetFileName(filePath));
        }

        private void ProgressSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(e.NewValue);
        }

        private void RewindButtonClick(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position.Subtract(TimeSpan.FromSeconds(10));
            UpdateTimeDisplay(null, null);
        }

        private void FastForwardButtonClick(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position.Add(TimeSpan.FromSeconds(10));
            UpdateTimeDisplay(null, null);
        }

        private void UpdateTimeDisplay(object sender, EventArgs e)
        {
            progressSlider.Value = mediaElement.Position.TotalSeconds;
        }


        private void OpenFileButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP3 files (*.mp3)|*.mp3|MP4 files (*.mp4)|*.mp4|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string selectedFileName in openFileDialog.FileNames)
                {
                    AddToPlaylist(selectedFileName);
                }
            }
        }

    }
}