using Core.ModelResponce;

namespace Lesson2.Services.Interfaces;

public interface IServices
{
    Task<Responce> ExecuteAsync(string requestBody);
}
