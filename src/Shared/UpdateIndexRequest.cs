namespace UnicodeBlazor.Shared
{
    public class UpdateIndexResponse
    {
        public UpdateIndexResponse()
        {
        }

        public string Version { get; set; }
        public int EntryCount { get; set; }
        public string Message { get; set; }
        public int BlockCount { get; set; }
    }

    public class UpdateIndexRequest
    {
        public UpdateIndexRequest()
        {
        }

        public string Version { get; set; }
    }
}
