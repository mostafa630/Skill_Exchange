using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Skill_Exchange.Application.DTOs;
using Skill_Exchange.Domain.Interfaces;

namespace Skill_Exchange.Application.Services.User.Commands.Handlers
{
    public class UploadImageHandler : IRequestHandler<UploadImage, Result<string>>
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUnitOfWork _unitOfWork;

        public UploadImageHandler(IWebHostEnvironment env, IUnitOfWork unitOfWork)
        {
            _env = env;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(UploadImage request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.image == null || request.image.Length == 0)
                    return Result<string>.Fail("No image uploaded.");

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(request.image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(extension))
                    return Result<string>.Fail("Invalid file type. Only .jpg, .jpeg, .png, .gif allowed.");

                var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "Users");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{request.userId}{extension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                // 🔹 Check if user has old image (e.g. .png but new upload is .jpg)
                var user = await _unitOfWork.Users.GetByIdAsync(request.userId);
                if (user == null)
                    return Result<string>.Fail("User not found.");

                if (!string.IsNullOrEmpty(user.ProfileImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, user.ProfileImageUrl.TrimStart('/'));
                    if (File.Exists(oldPath) && oldPath != filePath)
                        File.Delete(oldPath);
                }

                // 🔹 Save new image (overwrite if exists)
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.image.CopyToAsync(stream, cancellationToken);
                }

                var relativePath = $"/Users/{fileName}";
                user.ProfileImageUrl = relativePath;
                await _unitOfWork.CompleteAsync();

                return Result<string>.Ok(relativePath);
            }
            catch (Exception ex)
            {
                return Result<string>.Fail($"Uploading image failed: {ex.Message}");
            }
        }

    }
}