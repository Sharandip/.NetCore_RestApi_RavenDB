using Raven.Client.Documents;
using System.Security.Cryptography.X509Certificates;

namespace AGDATAApi.RavenDB
{
    // The `DocumentStoreHolder` class holds a single Document Store instance.
    public class DocumentStoreHolder
    {

        private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

        public static IDocumentStore Store => store.Value;

        private static IDocumentStore CreateStore()
        {
            IDocumentStore store = new DocumentStore()
            {
                Urls = new[] { "http://192.168.56.1:8080/",
                            },
                Conventions =
            {
                MaxNumberOfRequestsPerSession = 10,
                UseOptimisticConcurrency = true
            },
                Database = "AGDATA",

            }.Initialize();

            return store;
        }
    }
}
