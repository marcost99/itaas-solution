namespace ItaasSolution.Api.Application.Services.FileLog.Info
{
    public interface IInfoFileLog
    {
        string UrlHost();
        string UrlPath(string tag);
        string FileNameBase();
        string FileNameFull(long idFileLog);
        long FileId(string path);
        long NewFileId();
        string PhysicalPath(string tag);
    }
}
