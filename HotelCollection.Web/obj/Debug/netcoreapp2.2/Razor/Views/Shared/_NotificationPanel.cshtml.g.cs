#pragma checksum "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/Shared/_NotificationPanel.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "2884ea83dbad2c25502c2fc0eec86ee1ce5d563d20217ee4fb7b9639227aeff9"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__NotificationPanel), @"mvc.1.0.view", @"/Views/Shared/_NotificationPanel.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/_NotificationPanel.cshtml", typeof(AspNetCore.Views_Shared__NotificationPanel))]
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
#line 1 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/_ViewImports.cshtml"
using HotelCollection.Web;

#line default
#line hidden
#line 2 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/_ViewImports.cshtml"
using HotelCollection.Web.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"2884ea83dbad2c25502c2fc0eec86ee1ce5d563d20217ee4fb7b9639227aeff9", @"/Views/Shared/_NotificationPanel.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"27c34cc7a0974fe922d3f4bc244633f49b4b1bdd51ed3b78432ec0b8d7661f71", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__NotificationPanel : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 1, true);
            WriteLiteral("\n");
            EndContext();
#line 2 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/Shared/_NotificationPanel.cshtml"
 if (TempData["notification"] != null)
{
    

#line default
#line hidden
            BeginContext(47, 34, false);
#line 4 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/Shared/_NotificationPanel.cshtml"
Write(Html.Raw(TempData["notification"]));

#line default
#line hidden
            EndContext();
#line 4 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/Shared/_NotificationPanel.cshtml"
                                       
}

#line default
#line hidden
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
