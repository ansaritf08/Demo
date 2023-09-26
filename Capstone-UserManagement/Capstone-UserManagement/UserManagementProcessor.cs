using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Publicis.ReportHub.Framework.Config.Interface;
using Publicis.ReportHub.Framework.ConfigProvider.Exceptions;
using Publicis.ReportHub.Framework.DB.Exceptions;
using Publicis.ReportHub.Framework.DTO.DataContracts;
using Publicis.ReportHub.Framework.DTO.MessageContracts;
using Publicis.ReportHub.Framework.Messaging.Exceptions;
using Publicis.ReportHub.Framework.Messaging.Interface;
using Publicis.ReportHub.Framework.Observability.Helpers;
using Publicis.ReportHub.Framework.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Publicis.ReportHub.Services.MessageEligibility
{

    public class UserManagementProcessor : Framework.Messaging.Impl.EventHandler
    {
        private readonly IBlobStorageClient _blobStorageClient;
        private readonly IMessageSender _messageSender;
        string connectionString = "Server=tcp:capstone-dev.database.windows.net,1433;Initial Catalog=usermanagement;Persist Security Info=False;User ID=tadmin;Password=Test#1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public UserManagementProcessor(ILogger<Framework.Messaging.Impl.EventHandler> logger, IEventHubSettings iEventHubSettings, IBlobStorageClient blobStorageClient, IMessageSender messageSender) : base(logger, iEventHubSettings)
        {
            QueueName = "usereh";
            _blobStorageClient = blobStorageClient;
            _messageSender = messageSender;
        }

        protected override async Task ProcessMessage( string rawMessageBody, IDictionary<string, object> userProperties, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(LogMessages.EligibiltyRequestMessageProcessingStarted.Key, string.Format(LogMessages.EligibiltyRequestMessageProcessingStarted.Value, "messageId"));
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var message = JsonConvert.DeserializeObject<Document>(rawMessageBody, serializerSettings);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO userdetails (UserId, UserName,Address,Department,Email) VALUES (@userid, @username,@address,@department,@email)";
                    connection.Open();
                    // Insert data into the database
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //command.Parameters.AddWithValue("@id", message.id);
                        command.Parameters.AddWithValue("@userid", message.UserId);
                        command.Parameters.AddWithValue("@username", message.UserName);
                        command.Parameters.AddWithValue("@address", message.Address);
                        command.Parameters.AddWithValue("@department", message.Department);
                        command.Parameters.AddWithValue("@email", message.Email);
                        // Execute the SQL command
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }



                //DaprRootMessage<TNTMessage> daprRootMessage = new DaprRootMessage<TNTMessage>();
                // TNTMessage tNT = message.Data.Data;


                _logger.LogInformation(LogMessages.ValidEligibiltyRequestMessageProcessingStarted.Key, string.Format(LogMessages.ValidEligibiltyRequestMessageProcessingStarted.Value, ""));
         
              //  await _messageSender.SendMessage("validateeh", daprRootMessage);

               
            }
            catch (ConfigProviderException configProviderException)
            {
                _logger.LogError(LogMessages.ConfigProviderExceptionLog.Key,
                                 configProviderException,
                                string.Format(LogMessages.ConfigProviderExceptionLog.Value, configProviderException.Message));
                throw;
            }
            catch (DatabaseException databaseException)
            {
                _logger.LogError(LogMessages.DatabaseExceptionLog.Key,
                                 databaseException,
                                string.Format(LogMessages.DatabaseExceptionLog.Value, databaseException.Message));
                throw;
            }
            catch (MessageFrameworkException messageFrameworkException)
            {
                _logger.LogError(LogMessages.MessageFrameworkExceptionLog.Key,
                                 messageFrameworkException,
                                string.Format(LogMessages.MessageFrameworkExceptionLog.Value, messageFrameworkException.Message));
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(LogMessages.GeneralExceptionLog.Key,
                                exception,
                                string.Format(LogMessages.GeneralExceptionLog.Value, exception.Message));
            }
        }
    }
}
