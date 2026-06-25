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

        if (string.IsNullOrEmpty(url))
        {
            MessageBox.Show("请输入视频URL", "错误");
            return;
        }

        if (string.IsNullOrEmpty(fileName))
            fileName = "%(title)s";   // 默认使用视频标题

        if (!fileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase) &&
            !fileName.Contains("%(ext)s"))
            fileName += ".%(ext)s";

        // 获取 yt-dlp.exe 路径
        var ytDlpPath = Path.Combine(AppContext.BaseDirectory, "Assets", "yt-dlp.exe");
        if (!File.Exists(ytDlpPath))
        {
            MessageBox.Show($"找不到 yt-dlp.exe: {ytDlpPath}", "错误");
            return;
        }

        // 修改为桌面路径
        var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var outputPath = Path.Combine(desktop, fileName);

        // 拼接完整命令
        var command = $"\"{ytDlpPath}\" \"{url}\" -o \"{outputPath}\" --restrict-filenames";
        CommandTextBox.Text = command;

        StatusText.Text = "命令已生成 → 请复制或直接执行下载...";

        // 执行下载
        var psi = new ProcessStartInfo
        {
            FileName = ytDlpPath,
            Arguments = $"\"{url}\" -o \"{outputPath}\" --restrict-filenames",
            UseShellExecute = false,
            CreateNoWindow = false
        };

        try
        {
            Process.Start(psi);
            StatusText.Text += "\n下载进程已启动...";
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "启动失败");
        }
    }
}