namespace Lipar.Infrastructure.Tools.Utilities.Configurations
{
    public class LiparOptions
    {
        public Swagger Swagger { get; set; }
        public OutBoxEvent OutBoxEvent { get; set; }
        public PoolingPublisher PoolingPublisher { get; set; }
        public ChangesInterceptor EntityChangesInterceptor { get; set; }

    }
}
