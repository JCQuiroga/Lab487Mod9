using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Lab487Mod9.Utils
{
    public class Storage
    {
        private CloudStorageAccount cuentaAlmacenamiento;

        public Storage(String cuenta, String clave)
        {
            StorageCredentials cred = new StorageCredentials(cuenta, clave);
            cuentaAlmacenamiento = new CloudStorageAccount(cred, true);
        }

        private void ComprobarContainer(string contenedor)
        {
            CloudBlobClient blobClient = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(contenedor);
            container.CreateIfNotExists();
        }

        public List<CloudBlockBlob> ListaContenedor(String contenedor)
        {
            ComprobarContainer(contenedor);

            CloudBlobClient cliente = cuentaAlmacenamiento.CreateCloudBlobClient();

            CloudBlobContainer carpeta = cliente.GetContainerReference(contenedor);

            List<CloudBlockBlob> urls = new List<CloudBlockBlob>();

            foreach (IListBlobItem item in carpeta.ListBlobs(null, false))
            {
                if (item is CloudBlockBlob)
                {
                    var blob = item as CloudBlockBlob;

                    urls.Add(blob);
                }
            }
            return urls;
        }

        public void SubirFoto(Stream foto, String nombre, String contenedor)
        {
            CloudBlobClient blobClient = cuentaAlmacenamiento.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference(contenedor);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(nombre);
            blockBlob.UploadFromStream(foto);
            foto.Close();
        }

        public void BorrarFoto(String foto, String contenedor)
        {
            ComprobarContainer(contenedor);
            CloudBlobClient cliente = cuentaAlmacenamiento.CreateCloudBlobClient();
            CloudBlobContainer carpeta = cliente.GetContainerReference(contenedor);
            CloudBlockBlob blockBlob = carpeta.GetBlockBlobReference(foto);
            blockBlob.Delete();
        }
    }
}