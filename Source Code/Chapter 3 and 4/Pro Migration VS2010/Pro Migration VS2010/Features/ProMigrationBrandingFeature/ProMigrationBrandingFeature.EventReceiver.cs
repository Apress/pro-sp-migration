using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace Pro_Migration_VS2010.Features.ProMigrationBrandingFeature
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("f1b2a981-0438-4905-854f-d5ab5c6d9f8e")]
    public class ProMigrationBrandingFeatureEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb currentWeb = properties.Feature.Parent as SPWeb;
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

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPWeb currentWeb = properties.Feature.Parent as SPWeb;
            if (currentWeb != null)
            {
                string webAppRelativePath = currentWeb.ServerRelativeUrl;
                if(!webAppRelativePath.EndsWith("/"))
                {
                    webAppRelativePath += "/";
                }

                Uri masterUri = new Uri(currentWeb.Url + "/_catalogs/masterpage/v4.master");

                currentWeb.MasterUrl = masterUri.AbsolutePath;
                // Very Important for Publishing Site and not required for other templates
                currentWeb.CustomMasterUrl = masterUri.AbsolutePath;
                currentWeb.UIVersion = 4;
                currentWeb.Update();
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
