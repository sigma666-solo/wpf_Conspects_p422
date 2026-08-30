using Core.Model;
using Core.ModelResponce;
using ShopApp.Data;
using Lesson2.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Lesson2.Services;

public class UpdateServices(ApplicationContext applicationContext) : IServices
{
    public async Task<Responce> ExecuteAsync(string requestBody)
    {
        var requestProduct = JsonSerializer.Deserialize<Product>(requestBody);
        var updatingProduct = await applicationContext.Products.FirstOrDefaultAsync(product => product.Id == requestProduct.Id);

        if (updatingProduct == null)
        {
            return new Responce
            {
                TypeResponse = TypeResponse.Update,
                Body = "Продукт с данным Id не найден"
            };
        }

        updatingProduct.Name = requestProduct.Name;
        updatingProduct.Description = requestProduct.Description;

        await applicationContext.SaveChangesAsync();

        var responce = new Responce
        {
            TypeResponse = TypeResponse.Update,
            Body = "Продукт успешно обновлён"
        };

        return responce;
    }
}
