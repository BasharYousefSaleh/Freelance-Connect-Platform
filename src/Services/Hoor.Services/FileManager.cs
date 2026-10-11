//namespace Hoor.Services
//{
//    using System.IO;
//    using System.Linq;
//    using System.Threading.Tasks;

//    using CloudinaryDotNet;
//    using CloudinaryDotNet.Actions;
//    using Hoor.Common;
//    using Hoor.Data.Common.Repositories;
//    using Hoor.Data.Models;
//    using Hoor.Services.Interfaces;
//    using Microsoft.AspNetCore.Http;

//    public class FileManager : IFileManager
//    {
//        private readonly IRepository<Attachment> attachmentRepository;

//        public FileManager(IRepository<Attachment> attachmentRepository)
//        {
//            this.attachmentRepository = attachmentRepository;
//        }

//        public async Task AddFileToContractAsync(IFormFile file, string contractId)
//        {
//            var attachmentUrl = await this.UploadAttachmentAsync(file);

//            var attachment = new Attachment
//            {
//                Url = attachmentUrl,
//                ContractId = contractId,
//                Name = file.FileName.Split(".").First().ToUpper(),
//                Extension = file.FileName.Split(".").Last().ToUpper(),
//            };

//            await this.attachmentRepository.AddAsync(attachment);
//            await this.attachmentRepository.SaveChangesAsync();
//        }

//        public async Task<string> UploadAttachmentAsync(IFormFile attachment)
//        {
//            Account account = new Account(
//                GlobalConstants.SystemName.ToLower(),
//                GlobalConstants.CloudinaryApiKey,
//                GlobalConstants.CloudinaryApiSecretKey);

//            Cloudinary cloudinary = new Cloudinary(account);
//            cloudinary.Api.Secure = true;

//            using (var stream = new MemoryStream())
//            {
//                stream.Flush();
//                await attachment.CopyToAsync(stream);
//                stream.Position = 0;

//                var uploadParams = new ImageUploadParams()
//                {
//                    File = new FileDescription(attachment.FileName.Split(".").First(), stream),
//                };

//                var uploadResult = cloudinary.Upload(uploadParams);

//                var downloadableUrl = "https://res.cloudinary.com/jobzy/image/upload/h_300,w_300/fl_attachment/v" +
//                                      $"{uploadResult.Version}/{uploadResult.PublicId}.{uploadResult.Format}";

//                return downloadableUrl;
//            }
//        }
//    }
//}


using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hoor.Common;
using Hoor.Data.Common.Repositories;
using Hoor.Data.Models;
using Hoor.Services.Interfaces;
using Hoor.Web.ViewModels.Jobs;
using Microsoft.AspNetCore.Http;

namespace Hoor.Services
{
    public class FileManager : IFileManager
    {
        //private readonly IFreelancePlatform freelancePlatform;
        private readonly IRepository<Attachment> attachmentRepository;

        public FileManager(IRepository<Attachment> attachmentRepository)
        {
           
            this.attachmentRepository = attachmentRepository;
        }

        public async Task AddFileToContractAsync(IFormFile file, string contractId)
        {
            var attachmentUrl = await this.UploadAttachmentAsync(file);

            var attachment = new Attachment
            {
                Url = attachmentUrl,
                ContractId = contractId,
                Name = file.FileName.Split(".").First().ToUpper(),
                Extension = file.FileName.Split(".").Last().ToUpper(),
            };

            await this.attachmentRepository.AddAsync(attachment);
            await this.attachmentRepository.SaveChangesAsync();
        }

        public async Task AddFileToJobAsync(IFormFile file, string jobid)
        {
            var attachmentUrl = await this.UploadAttachmentAsync(file);
            //var job = await this.freelancePlatform.JobManager.GetJobByIdAsync<SingleJobViewModel>(jobid);
            
           

            await this.attachmentRepository.SaveChangesAsync();
        }

        public async Task<string> UploadAttachmentAsync(IFormFile attachment )
        {
            // تحديد مسار المجلد حيث سيتم تخزين الملفات
            var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profil");

            // التأكد من أن المجلد موجود، وإذا لم يكن كذلك، إنشاؤه
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            // إنشاء اسم فريد للملف لتجنب التعارض
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(attachment.FileName)}_{Guid.NewGuid()}{Path.GetExtension(attachment.FileName)}";

            // المسار الكامل للملف داخل المجلد
            var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

            // حفظ الملف في المسار المحدد
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            // إعادة رابط الملف النسبي
            var downloadableUrl = $"/uploads/{uniqueFileName}";

            return downloadableUrl;
        }

        public async Task<string> CarduploadAttachmentAsync(IFormFile attachment,string nameImage)
        {
            // تحديد مسار المجلد حيث سيتم تخزين الملفات
            var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Cards");

            // التأكد من أن المجلد موجود، وإذا لم يكن كذلك، إنشاؤه
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }


            // إنشاء اسم فريد للملف لتجنب التعارض
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(attachment.FileName)}_{nameImage}{Path.GetExtension(attachment.FileName)}";

            // المسار الكامل للملف داخل المجلد
            var filePath = Path.Combine(uploadFolderPath, uniqueFileName);


            // التحقق إذا كان الملف موجودًا بالفعل
            if (File.Exists(filePath))
            {
                // حذف الملف الحالي إذا كان موجودًا
                File.Delete(filePath);
            }


            // حفظ الملف في المسار المحدد
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            // إعادة رابط الملف النسبي
            var downloadableUrl = $"/Cards/{uniqueFileName}";

            return downloadableUrl;
        }

        public async Task<string> profileuploadAttachmentAsync(IFormFile attachment, string nameImage)
        {
            // تحديد مسار المجلد حيث سيتم تخزين الملفات
            var uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "profil");

            // التأكد من أن المجلد موجود، وإذا لم يكن كذلك، إنشاؤه
            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }


            // إنشاء اسم فريد للملف لتجنب التعارض
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(attachment.FileName)}_{nameImage}{Path.GetExtension(attachment.FileName)}";

            // المسار الكامل للملف داخل المجلد
            var filePath = Path.Combine(uploadFolderPath, uniqueFileName);


            // التحقق إذا كان الملف موجودًا بالفعل
            if (File.Exists(filePath))
            {
                // حذف الملف الحالي إذا كان موجودًا
                File.Delete(filePath);
            }


            // حفظ الملف في المسار المحدد
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await attachment.CopyToAsync(stream);
            }

            // إعادة رابط الملف النسبي
            var downloadableUrl = $"/Cards/{uniqueFileName}";

            return downloadableUrl;
        }
    }
}
