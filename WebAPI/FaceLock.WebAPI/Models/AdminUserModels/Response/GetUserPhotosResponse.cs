using FaceLock.Domain.Entities.UserAggregate;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Threading.Tasks;

namespace FaceLock.WebAPI.Models.AdminUserModels.Response
{
    public class GetUserPhotosResponse
    {
        public byte[] FileContents { get; set; }
        public string ContentType { get; set;}
        public string FileDownloadName { get; set;}

        public GetUserPhotosResponse(string userId, IEnumerable<UserFace> userFaces)
        {
            FileContents = Make(userId, userFaces);
            ContentType = "application/zip";
            FileDownloadName = $"{userId}_photos.zip";
        }

        private byte[] Make(string userId, IEnumerable<UserFace> userFaces)
        {
            // Create a memory stream to store the byte data for each photo
            var streams = new List<MemoryStream>();
            var archiveStream = new MemoryStream();

            // Create a zip archive and add each memory stream as a new entry
            using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
            {
                foreach (var userFace in userFaces)
                {
                    var i = 0;
                    var stream = new MemoryStream(userFace.ImageData);
                    streams.Add(stream);

                    stream = streams[i];
                    var entryName = $"photo_{userFace.Id}.{userFace.ImageMimeType.Replace("/", ".")}";
                    var entry = archive.CreateEntry(entryName);

                    using (var entryStream = entry.Open())
                    {
                        stream.CopyTo(entryStream);
                    }

                    i++;
                }
            }

            // Reset the memory streams so they can be read again
            foreach (var stream in streams)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }

            return archiveStream.ToArray();
        }
    }
}
