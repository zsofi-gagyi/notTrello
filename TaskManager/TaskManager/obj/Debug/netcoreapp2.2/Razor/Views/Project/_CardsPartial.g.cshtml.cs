#pragma checksum "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3ce74aa90d4531cb411a23ea1e939bd44348e754"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Project__CardsPartial), @"mvc.1.0.view", @"/Views/Project/_CardsPartial.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Project/_CardsPartial.cshtml", typeof(AspNetCore.Views_Project__CardsPartial))]
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
#line 1 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
using TodoWithDatabase.Models.DAOs;

#line default
#line hidden
#line 2 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
using TodoWithDatabase.Models.DAOs.JoinTables;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3ce74aa90d4531cb411a23ea1e939bd44348e754", @"/Views/Project/_CardsPartial.cshtml")]
    public class Views_Project__CardsPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(87, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 4 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
   foreach (Card card in (List<Card>)ViewData["cards"])
    {

#line default
#line hidden
            BeginContext(153, 41, true);
            WriteLiteral("    <ul class=\"displayCard\">\r\n        <li");
            EndContext();
            BeginWriteAttribute("class", " class=", 194, "", 234, 1);
#line 7 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
WriteAttributeValue("", 201, card.Done ? "done" : "notDone", 201, 33, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(234, 17, true);
            WriteLiteral(">\r\n            <a");
            EndContext();
            BeginWriteAttribute("href", " href=\"", 251, "\"", 300, 4);
#line 8 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
WriteAttributeValue("", 258, card.Project.Id, 258, 16, false);

#line default
#line hidden
            WriteAttributeValue("", 274, "/cards/", 274, 7, true);
#line 8 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
WriteAttributeValue("", 281, card.Id, 281, 8, false);

#line default
#line hidden
            WriteAttributeValue("", 289, "/toggleDone", 289, 11, true);
            EndWriteAttribute();
            BeginContext(301, 1, true);
            WriteLiteral(">");
            EndContext();
            BeginContext(303, 22, false);
#line 8 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
                                                            Write(ViewData["toggleName"]);

#line default
#line hidden
            EndContext();
            BeginContext(325, 47, true);
            WriteLiteral("</a>\r\n        </li>\r\n        <li class=\"title\">");
            EndContext();
            BeginContext(373, 10, false);
#line 10 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
                     Write(card.Title);

#line default
#line hidden
            EndContext();
            BeginContext(383, 39, true);
            WriteLiteral("</li>\r\n        <li class=\"description\">");
            EndContext();
            BeginContext(423, 16, false);
#line 11 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
                           Write(card.Description);

#line default
#line hidden
            EndContext();
            BeginContext(439, 49, true);
            WriteLiteral("</li>\r\n        <li class=\"description\">Deadline: ");
            EndContext();
            BeginContext(489, 13, false);
#line 12 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
                                     Write(card.Deadline);

#line default
#line hidden
            EndContext();
            BeginContext(502, 42, true);
            WriteLiteral("</li>\r\n        <li class=\"responsibles\">\r\n");
            EndContext();
#line 14 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
            foreach (AssigneeCard assigneeCard in card.AssigneeCards)
            {

#line default
#line hidden
            BeginContext(630, 46, true);
            WriteLiteral("                <span class=\"responsibleName\">");
            EndContext();
            BeginContext(677, 30, false);
#line 16 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
                                         Write(assigneeCard.Assignee.UserName);

#line default
#line hidden
            EndContext();
            BeginContext(707, 9, true);
            WriteLiteral("</span>\r\n");
            EndContext();
#line 17 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
            }

#line default
#line hidden
            BeginContext(731, 26, true);
            WriteLiteral("        </li>\r\n    </ul>\r\n");
            EndContext();
#line 20 "D:\work\PROGRAMOZ\newGithub\C#\notTrello\TaskManager\TaskManager\Views\Project\_CardsPartial.cshtml"
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
