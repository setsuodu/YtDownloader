using System.Diagnostics;
using System.IO;
using System.Windows;

namespace YtDownloader.Wpf;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        var url = UrlTextBox.Text.Trim();
        var fileName = FileNameTextBox.Text.Trim();

        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(fileName))
        {
            MessageBox.Show("填完整", "错误");
            return;
        }

        if (!fileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
            fileName += ".mp4";

        // 关键：获取实际运行目录
        var ytDlpPath = Path.Combine(AppContext.BaseDirectory, "Assets", "yt-dlp.exe");

        if (!File.Exists(ytDlpPath))
        {
            MessageBox.Show($"找不到yt-dlp: {ytDlpPath}");
            return;
        }

        var downloads = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Downloads");
        var output = Path.Combine(downloads, fileName);

        // 直接启动，不用cmd中转
        var psi = new ProcessStartInfo
        {
            FileName = ytDlpPath,
            Arguments = $"\"{url}\" -o \"{output}\"",
            UseShellExecute = false,
            CreateNoWindow = false
        };

        try { Process.Start(psi); }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }
}