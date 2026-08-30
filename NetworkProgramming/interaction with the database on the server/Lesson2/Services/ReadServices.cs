using Core.Model;
using Core.ModelResponce;
using ShopApp.Data;
using Lesson2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lesson2.Services;

public class ReadServices(ApplicationContext applicationContext) : IServices
{
    public async Task<Responce> ExecuteAsync(string requestBody)
    {
        var product = await applicationContext.Products.AsNoTracking().ToListAsync();

        var responce = new Responce
        {
            TypeResponse = TypeResponse.Read,
            Body = JsonSerializer.Serialize(product)
        };

        return responce;
    }
}