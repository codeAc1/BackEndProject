#pragma checksum "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3552358b5e61ac9837374c2866ffb8bb94038589"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__ProductDetailPartial), @"mvc.1.0.view", @"/Views/Shared/_ProductDetailPartial.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 2 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\_ViewImports.cshtml"
using Juan.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\_ViewImports.cshtml"
using Juan.ViewModels;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\_ViewImports.cshtml"
using Juan.Services;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3552358b5e61ac9837374c2866ffb8bb94038589", @"/Views/Shared/_ProductDetailPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b705268eb85f917cf99ee8529dca17fb70bcbc02", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__ProductDetailPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Product>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("product thumb"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString(""), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("basketform"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "get", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "product", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "AddBasket", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral(@"<div class=""modal-header"">
    <button type=""button"" class=""close"" data-dismiss=""modal"">&times;</button>
</div>
<div class=""modal-body"">
    <!-- product details inner end -->
    <div class=""product-details-inner"">
        <div class=""row"">
            <div class=""col-lg-5"">
                <div class=""product-large-slider mb-20"">
                    <div class=""pro-large-img img-zoom"">
                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3552358b5e61ac9837374c2866ffb8bb940385896161", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 453, "~/user/assets/img/product/", 453, 26, true);
#nullable restore
#line 13 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
AddHtmlAttributeValue("", 479, Model.MainImage, 479, 16, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </div>\r\n");
#nullable restore
#line 15 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                     foreach (ProductImage productImage in Model.ProductImages)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"pro-large-img img-zoom\">\r\n                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3552358b5e61ac9837374c2866ffb8bb940385898180", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 753, "~/user/assets/img/product/", 753, 26, true);
#nullable restore
#line 18 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
AddHtmlAttributeValue("", 779, productImage.Image, 779, 19, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                        </div>\r\n");
#nullable restore
#line 20 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                </div>\r\n                <div class=\"pro-nav slick-row-5\">\r\n                    <div class=\"pro-nav-thumb\">\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3552358b5e61ac9837374c2866ffb8bb9403858910197", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1041, "~/user/assets/img/product/", 1041, 26, true);
#nullable restore
#line 26 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
AddHtmlAttributeValue("", 1067, Model.MainImage, 1067, 16, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </div>\r\n");
#nullable restore
#line 28 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                     foreach (ProductImage productImage in Model.ProductImages)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <div class=\"pro-nav-thumb\">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "3552358b5e61ac9837374c2866ffb8bb9403858912180", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 2, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 1302, "~/user/assets/img/product/", 1302, 26, true);
#nullable restore
#line 30 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
AddHtmlAttributeValue("", 1328, productImage.Image, 1328, 19, false);

#line default
#line hidden
#nullable disable
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("</div>\r\n");
#nullable restore
#line 31 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                </div>\r\n            </div>\r\n            <div class=\"col-lg-7\">\r\n                <div class=\"product-details-des\">\r\n                    <h3 class=\"pro-det-title\">");
#nullable restore
#line 37 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                         Write(Model.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h3>\r\n                    <div class=\"pro-review\">\r\n                        <span><a href=\"#\">");
#nullable restore
#line 39 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                     Write(Model.Reviews.Where(r => !r.IsDeleted).Count());

#line default
#line hidden
#nullable disable
            WriteLiteral(" Review(s)</a></span>\r\n                    </div>\r\n                    <div class=\"price-box\">\r\n");
#nullable restore
#line 42 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                         if (Model.DiscountPrice != null)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <span class=\"price-regular\">$");
#nullable restore
#line 44 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                                    Write(Model.DiscountPrice);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n                            <span class=\"price-old\"><del>$");
#nullable restore
#line 45 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                                     Write(Model.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</del></span>\r\n");
#nullable restore
#line 46 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <span class=\"price-regular\">$");
#nullable restore
#line 49 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                                    Write(Model.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</span>\r\n");
#nullable restore
#line 50 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                    </div>
                    <p>Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.</p>
                    <div class=""quantity-cart-box row align-items-center mb-20"">
                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3552358b5e61ac9837374c2866ffb8bb9403858917136", async() => {
                WriteLiteral(@"
                            <div class=""quantity mb-2 "">
                                <div class=""pro-qty""><input id=""productcount"" type=""text"" value=""1""></div>
                            </div>
                            <div class=""costum d-flex mb-3"">
                                <div class=""pro-color "" style=""margin-right:1rem"">
                                    <select class=""form-control color"">
");
#nullable restore
#line 61 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                         foreach (ProductColor productColor in Model.ProductColors)
                                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3552358b5e61ac9837374c2866ffb8bb9403858918226", async() => {
#nullable restore
#line 63 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                                                        Write(productColor.Color.Name);

#line default
#line hidden
#nullable disable
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 63 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                               WriteLiteral(productColor.Id);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 64 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                        }

#line default
#line hidden
#nullable disable
                WriteLiteral("                                    </select>\r\n                                </div>\r\n                                <div class=\"pro-color \">\r\n                                    <select  class=\"form-control size\">\r\n");
#nullable restore
#line 69 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                         foreach (ProductSize productSize in Model.productSizes.OrderBy(s => s.Size.Name))
                                        {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3552358b5e61ac9837374c2866ffb8bb9403858921072", async() => {
#nullable restore
#line 71 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                                                       Write(productSize.Size.Name);

#line default
#line hidden
#nullable disable
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 71 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                               WriteLiteral(productSize.Id);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 72 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                        }

#line default
#line hidden
#nullable disable
                WriteLiteral("                                    </select>\r\n                                </div>\r\n                            </div>\r\n                            <div class=\"product-add-cart\">\r\n");
#nullable restore
#line 77 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                 if (Model.Count > 0)
                                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                    <button   type=\"submit\" class=\"btn btn-default addbasketbtn\">Add to cart</button>\r\n");
#nullable restore
#line 80 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                }
                                else
                                {

#line default
#line hidden
#nullable disable
                WriteLiteral("                                    <button disabled=\"disabled\" class=\"btn btn-default\">Add to cart</button>\r\n");
#nullable restore
#line 84 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                }

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                            </div>\r\n                        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 54 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                                                                                                             WriteLiteral(Model.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                    </div>\r\n                    \r\n                    <div class=\"availability mb-20\">\r\n                        <h5 class=\"cat-title\">Availability:</h5>\r\n");
#nullable restore
#line 92 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                         if (Model.Count > 0)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <span class=\"text-success\">In Stock</span>\r\n");
#nullable restore
#line 95 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                        }
                        else
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <span class=\"text-danger\">Out Stock</span>\r\n");
#nullable restore
#line 99 "C:\Users\Admin\Desktop\Backend-Final\Juan\Juan\Views\Shared\_ProductDetailPartial.cshtml"
                        }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                    </div>
                    <div class=""share-icon"">
                        <h5 class=""cat-title"">Share:</h5>
                        <a href=""#""><i class=""fa fa-facebook""></i></a>
                        <a href=""#""><i class=""fa fa-twitter""></i></a>
                        <a href=""#""><i class=""fa fa-pinterest""></i></a>
                        <a href=""#""><i class=""fa fa-google-plus""></i></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- product details inner end -->
</div>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Product> Html { get; private set; }
    }
}
#pragma warning restore 1591
