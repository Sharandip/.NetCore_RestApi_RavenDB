﻿using Raven.Client.Documents;
using System.Security.Cryptography.X509Certificates;

namespace AGDATAApi.RavenDB
{
    // The `DocumentStoreHolder` class holds a single Document Store instance.
    public class DocumentStoreHolder
    {
        // Use Lazy<IDocumentStore> to initialize the document store lazily. 
        // This ensures that it is created only once - when first accessing the public `Store` property.
        private static Lazy<IDocumentStore> store = new Lazy<IDocumentStore>(CreateStore);

        public static IDocumentStore Store => store.Value;

        private static IDocumentStore CreateStore()
        {
            IDocumentStore store = new DocumentStore()
            {
                // Define the cluster node URLs (required)
                Urls = new[] { "http://192.168.56.1:8080/", 
                           /*some additional nodes of this cluster*/ },

                // Set conventions as necessary (optional)
                Conventions =
            {
                MaxNumberOfRequestsPerSession = 10,
                UseOptimisticConcurrency = true
            },

                // Define a default database (optional)
                Database = "AGDATA",

                // Define a client certificate (optional)
                //Certificate = new X509Certificate2("C:\\path_to_your_pfx_file\\cert.pfx"),

                // Initialize the Document Store
            }.Initialize();

            return store;
        }
    }
}
