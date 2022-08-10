using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using E_CommerceStore.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;

namespace E_CommerceStore.TagHelpers
{
    [HtmlTargetElement("tbList",ParentTag="div")]
    public class TypeCatalogListTagHelper : TagHelper
    {

        private string RequestPath = String.Empty; 
        public IEnumerable<ItemType> types { get; set; } = null!;

        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        public TypeCatalogListTagHelper(IEnumerable<ItemType> types)
        {
            this.types = types;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string fullPath = ViewContext.HttpContext.Request.Path;
            if(fullPath.Count(c=>c.Equals('/')) < 2)
            {
                RequestPath = fullPath;
            }
            else
            {
                RequestPath = '/' + fullPath.Split('/',
                    StringSplitOptions.RemoveEmptyEntries)[0];
            }

            output.TagName = "ul";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("class", "list-group bg-silver-sand");
            if (types.Count() == 0)
                throw new ArgumentException("Database doesnt contain any brand");
           

            foreach(ItemType type in types)
            {
                output.Content.AppendHtml(BuildItemTypeLink(type));
            }
        }


        private string BuildItemTypeLink(ItemType type)
        {
            TagBuilder mainListItem = new TagBuilder("li");
            mainListItem.InnerHtml.AppendHtml($"<form method='post' action='{RequestPath}/{type.Id}'" +
                $" class='inline'>" +
                $"<input type='hidden' name='extra_submit_param{type.Id}' value='extra_submit_value" +
                $"{type.Id}'> <button type='submit' name='submit_param{type.Id}' value='submit_value" +
                $"{type.Id}' class='link-button'>" +
                $"{type.Name}</button></form>");

            mainListItem.Attributes.Add("class", "list-group-item list-group-item-action bg-silver-sand");

            TagBuilder typeBrandsList = new TagBuilder("ul");

            foreach (ItemBrand brand in type.Brands)
            {
                typeBrandsList.InnerHtml.AppendHtml(BuildBrandLink(brand, type.Id));
            }

            mainListItem.InnerHtml.AppendHtml(typeBrandsList);

            using StringWriter sw = new StringWriter();
            mainListItem.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
            return sw.ToString();
        }

        private string BuildBrandLink(ItemBrand brand,int parentTypeId)
        {
            TagBuilder brandItem = new TagBuilder("li");
            brandItem.InnerHtml.AppendHtml($"<form method='post' action='{RequestPath}/{parentTypeId}/{brand.Id}'" +
                $" class='inline'>" +
                $"<input type='hidden' name='extra_submit_param{parentTypeId}|{brand.Id}'" +
                $" value='extra_submit_value{parentTypeId}|{brand.Id}" +
                $"{parentTypeId}|{brand.Id}'> <button type='submit' " +
                $"name='submit_param{parentTypeId}|{brand.Id}' value='submit_value" +
                $"{parentTypeId}|{brand.Id}' class='link-button'>" +
                $"{brand.Name}</button></form>");

            brandItem.Attributes.Add("class", "list-group-item list-group-item-action bg-silver-sand");

            using StringWriter sw = new StringWriter();
            brandItem.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
            return sw.ToString();
        }
    }
}
