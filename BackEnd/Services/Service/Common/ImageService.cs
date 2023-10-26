using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Minio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Service.Common
{
    public static class ImageService
    {
        //public static IConfiguration _configuration;
        //public static void Initialize(IConfiguration Configuration)
        //{
        //    _configuration = Configuration;
        //}
        public static async Task<string> AddImage(IFormFile file, IConfiguration _configuration)
        {
            CommonService CS = new CommonService(_configuration);
            MinioClient minio = CS.CreateMinio();
            try
            {
                var bucketName = _configuration["MinIO:BucketName"];
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);
                    await minio.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                using (var data = file.OpenReadStream())
                {
                    var objectName = file.FileName + '_' + DateTime.Now.ToString();
                    var contentType = file.ContentType;
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(bucketName)
                        .WithObject(objectName)
                        .WithStreamData(data)
                        .WithObjectSize(data.Length)
                        .WithContentType(contentType);
                    await minio.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
                    return objectName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public static async Task<bool> DeleteImage(string imageName, IConfiguration _configuration)
        {
            CommonService CS = new CommonService(_configuration);
            MinioClient minio = CS.CreateMinio();
            try
            {
                var bucketName = _configuration["MinIO:BucketName"];
                var beArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);
                bool found = await minio.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (found)
                {
                    var rmArgs = new RemoveObjectArgs()
                                  .WithBucket(bucketName)
                                  .WithObject(imageName);
                    await minio.RemoveObjectAsync(rmArgs).ConfigureAwait(false);
                    return true;
                }
                else
                {
                    return false;
                }
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
    }
}
