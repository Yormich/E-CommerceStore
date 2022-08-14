using Microsoft.AspNetCore.Razor.TagHelpers;
using E_CommerceStore.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using E_CommerceStore.Utilities;


namespace E_CommerceStore.TagHelpers
{
    [HtmlTargetElement("itemsLayout",ParentTag ="div")]
    public class ShowItemsTagHelper : TagHelper
    {
        public IEnumerable<Item> items { get; set; }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        IImagePathProvider itemImageProvider { get; set; }

        public ShowItemsTagHelper(IEnumerable<Item> items,
            [FromServices] ItemImagePathProvider provider)
        {
            this.items = items;
            itemImageProvider = provider;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if(itemImageProvider is not null)
                Console.WriteLine("PROVIDER SUCCESSFULLY INJECTED");

            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.Add("class", "products-wrapper");
            foreach(Item item in items)
            {
                output.Content.AppendHtml(MakeItemProduct(item));
            }
        }

        public string MakeItemProduct(Item item)
        {
            TagBuilder itemCard = new TagBuilder("div");
            itemCard.Attributes.Add("class", "product-card");

            string ImageSource = itemImageProvider.GetImagePath(ViewContext.HttpContext,
                item.ImageSource);
         
            itemCard.InnerHtml.AppendHtml($"<img src='{ImageSource}'" +
                $"class='product-image'>" +
                $"<a href='{FormProductPageUrl(item.Id)}' class='card-button'>{item.Name}" +
                $"</a>" +
                $"<p class='product-price'>{item.Price} ₴</p>");

            using StringWriter sw = new StringWriter();
            itemCard.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);

            return sw.ToString();
        }

        private string FormProductPageUrl(int itemId)
        {

            HttpRequest request = ViewContext.HttpContext.Request;
            string baseurl = String.Concat(request.Scheme,
                "://", request.Host.ToUriComponent(),
                request.PathBase.ToUriComponent(),
                "/ProductPage/",itemId.ToString());
            Console.WriteLine(baseurl);
            return baseurl;
        }
    }
}
