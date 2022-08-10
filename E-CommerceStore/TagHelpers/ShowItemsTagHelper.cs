using Microsoft.AspNetCore.Razor.TagHelpers;
using E_CommerceStore.Models.DatabaseModels;

namespace E_CommerceStore.TagHelpers
{
    [HtmlTargetElement("itemsLayout",ParentTag ="div")]
    public class ShowItemsTagHelper : TagHelper
    {
        public IEnumerable<Item> items { get; set; }

        public ShowItemsTagHelper(IEnumerable<Item> items)
        {
            this.items = items;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            foreach(Item item in items)
            {
                output.Content.Append(MakeItemHtmlElement(item));
            }
        }

        public string MakeItemHtmlElement(Item item)
        {
            return item.Name;
        }
    }
}
