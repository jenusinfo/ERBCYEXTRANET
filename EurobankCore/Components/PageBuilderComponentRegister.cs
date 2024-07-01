using Eurobank;
using Eurobank.Sections;
using Eurobank.PageTemplates;
using Eurobank.Widgets;

using Kentico.PageBuilder.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;
using Eurobank.Components.Widgets.Polls;

// Widgets
[assembly: RegisterWidget(ComponentIdentifiers.TESTIMONIAL_WIDGET, "Testimonial", typeof(TestimonialWidgetProperties), "~/Components/Widgets/TestimonialWidget/_Eurobank_LandingPage_TestimonialWidget.cshtml", Description = "Displays a quotation with its author.", IconClass = "icon-right-double-quotation-mark")]
[assembly: RegisterWidget(ComponentIdentifiers.CTA_BUTTON_WIDGET, "CTA button", typeof(CTAButtonWidgetProperties), "~/Components/Widgets/CTAButton/_Eurobank_General_CTAButtonWidget.cshtml", Description = "Call to action button with configurable target page.", IconClass = "icon-rectangle-a")]
[assembly: RegisterWidget(ComponentIdentifiers.NEWS_WIDGET, typeof(NewsWidgetViewComponenet), "News Widgets", typeof(NewsWidgetProperties))]
[assembly: RegisterWidget(ComponentIdentifiers.WHATS_NEW, typeof(WhatsNewWidgetViewComponent), "News Widgets", typeof(WhatsNewsWidgetProperties))]
[assembly: RegisterWidget(ComponentIdentifiers.OPINION_POLLS, typeof(PollsWidgetViewComponent), "News Widgets", typeof(PollsWidgetsProperties))]
[assembly: RegisterWidget(ComponentIdentifiers.USEFULL_LINKS, typeof(UseFulLinksWidgetViewComponent), "News Widgets", typeof(UseFulLinksWidgetProperties))]

// Sections
[assembly: RegisterSection(ComponentIdentifiers.SINGLE_COLUMN_SECTION, "Single column", typeof(ThemeSectionProperties), "~/Components/Sections/_Eurobank_SingleColumnSection.cshtml", Description = "Single-column section with one zone.", IconClass = "icon-square")]
[assembly: RegisterSection(ComponentIdentifiers.TWO_COLUMN_SECTION, "Two columns", typeof(ThemeSectionProperties), "~/Components/Sections/_Eurobank_TwoColumnSection.cshtml", Description = "Two-column section with two zones.", IconClass = "icon-l-cols-2")]
[assembly: RegisterSection(ComponentIdentifiers.TWO_COLUMN_SECTION_9x3, "Two columns 9x3", typeof(ThemeSectionProperties), "~/Components/Sections/_Eurobank_TwoColumnSection_9x3.cshtml", Description = "Two-column section with two zones.", IconClass = "icon-l-cols-2")]
[assembly: RegisterSection(ComponentIdentifiers.THREE_COLUMN_SECTION, "Three columns", typeof(ThreeColumnSectionProperties), "~/Components/Sections/ThreeColumnSection/_Eurobank_ThreeColumnSection.cshtml", Description = "Three-column section with three zones.", IconClass = "icon-l-cols-3")]

// Page templates
[assembly: RegisterPageTemplate(ComponentIdentifiers.LANDING_PAGE_SINGLE_COLUMN_TEMPLATE, "Single column landing page", typeof(LandingPageSingleColumnProperties), "~/PageTemplates/LandingPage/_Eurobank_LandingPageSingleColumn.cshtml", Description = "A default single column page template with two sections differentiated by a background color.")]