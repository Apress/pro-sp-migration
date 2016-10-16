using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Pro_Migration_VS2010.ProMigrationBrandingEventReceiver
{


    /// <summary>
    /// Web Events
    /// </summary>
    public class ProMigrationBrandingEventReceiver : SPWebEventReceiver
    {
       /// <summary>
       /// A site was provisioned
       /// </summary>
       public override void WebProvisioned(SPWebEventProperties properties)
       {
           base.WebProvisioned(properties);

           SPWeb currentWeb = properties.Web;
           if (currentWeb != null)
           {
               string webAppRelativePath = currentWeb.ServerRelativeUrl;
               if (!webAppRelativePath.EndsWith("/"))
               {
                   webAppRelativePath += "/";
               }

               SPFile file = currentWeb.GetFile("/_catalogs/masterpage/_custom.master");
               if (file.Exists)
               {
                   if (file.CustomizedPageStatus == SPCustomizedPageStatus.Customized)
                   {
                       file.RevertContentStream();
                   }
               }

               Uri masterUri = new Uri(currentWeb.Url + "/_catalogs/masterpage/_custom.master");

               currentWeb.MasterUrl = masterUri.AbsolutePath;
               // Very Important for Publishing Site and not required for other templates
               currentWeb.CustomMasterUrl = masterUri.AbsolutePath;
               currentWeb.UIVersion = 4;
               currentWeb.Update();
           }
       }
    }


}
