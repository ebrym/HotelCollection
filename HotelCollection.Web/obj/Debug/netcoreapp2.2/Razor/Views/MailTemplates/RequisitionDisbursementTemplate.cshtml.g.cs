#pragma checksum "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "efbae31eaeb63f115c9e13c90538ffcc26a434328f89cb7dd6e4d90ccc97c23e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_MailTemplates_RequisitionDisbursementTemplate), @"mvc.1.0.view", @"/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml", typeof(AspNetCore.Views_MailTemplates_RequisitionDisbursementTemplate))]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"efbae31eaeb63f115c9e13c90538ffcc26a434328f89cb7dd6e4d90ccc97c23e", @"/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA256", @"27c34cc7a0974fe922d3f4bc244633f49b4b1bdd51ed3b78432ec0b8d7661f71", @"/Views/_ViewImports.cshtml")]
    public class Views_MailTemplates_RequisitionDisbursementTemplate : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<HotelCollection.Web.Models.RequisitionModel>
    {
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(51, 7, true);
            WriteLiteral("<HTML>\n");
            EndContext();
            BeginContext(58, 76, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("HEAD", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "efbae31eaeb63f115c9e13c90538ffcc26a434328f89cb7dd6e4d90ccc97c23e3774", async() => {
                BeginContext(64, 63, true);
                WriteLiteral("\n    <TITLE>CYBERSPACE REQUISITION ::::: NOTIFICATION </TITLE>\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(134, 1, true);
            WriteLiteral("\n");
            EndContext();
            BeginContext(135, 17747, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("BODY", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "efbae31eaeb63f115c9e13c90538ffcc26a434328f89cb7dd6e4d90ccc97c23e5020", async() => {
                BeginContext(141, 848, true);
                WriteLiteral(@"
    <table align=""left"" width=""90%"" border=""1"" cellspacing=""10"" cellpadding=""10"">
        <tr>
            <td align=""center"" valign=""top"">
                <br />
                <font face=""Verdana, Arial, Helvetica, sans-serif"" size=""3"">::::: CYBERSPACE REQUISITION DISBURSEMENT NOTIFICATION :::::</font><br /><br />
                                                                                                                                                     <table border=""1"" style=""border-color: black;"" width=""90%"" cellpadding=""5"" cellspacing=""5"">
                                                                                                                                                         <tr><td align=""left"" colspan=""2"" style=""width: 100%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Dear ");
                EndContext();
                BeginContext(990, 25, false);
#line 13 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                          Write(Model.StaffName.ToUpper());

#line default
#line hidden
                EndContext();
                BeginContext(1015, 33, true);
                WriteLiteral(", <br />Your Request with number ");
                EndContext();
                BeginContext(1049, 8, false);
#line 13 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                     Write(Model.Id);

#line default
#line hidden
                EndContext();
                BeginContext(1057, 473, true);
                WriteLiteral(@" has been disbursed. See details below;</b></font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Requisition Number</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(1531, 8, false);
#line 14 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                       Write(Model.Id);

#line default
#line hidden
                EndContext();
                BeginContext(1539, 423, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Staff Email</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(1963, 16, false);
#line 15 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                Write(Model.StaffEmail);

#line default
#line hidden
                EndContext();
                BeginContext(1979, 422, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Department</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(2402, 16, false);
#line 16 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                               Write(Model.Department);

#line default
#line hidden
                EndContext();
                BeginContext(2418, 416, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Unit</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(2835, 10, false);
#line 17 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                         Write(Model.Unit);

#line default
#line hidden
                EndContext();
                BeginContext(2845, 421, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Job Title</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(3267, 14, false);
#line 18 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                              Write(Model.JobTitle);

#line default
#line hidden
                EndContext();
                BeginContext(3281, 424, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Request Type</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(3706, 21, false);
#line 19 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                 Write(Model.RequisitionType);

#line default
#line hidden
                EndContext();
                BeginContext(3727, 424, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Project Code</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(4152, 17, false);
#line 20 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                 Write(Model.ProjectCode);

#line default
#line hidden
                EndContext();
                BeginContext(4169, 424, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Project Name</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(4594, 17, false);
#line 21 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                 Write(Model.ProjectName);

#line default
#line hidden
                EndContext();
                BeginContext(4611, 420, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Marketer</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(5032, 18, false);
#line 22 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                             Write(Model.MarketerName);

#line default
#line hidden
                EndContext();
                BeginContext(5050, 419, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Remarks</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(5470, 13, false);
#line 23 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                            Write(Model.Remarks);

#line default
#line hidden
                EndContext();
                BeginContext(5483, 427, true);
                WriteLiteral(@"</font></td></tr>
                                                                                                                                                         <tr><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Disurse Remarks</b></font></td><td align=""left"" style=""width: 50%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">");
                EndContext();
                BeginContext(5911, 12, false);
#line 24 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                                                                                                                                                                    Write(Model.Status);

#line default
#line hidden
                EndContext();
                BeginContext(5923, 3407, true);
                WriteLiteral(@"</font></td></tr>

                                                                                                                                                         <tr><td align=""center"" colspan=""2"" style=""width: 100%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>ITEM DETAILS</b></font></td></tr>
                                                                                                                                                         <tr>
                                                                                                                                                             <td align=""left"" colspan=""2"" style=""width: 100%"" valign=""top"">
                                                                                                                                                                 <table border=""1"" style=""border-color: black;width: 100%;"" cellpadding=""5"" cellspacing=""5"">
                                                        ");
                WriteLiteral(@"                                                                                                             <thead class=""thead-light"">
                                                                                                                                                                         <tr>
                                                                                                                                                                             <th align=""left"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>S/N</b></font></th>
                                                                                                                                                                             <th align=""left"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Category</b></font></th>
                                                                                                                                        ");
                WriteLiteral(@"                                     <th align=""left"" style=""width: 100%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Description</b></font></th>
                                                                                                                                                                             <th align=""Center"" style=""width: 100%"" valign=""top""><font align=""left"" face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Quantity</b></font></th>
                                                                                                                                                                             <th align=""Center"" style=""width: 100%"" valign=""top""><font align=""left"" face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>Quantity Issued</b></font></th>
                                                                                                                                                                         </tr>
            ");
                WriteLiteral(@"                                                                                                                                                         </thead>
                                                                                                                                                                     <tbody>
");
                EndContext();
#line 40 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                           
                                                                                                                                                                             int j = 0;
                                                                                                                                                                         

#line default
#line hidden
#line 43 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                          foreach (var item in @Model.Items)
                                                                                                                                                                         {

#line default
#line hidden
                BeginContext(10233, 448, true);
                WriteLiteral(@"                                                                                                                                                                             <tr>
                                                                                                                                                                                 <td align=""left"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><b>");
                EndContext();
                BeginContext(10683, 9, false);
#line 46 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                                                                                                          Write(j = j + 1);

#line default
#line hidden
                EndContext();
                BeginContext(10693, 864, true);
                WriteLiteral(@"</b></font></td>
                                                                                                                                                                                 <td align=""left"" style=""width: 40%"" valign=""top"">
                                                                                                                                                                                     <font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1"">
                                                                                                                                                                                         <b>
                                                                                                                                                                                             ");
                EndContext();
                BeginContext(11558, 17, false);
#line 50 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                        Write(item.ItemCategory);

#line default
#line hidden
                EndContext();
                BeginContext(11575, 1411, true);
                WriteLiteral(@"
                                                                                                                                                                                         </b>
                                                                                                                                                                                     </font>
                                                                                                                                                                                 </td>
                                                                                                                                                                                 <td align=""left"" style=""width: 100%"" valign=""top"">
                                                                                                                                                                                     <font face=""Verdana, Arial, Helvetica, sans-serif"" s");
                WriteLiteral(@"ize=""1"">
                                                                                                                                                                                         <b>
                                                                                                                                                                                             ");
                EndContext();
                BeginContext(12987, 20, false);
#line 57 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                        Write(item.ItemDescription);

#line default
#line hidden
                EndContext();
                BeginContext(13007, 1413, true);
                WriteLiteral(@"
                                                                                                                                                                                         </b>
                                                                                                                                                                                     </font>
                                                                                                                                                                                 </td>
                                                                                                                                                                                 <td align=""center"" style=""width: 100%"" valign=""top"">
                                                                                                                                                                                     <font face=""Verdana, Arial, Helvetica, sans-serif""");
                WriteLiteral(@" size=""1"">
                                                                                                                                                                                         <b>
                                                                                                                                                                                             ");
                EndContext();
                BeginContext(14421, 13, false);
#line 64 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                        Write(item.Quantity);

#line default
#line hidden
                EndContext();
                BeginContext(14434, 1413, true);
                WriteLiteral(@"
                                                                                                                                                                                         </b>
                                                                                                                                                                                     </font>
                                                                                                                                                                                 </td>
                                                                                                                                                                                 <td align=""center"" style=""width: 100%"" valign=""top"">
                                                                                                                                                                                     <font face=""Verdana, Arial, Helvetica, sans-serif""");
                WriteLiteral(@" size=""1"">
                                                                                                                                                                                         <b>
                                                                                                                                                                                             ");
                EndContext();
                BeginContext(15848, 19, false);
#line 71 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                                        Write(item.QuantityIssued);

#line default
#line hidden
                EndContext();
                BeginContext(15867, 742, true);
                WriteLiteral(@"
                                                                                                                                                                                         </b>
                                                                                                                                                                                     </font>
                                                                                                                                                                                 </td>
                                                                                                                                                                             </tr>
");
                EndContext();
#line 76 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
                                                                                                                                                                         }

#line default
#line hidden
                BeginContext(16780, 1010, true);
                WriteLiteral(@"                                                                                                                                                                     </tbody>

                                                                                                                                                                 </table>
                                                                                                                                                             </td>
                                                                                                                                                         </tr>


                                                                                                                                                     </table><br /><br />
            </td>
        </tr>

        <tr><td align=""center"" colspan=""2"" style=""width: 100%"" valign=""top""><font face=""Verdana, Arial, Helvetica, sans-serif"" size=""1""><a");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 17790, "\"", 17811, 1);
#line 88 "/Users/ibrahim.abdullahi/RiderProjects/HotelCollection/HotelCollection.Web/Views/MailTemplates/RequisitionDisbursementTemplate.cshtml"
WriteAttributeValue("", 17797, Model.AppLink, 17797, 14, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(17812, 63, true);
                WriteLiteral("><b>CLICK HERE TO LOGON></b></a></font></td></tr>\n    </table>\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(17882, 8, true);
            WriteLiteral("\n</HTML>");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<HotelCollection.Web.Models.RequisitionModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
