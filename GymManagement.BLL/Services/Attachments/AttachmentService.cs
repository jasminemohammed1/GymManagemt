using GymManagement.BLL.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] _allowedExtensions = [".png",".jpg",".jpeg"];
        private readonly long _maxlength = 5 * 1024 * 1024;
        private readonly ILogger<AttachmentService> logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AttachmentService(ILogger<AttachmentService> logger ,IWebHostEnvironment webHostEnvironment)
        {
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        public Result Delete(string fileName, string FolderName)
        {
          var fullpath = Path.Combine(webHostEnvironment.ContentRootPath , FolderName , fileName);
           
            try
            {
                if (!File.Exists(fullpath))
                {
                   
                    Result.NotFound();

                }
                File.Delete(fullpath);
                return Result.Ok();
            }catch(Exception ex)
            {
                logger.LogError($"Fail to delete {fullpath}");
                return Result.Fail("Fail to delete the file");
            }
        }

        public Result<(Stream stream, string ContentType)>? GetFile(string fileName, string folderName)
        {
            if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
            {
                return null;
            }
            var fullpath = Path.Combine(webHostEnvironment.ContentRootPath, folderName, fileName);
            if (!File.Exists(fullpath)) return null;
            var stream = new  FileStream(fullpath, FileMode.Open , FileAccess.Read);
            var extension = Path.GetExtension(fullpath);
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "application/octet-stream"

            };
            return Result<(Stream stream, string ContentType)>.Ok((stream, contentType));
        }

        public async Task<Result<string> ?> UploudAsync(Stream stream, string fileName, string FolderName, CancellationToken ct)
        {
            if (stream == null || !stream.CanRead) return null;
            if (stream.Length == 0) return null;
            if (stream.Length > _maxlength)
            {
                logger.LogError($"File Stream > max size: {stream.Length}");
                return Result<string>.Validation("Not allowed size"); }

            var extension = Path.GetExtension(fileName);
            if(string.IsNullOrEmpty(extension) ||  ! _allowedExtensions.Contains(extension))
            {
                logger.LogError($"File Extension not from allowed ones : {extension}");
                return Result<string>.Validation("Not Valid Extension");
            }

            var uplouadfolder = Path.Combine(webHostEnvironment.ContentRootPath, FolderName);
            Directory.CreateDirectory(uplouadfolder);

            var storedName = $"{Guid.NewGuid()}{fileName}";
            var filepath = Path.Combine(uplouadfolder, storedName);

          
            try
            {
                using var str = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                await stream.CopyToAsync(str);
                return Result<string>.Ok(storedName);
            }
            catch( Exception ex )
            {
                logger.LogError(ex, $"Failed to upload the file {fileName}");
                return null;
            }



            



        }
    }
}
