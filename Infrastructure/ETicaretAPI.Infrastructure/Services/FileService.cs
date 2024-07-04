using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (to be implemented)
                throw;
            }
        }

        private async Task<string> FileRenameAsync(string path, string fileName, bool first = true)
        {
            return await Task.Run(() =>
            {
                string extension = Path.GetExtension(fileName);
                string baseFileName = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = first ? $"{NameOperation.CharacterRegulatory(baseFileName)}{extension}" : fileName;

                while (File.Exists(Path.Combine(path, newFileName)))
                {
                    int dashIndex = newFileName.LastIndexOf('-');
                    int dotIndex = newFileName.LastIndexOf('.');

                    if (dashIndex == -1 || dashIndex > dotIndex)
                    {
                        newFileName = $"{baseFileName}-2{extension}";
                    }
                    else
                    {
                        string numberPart = newFileName.Substring(dashIndex + 1, dotIndex - dashIndex - 1);
                        if (int.TryParse(numberPart, out int fileNumber))
                        {
                            newFileName = $"{baseFileName}-{fileNumber + 1}{extension}";
                        }
                        else
                        {
                            newFileName = $"{baseFileName}-2{extension}";
                        }
                    }
                }

                return newFileName;
            });
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            List<bool> results = new();

            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(uploadPath, file.FileName);
                bool result = await CopyFileAsync(Path.Combine(uploadPath, fileNewName), file);
                datas.Add((fileNewName, Path.Combine(uploadPath, fileNewName)));
                results.Add(result);
            }

            if (results.TrueForAll(x => x))
            {
                return datas;
            }

            return null;
        }
    }
}
