using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WebApi.Models;

namespace WebApi.Formatters
{
    public class OneLineOutputFormatter : TextOutputFormatter
    {

        public OneLineOutputFormatter() {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/oneline"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
            SupportedEncodings.Add(Encoding.ASCII);
        }

        public async override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var httpContext = context.HttpContext;

            if (context.Object is IEnumerable<ShopModel> shops) 
            {
                List<string> Shops = new List<string>();

                foreach (var shop in shops)
                {
                    Shops.Add(CreateLine(shop));
                }

                await httpContext.Response.WriteAsync(string.Join("|", Shops));
            }
            else
            {
                await httpContext.Response.WriteAsync(CreateLine(context.Object as ShopModel));
            }

        }

        protected override bool CanWriteType(Type? type)
        {
            return typeof(ShopModel).IsAssignableFrom(type) 
                || typeof(IEnumerable<ShopModel>).IsAssignableFrom(type);
        }

        private string CreateLine(ShopModel model)
        {
            List<string> Lines = new List<string>();

            Lines.Add(model.ShopId.ToString());
            Lines.Add(model.Name);
            Lines.Add(model.Location);
            Lines.Add(model.ShopType);
            Lines.Add(model.ShopId.ToString());

            return string.Join(";", Lines);
        }
    }
}
