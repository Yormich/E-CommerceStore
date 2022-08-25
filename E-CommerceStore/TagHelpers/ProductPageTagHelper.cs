using Microsoft.AspNetCore.Razor.TagHelpers;
using E_CommerceStore.Models.DatabaseModels;
using E_CommerceStore.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace E_CommerceStore.TagHelpers
{
    [HtmlTargetElement("product-page",ParentTag ="div")]
    public class ProductPageTagHelper : TagHelper
    {
        public Item pageitem { get; set; } = null!;

        public IImagePathProvider imageProvider;

        [ViewContext]
        public ViewContext viewContext { get; set; } = null!;

        public ProductPageTagHelper([FromServices] ItemImagePathProvider provider)
        {
            this.imageProvider = provider;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "product-info-wrapper");

            output.Content.SetHtmlContent(MakeProductImage(viewContext.HttpContext, pageitem.ImageSource));

            output.Content.AppendHtml(MakeProductInfo());
        }

        private string MakeProductInfo()
        {
            TagBuilder info = new TagBuilder("div");

            info.Attributes.Add("class", "product-props-wrapper");

            info.InnerHtml.AppendHtml($"<h1><strong>{pageitem.ItemType.Name}: <br />{pageitem.Name}</strong></h1>");

   

            foreach (ItemPropertyCategory category in pageitem.ItemType.itemPropertyCategories)
            {
                TagBuilder categoryBuilder = new TagBuilder("p");
                categoryBuilder.InnerHtml.AppendHtml($"<h3><strong>{category.Name}</strong></h3>");

                TagBuilder propsList = new TagBuilder("dl");

                IEnumerable<ItemProperty> CategoryProperties = pageitem.PersonalProperties
                    .Where(prop => prop.ItemPropertyCategoryId == category.Id);

                foreach (ItemProperty property in CategoryProperties)
                {
                    TagBuilder propName = new TagBuilder("dt");
                    propName.Attributes.Add("class", "dt-custom");
                    propName.InnerHtml.Append($"{property.PropertyName}");
                    
                    TagBuilder propValue = new TagBuilder("dd");
                    propValue.InnerHtml.Append($"{property.PropertyValue}");
                    
                    propsList.InnerHtml.AppendHtml(propName);
                    propsList.InnerHtml.AppendHtml(propValue);
                }
                categoryBuilder.InnerHtml.AppendHtml(propsList);
                info.InnerHtml.AppendHtml(categoryBuilder);
            }
            
            StringWriter sw = new StringWriter();
            info.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
            return sw.ToString();
        }

        private string MakeProductImage(HttpContext context,string? imagePath)
        {
            TagBuilder image = new TagBuilder("div");
            image.Attributes.Add("class","product-page-image-cont");
            string path = imageProvider.GetImagePath(context, imagePath);
            image.InnerHtml.AppendHtml($"<img src='{path}' class='product-page-image'>");

            StringWriter sw = new StringWriter();
            image.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
            return sw.ToString();
        }
    }
}
