using CommunityToolkit.Maui.Storage;
using System.Text;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;








namespace okiyoutubeindirici
{
    public partial class MainPage : ContentPage
    {
        IFileSaver fileSaver;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public MainPage(IFileSaver fileSaver)
        {

            InitializeComponent();
            this.fileSaver = fileSaver;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Task.Run(async () =>
                {
                
                    var youtubeClient = new YoutubeClient();
                    var video = await youtubeClient.Videos.GetAsync(videoUrl.Text);
                    var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(video.Id, cancellationTokenSource.Token);
                    var streamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                    var stream = await youtubeClient.Videos.Streams.GetAsync(streamInfo, cancellationTokenSource.Token);

                    var fullpath = await fileSaver.SaveAsync($"{video.Title}.mp3", stream, cancellationTokenSource.Token);

                    sonuc.Text = fullpath.FilePath + " Dizinine Kaydedildi";
                  
                });
            }
            catch (Exception ex)
            {
                
            }


        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {

                await Task.Run(async () =>
                {
                    
                        var youtubeClient = new YoutubeClient();
                        var video = await youtubeClient.Videos.GetAsync(videoUrl.Text);
                        var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(video.Id);

                        // En yüksek kaliteli video akışını seç
                        var streamInfo = streamManifest.GetMuxedStreams()
                            .OrderByDescending(s => s.VideoQuality)
                            .ThenByDescending(s => s.Bitrate) // Bit rate'e göre sıralayın
                            .FirstOrDefault();
                        var stream = await youtubeClient.Videos.Streams.GetAsync(streamInfo, cancellationTokenSource.Token);

                        var fullpath = await fileSaver.SaveAsync($"{video.Title}.mp4", stream, cancellationTokenSource.Token);

                        sonuc.Text = fullpath.FilePath + " Dizinine Kaydedildi";
                        await DisplayAlert("Başarılı", "Dosya Yüklendi", "Tamam");
                    

                  




                });
        }
            catch (Exception ex)
            {

                

            }
        }
           

        }
    }


