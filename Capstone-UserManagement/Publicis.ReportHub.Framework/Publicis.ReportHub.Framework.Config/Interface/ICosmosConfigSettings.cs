namespace Publicis.ReportHub.Framework.Config.Interface
{
    public interface ICosmosConfigSettings
    {
         string CosmosReferencedataContainername { get; set; }

         string CosmosConnectionString { get; set; }

         string CosmosDataBase { get; set; }
    }
}
