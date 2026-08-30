using Core.Model;
using Core.ModelResponce;
using ShopApp.Data;
using Lesson2.Services.Interfaces;
using System.Text.Json;

namespace Lesson2.Services;

public class CreateServices(ApplicationContext applicationContext) : IServices
{
    public async Task<Responce> ExecuteAsync(string requestBody)
    {
        var product = JsonSerializer.Deserialize<Product>(requestBody);

        await applicationContext.AddAsync(product);
        await applicationContext.SaveChangesAsync();

        var responce = new Responce
        {
            TypeResponse = TypeResponse.Create,
            Body = "Продукт успешно создан"
        };

        return responce;
    }
}
