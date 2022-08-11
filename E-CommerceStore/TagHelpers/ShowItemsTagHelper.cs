using Microsoft.AspNetCore.Razor.TagHelpers;
using E_CommerceStore.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace E_CommerceStore.TagHelpers
{
    [HtmlTargetElement("itemsLayout",ParentTag ="div")]
    public class ShowItemsTagHelper : TagHelper
    {
        public IEnumerable<Item> items { get; set; }

        private string defaultImagePath = String.Empty;

        public string DefaultImagePath
        {
            get
            {
                return defaultImagePath;
            }
            set
            {
                if(defaultImagePath == String.Empty)
                {
                    defaultImagePath = value;
                }
            }
        }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public ShowItemsTagHelper(IEnumerable<Item> items)
        {
            this.items = items;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            defaultImagePath = Path.Combine(SetDefaultImagePath(), "DefaultItem.png");

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

            string ImageSource = String.IsNullOrEmpty(item.ImageSource) ? defaultImagePath :
                item.ImageSource;
         
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

        private string SetDefaultImagePath()
        {
            HttpRequest request = ViewContext.HttpContext.Request;
            return String.Concat(request.Scheme,
                "://", request.Host.ToUriComponent(),
                request.PathBase.ToUriComponent(),
                "/StaticImages");
        }
    }
}
