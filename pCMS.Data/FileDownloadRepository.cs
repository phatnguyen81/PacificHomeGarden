using pCMS.Core;

namespace pCMS.Data
{
    public interface IFileDownloadRepository : IRepository<FileDownload>
    {
    }
    public class FileDownloadRepository : EfRepository<FileDownload>, IFileDownloadRepository
    {
        public FileDownloadRepository(pCMSEntities context) : base(context) { }
    }
  
}