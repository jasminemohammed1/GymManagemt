using GymManagement.BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Attachments
{
    public interface IAttachmentService
    {
        Task<Result<string> ?> UploudAsync(Stream stream, string fileName, string FolderName, CancellationToken ct);
        Result Delete(string fileName, string FolderName);
        Result<(Stream stream , string ContentType)>? GetFile(string fileName , string folderName);
    }
}
