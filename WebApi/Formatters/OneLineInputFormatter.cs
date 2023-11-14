using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WebApi.Models;

namespace WebApi.Formatters
{
    public class OneLineInputFormatter : TextInputFormatter
    {

        public OneLineInputFormatter() {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/oneline"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedEncodings.Add(Encoding.ASCII);
        }

        protected override bool CanReadType(Type type)
        => type == typeof(ShopCreateModel) 
        || type == typeof(ShopEditModel);

        public async override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            var httpContext = context.HttpContext;

            using var reader = new StreamReader(httpContext.Request.Body, encoding);

            string line = await reader.ReadToEndAsync();

            string[] lines = line.Split(';');

            if (context.ModelType == typeof(ShopCreateModel))
            {
                var shop = new ShopCreateModel() {
                    Name = lines[0],
                    Location = lines[1],
                    ShopTypeId = Convert.ToInt32(lines[2])
                };

                return await InputFormatterResult.SuccessAsync(shop);
            }
            else if (context.ModelType == typeof(ShopEditModel))
            {
                var shop = new ShopEditModel()
                {
                    ShopId = Convert.ToInt32(lines[0]),
                    Name = lines[1],
                    Location = lines[2],
                    ShopTypeId = Convert.ToInt32(lines[3])
                };

                return await InputFormatterResult.SuccessAsync(shop);
            }
            else
            {
                return await InputFormatterResult.FailureAsync();
            }
        }


    }
}
