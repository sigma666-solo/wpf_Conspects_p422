using Core.ModelResponce;
using ShopApp.Data;
using Lesson2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lesson2.Services;

public class DeleteServices(ApplicationContext applicationContext) : IServices
{
    public async Task<Responce> ExecuteAsync(string requestBody)
    {
        var idProduct = JsonSerializer.Deserialize<int>(requestBody);

        var product = await applicationContext.Products.FirstOrDefaultAsync(product => product.Id == idProduct);

        if (product == null)
        {
            return new Responce
            {
                TypeResponse = TypeResponse.Delete,
                Body = "Объект с данным Id не существует в системе"
            };
        }

        applicationContext.Remove(product);
        await applicationContext.SaveChangesAsync();

        var responce = new Responce
        {
            TypeResponse = TypeResponse.Delete,
            Body = "Продукт успешно удалён"
        };

        return responce;
    }
}
