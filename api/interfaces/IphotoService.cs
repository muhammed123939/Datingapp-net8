using System;
using CloudinaryDotNet.Actions;

namespace api.interfaces;

public interface IphotoService
{
 Task <ImageUploadResult> AddPhotoAsync (IFormFile file) ;
 Task <DeletionResult> DeletePhotoAsync (string PublicId);
 
}
