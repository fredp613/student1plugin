using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Net;

// Microsoft Dynamics CRM namespace(s)
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace Microsoft.Crm.Sdk.Samples
{
 	public class FollowupPlugin: IPlugin
	{
		/// <summary>
        /// A plug-in that creates a follow-up task activity when a new account is created.
		/// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and asynchronous mode.
        /// </remarks>
        public void Execute(IServiceProvider serviceProvider)
		{
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "tr_inquiry")
                    return;
                using (WebClient client = new WebClient())
                {
                    try
                {

                  	var response = entity.getAttributeValue<string>("tr_response");
                        UpdateAPI(entity.Id.ToString(), response);

                   
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in the FollowupPlugin plug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                    throw;
                }

                }
            }
        }
    }
    
	public void UpdatAPI(string Id, string response) {
		var updateData = "{'InquiryId': "+Id+", 'Response': "+response+"}";
		var client = new WebClient(); 
		client.headers.Add(HttpRequestHeader.ContentType, "application/json"); 
		client.UploadStringAsync(new Uri("http://s4.fredp613.com/api/v1/inquries/"+Id, "PUT", UpdateData);
	}
}
