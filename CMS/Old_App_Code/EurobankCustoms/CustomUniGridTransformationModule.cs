using CMS;
using CMS.Base;
using CMS.DataEngine;
using CMS.UIControls;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.DocumentEngine.Types.Eurobank;
using CMS.SiteProvider;
using CMS.Localization;
using System;

[assembly: RegisterModule(typeof(CustomUniGridTransformationModule))]

public class CustomUniGridTransformationModule : Module
{
    public CustomUniGridTransformationModule(): base("CustomUniGridTransformations")
    {
    }

    protected override void OnInit()
    {
        base.OnInit();

        UniGridTransformations.Global.RegisterTransformation("#customNodeNameFromNodeGUID", CustomNodeNameFromNodeGUID);
    }

    private static object CustomNodeNameFromNodeGUID(object parameter)
    {
        LookupItem lookupItem = LookupItemProvider.GetLookupItem(new Guid(parameter.ToString()), LocalizationContext.CurrentCulture.CultureName, SiteContext.CurrentSiteName)
            .Column("NodeName");
        if (lookupItem != null)
        {
            return lookupItem.NodeName;
        }
        return "N/A";
    }
}