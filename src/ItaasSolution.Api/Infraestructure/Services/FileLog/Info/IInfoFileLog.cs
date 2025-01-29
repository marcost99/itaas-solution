namespace ItaasSolution.Api.Infraestructure.Services.FileLog.Info
{
    public interface IInfoFileLog
    {
        string GetUrlHost();
        string GetUrlPath(string tag);
        string GetFileNameBase();
        string GetFileNameFull(long idFileLog);
        string GetRegexFileName();
        string GetRegexFileNameId();
        long GetFileId(string path);
        string GetPhysicalPath(string tag);
    }
}
