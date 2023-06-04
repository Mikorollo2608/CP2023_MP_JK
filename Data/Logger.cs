using System.Text;

namespace Data
{
    internal class Logger : LoggerApi
    {
        private DateTime logStart;
        private FileStream file;
        private String fileName;
        private List<LogEntry> logEntries = new List<LogEntry>();

        public Logger(String fileName)
        {
            this.fileName = fileName;
            file = File.Create(fileName);
            file.Close();
            logStart = DateTime.Now;
            Task.Run(() => { log(); });
        }

        override public void addToQueue(DateTime time, int ball1, int ball2)
        {
            logEntries.Add(new LogEntry(time, ball1, ball2));
        }

        private async void log()
        {
            while (true)
            {
                if (logEntries.Count > 0)
                {
                    LogEntry entry = logEntries.First();
                    StringBuilder message = new StringBuilder();
                    message.Append(entry.collisionTime.Subtract(logStart).ToString("mmssfff"));
                    message.Insert(2, ":");
                    message.Insert(5, ":");
                    message.Append(" - Ball ");
                    message.Append(entry.ball1Index);
                    message.Append(", Ball ");
                    message.Append(entry.ball2Index);
                    message.Append("\n");
                    file = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
                    byte[] buffer = Encoding.UTF8.GetBytes(message.ToString());
                    await file.WriteAsync(buffer, 0, buffer.Length);
                    file.Close();
                    logEntries.RemoveAt(0);
                }
            }
        }
    }
}

internal struct LogEntry
{
    public LogEntry(DateTime time, int ball1, int ball2)
    {
        collisionTime = time;
        ball1Index = ball1;
        ball2Index = ball2;
    }
    public DateTime collisionTime { get; }
    public int ball1Index { get; }
    public int ball2Index { get; }
}
