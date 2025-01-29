namespace ItaasSolution.Api.Domain.Entities
{
    public class FileLog
    {
        private long _id { get; set; }
        private string _contentTextFileLogMinhaCdn { get; set; }
        private string _contentTextFileLogAgora { get; set; }

        public long Id { get => _id; set => _id = value; }
        public string ContentTextFileLogMinhaCdn { get => _contentTextFileLogMinhaCdn; set => _contentTextFileLogMinhaCdn = value; }
        public string ContentTextFileLogAgora { get => _contentTextFileLogAgora; set => _contentTextFileLogAgora = value; }

        public FileLog(long id, string contentTextFileLogMinhaCdn, string contentTextFileLogAgora)
        {
            this._id = id;
            this._contentTextFileLogMinhaCdn = contentTextFileLogMinhaCdn;
            this._contentTextFileLogAgora = contentTextFileLogAgora;
        }
    }
}
