namespace Lipar.Infrastructure.Tools.Utilities.Configurations
{
    public class OutBoxEvent
    {
        public string ConnectionString { get; set; }
        public string SelectCommand { get; set; }
        public string UpdateCommand { get; set; }
        public string InsertCommand { get; set; }
    }
}
